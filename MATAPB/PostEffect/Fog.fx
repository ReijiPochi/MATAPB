Texture2DMS<float4, 4> renderTarget;
Texture2DMS<float, 4> zBuffer;

struct VertexData
{
	float4 position : SV_Position;
	float4 normal : NORMAL;
	float2 texCoord : TEXCOORD;
};

VertexData MyVertexShader(VertexData vertex)
{
	vertex.texCoord.x *= 1368;
	vertex.texCoord.y *= 912;
	return vertex;
}

float4 MyPixelShader(VertexData vertex) : SV_Target
{
	int2 index = int2(vertex.texCoord.x, vertex.texCoord.y);
	float4 result;

	for (int i = 0; i < 4; i++)
	{
		float fog = saturate(zBuffer.Load(index, i) - 0.02);
		result += renderTarget.Load(index, i) + fog * 40.0;
	}

	result /= 4;

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