﻿$V$

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

struct ColorOutput
{
	float4 color : SV_TARGET0;
	float4 g : SV_TARGET1;
	float z : SV_TARGET2;
};

VertexData MyVertexShader(VertexData vertex)
{

$VS$
	return vertex;
}

$GS$

ColorOutput MyPixelShader(VertexData vertex)
{
	ColorOutput result = (ColorOutput)0;
	result.g = vertex.normal;

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