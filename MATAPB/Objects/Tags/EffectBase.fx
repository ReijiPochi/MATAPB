$V$

cbuffer WorldConstantBuffer
{
	float4 light1_color;
	float4 light1_direction;
	float4 light1_lambertConstant;
	float4 light1_ambient;
}

struct VertexData
{
	float4 position : SV_Position;
	float4 normal : NORMAL;
	float2 texCoord : TEXCOORD;
};

VertexData MyVertexShader(VertexData vertex)
{

$VS$
	return vertex;
}

float4 MyPixelShader(VertexData vertex) : SV_Target
{
	float4 result = float4(0, 0, 0, 0);

$PS$
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