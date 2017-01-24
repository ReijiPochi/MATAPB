$V$

cbuffer WorldConstantBuffer
{
	float4 eye;
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

struct VertexOutput
{
	float4 position : SV_Position;
	float4 normal : NORMAL;
	float2 texCoord : TEXCOORD;
	float4 coord : POSITION0;
};

struct ColorOutput
{
	float4 color : SV_TARGET0;
	float4 z : SV_TARGET1;
	float4 g : SV_TARGET2;
};

VertexOutput MyVertexShader(VertexData vertex)
{
	VertexOutput result = (VertexOutput)0;

	result.position = vertex.position;
	result.normal = vertex.normal;
	result.texCoord = vertex.texCoord;
	result.coord = vertex.position;
$VS$
	return result;
}

$GS$

ColorOutput MyPixelShader(VertexOutput vertex)
{
	ColorOutput result = (ColorOutput)0;
	result.z = vertex.normal / 2.0 + 0.5;
	result.g = distance(vertex.coord, eye) / 1000.0;

$PS$
	return result;
}

technique10 MyTechnique
{
	pass MyPass
	{
		SetVertexShader(CompileShader(vs_5_0, MyVertexShader()));
		SetGeometryShader(CompileShader(gs_5_0, MyGeometryShader()));
		SetPixelShader(CompileShader(ps_5_0, MyPixelShader()));
	}
}