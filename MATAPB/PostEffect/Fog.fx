Texture2D renderTarget;
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

float4 MyPixelShader(VertexData vertex) : SV_Target
{
	float4 result;
	result = renderTarget.Sample(tex_sampler, vertex.texCoord);

	float fog = saturate(zBuffer.Sample(tex_sampler, vertex.texCoord) - 0.02);
	if (fog > 0.3) fog = 0;

	result.rg += fog * 4.0;
	result.b += fog * 4.5;

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