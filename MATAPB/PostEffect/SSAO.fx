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

struct ColorOutput
{
	float4 color : SV_Target0;
	float4 g : SV_Target1;
	float z : SV_Target2;
};

VertexData MyVertexShader(VertexData vertex)
{
	return vertex;
}

ColorOutput MyPixelShader(VertexData vertex)
{
	ColorOutput result = (ColorOutput)0;

	result.color = gBuffer.Sample(tex_sampler, vertex.texCoord);
	result.color.a = 0.5;

	return result;
}

technique10 MyTechnique
{
	pass MyPass
	{
		SetVertexShader(CompileShader(vs_5_0, MyVertexShader()));
		SetPixelShader(CompileShader(ps_5_0, MyPixelShader()));
	}
}