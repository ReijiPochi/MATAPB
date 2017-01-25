Texture2D renderTarget;
Texture2D gBuffer;
Texture2D zBuffer;

SamplerState tex_sampler
{
	Filter = MIN_MAG_LINEAR_MIP_POINT;
};

struct VertexData
{
	float4 position : SV_Position;
	float4 normal : NORMAL;
	float2 texCoord : TEXCOORD;
};

VertexData MyVertexShader(VertexData vertex)
{
	return vertex;
}

float3 randomNormal(float2 tex)
{
	float noiseX = (frac(sin(dot(tex, float2(15.8989f, 76.132f)*1.0f))*46336.23745f));
	float noiseY = (frac(sin(dot(tex, float2(11.9899f, 62.223f)*2.0f))*34748.34744f));
	float noiseZ = (frac(sin(dot(tex, float2(13.3238f, 63.122f)*3.0f))*59998.47362f));
	return normalize(float3(noiseX, noiseY, noiseZ));
}

float4 MyPixelShader(VertexData vertex) : SV_Target
{
	const unsigned int samples = 4;
	//Count of SSAO samples. 0-16, because sampleSphere has only 16 elements

	//SSAO Radius
	const float samplerRadius = 0.0001;

	//Per step strength
	const float strength = 0.5;

	//Total strength
	const float totalStrength = 3.0;

	//Depth falloff min value
	const float falloffMin = 0.00001;

	//Depth falloff max value
	const float falloffMax = 0.01;

	//Random samplekernel
	const float3 sampleSphere[] = {
		float3(0.2024537f, 0.841204f, -0.9060141f),
		float3(-0.2200423f, 0.6282339f,-0.8275437f),
		float3(0.3677659f, 0.1086345f,-0.4466777f),
		float3(0.8775856f, 0.4617546f,-0.6427765f),
		float3(0.7867433f,-0.141479f, -0.1567597f),
		float3(0.4839356f,-0.8253108f,-0.1563844f),
		float3(0.4401554f,-0.4228428f,-0.3300118f),
		float3(0.0019193f,-0.8048455f, 0.0726584f),
		float3(-0.7578573f,-0.5583301f, 0.2347527f),
		float3(-0.4540417f,-0.252365f, 0.0694318f),
		float3(-0.0483353f,-0.2527294f, 0.5924745f),
		float3(-0.4192392f, 0.2084218f,-0.3672943f),
		float3(-0.8433938f, 0.1451271f, 0.2202872f),
		float3(-0.4037157f,-0.8263387f, 0.4698132f),
		float3(-0.6657394f, 0.6298575f, 0.6342437f),
		float3(-0.0001783f, 0.2834622f, 0.8343929f), };

	float3 randNor = randomNormal(vertex.texCoord);//Get random normal 
	float depth = zBuffer.Sample(tex_sampler, vertex.texCoord);//Get linear depth 
	float3 normal = gBuffer.Sample(tex_sampler, vertex.texCoord) * 2.0 - 1.0;//Get view space normal 

	float radius = samplerRadius / depth;
	//We are working in 2D-Space, so we have to scale the 
	//radius by distance, to keep the illusion of a depth scene. 

	float3 centerPos = float3(vertex.texCoord, depth);
	float occ = 0.0;
	for (unsigned int i = 0; i < samples; ++i)
	{
		float3 offset = reflect(sampleSphere[i], randNor);//Reflect sample to random direction. 
		offset = sign(dot(offset, normal))*offset;//Convert to hemisphere. 
		offset.y = -offset.y;
		//Invert hemisphere on the Y-Axis. The texture coordinate increases 
		//from top to buttom, the view normal points opposite, 
		//without inverting the hemisphere points inside the floor, of course you could also 
		//invert the normal buffer's y-axis instead. 

		float3 ray = centerPos + radius*offset;
		//Ray, relative to the current texcoord and scaled using the SSAO radius. 

		if ((saturate(ray.x) != ray.x) || (saturate(ray.y) != ray.y))
			continue;//Skip rays outside the screen 

		float occDepth = zBuffer.SampleLevel(tex_sampler, ray.xy, 0);
		//Get linear depth at ray.xy 
		float3 occNormal = gBuffer.SampleLevel(tex_sampler, ray.xy, 0) * 2.0 - 1.0;
		//Get viewspace normal at ray.xy. 

		float depthDifference = (centerPos.z - occDepth);
		//Difference between depth and occluder depth. 
		float normalDifference = dot(occNormal, normal);
		//Difference(angle) between normal and occlusion normal. 

		float normalOcc = 1.0;
		normalOcc -= saturate(normalDifference);
		//Occlusion dependet on angle between normals, smaller angle causes more occlusion. 
		float depthOcc = step(falloffMin, depthDifference) * (1.0 - smoothstep(falloffMin, falloffMax, depthDifference));
		//Occlusion dependet on depth difference, limited using falloffMin and falloffMax. 
		//helps to reduce self occlusion and halo-artifacts. Try a bit around with falloffMin/Max. 

		occ += saturate(depthOcc*normalOcc*strength);
		//Take all occlusion factors together and scale them using the per step strength. 
	}
	occ /= samples;//Divide by number of samples to get the average occlusion. 

	float4 result;
	result = renderTarget.Sample(tex_sampler, vertex.texCoord);

	result.rgb *= (saturate(pow(1.0f - occ, totalStrength))) * 0.5 + 0.5;
	//Invert the result and potentiate it using the total strength. 

	return result;
	
}

float4 PS(VertexData input) : SV_Target
{
	const unsigned int numSamples = 9;
	const float texelsize = 1.0 / 1000.0;
	//Get the horizontal texel size. 
	const float samplerOffsets[numSamples] = { -8.0, -6.0, -4.0, -2.0, 0.0, 2.0, 4.0, 6.0, 8.0 };
	//Skip every second pixel. 

	float compareDepth = zBuffer.SampleLevel(tex_sampler, input.texCoord, 0).r;
	//Get the linear depth of the current pixel. 
	float result = 0.0;
	float weightSum = 0.0;
	for (unsigned int i = 0; i < numSamples; ++i)
	{
		float2 sampleOffset = float2(texelsize*samplerOffsets[i], 0.0f);
		float2 samplePos = input.texCoord + sampleOffset;
		//Compute sample position for Gaussian blur. 

		float sampleDepth = zBuffer.SampleLevel(tex_sampler, samplePos, 0).r;
		//Get the linear depth of the current sample. 

		float temp = compareDepth;
		temp -= 0.01;
		float weight = ((0.0001 + abs(temp)));
		//Compute bilateral weighting for the current sample, than bigger the 
		//difference between the depth values, than smaller the weighting. 
		//0.0001 is added to avoid division by zero. 

		result += renderTarget.SampleLevel(tex_sampler, samplePos, 0).r * weight;
		//Sample the point and do the weighting. 

		weightSum += weight;
		//Add the weight to the total weight for computing the average later. 
	}
	result /= weightSum;
	//Get average by dividing by the total weight 

	return result;
}

technique10 MyTechnique
{
	pass MyPass
	{
		SetVertexShader(CompileShader(vs_5_0, MyVertexShader()));
		SetPixelShader(CompileShader(ps_5_0, MyPixelShader()));
		//SetPixelShader(CompileShader(ps_5_0, PS()));
	}
}