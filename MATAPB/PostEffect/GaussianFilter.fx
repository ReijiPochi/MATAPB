﻿Texture2D src;
float weights[8];
float2 samplePos[8];
float2 offsetX, offsetY;

SamplerState tex_sampler
{
	Filter = MIN_MAG_LINEAR_MIP_POINT;
	AddressU = Mirror;
	AddressV = Mirror;
};

struct VertexData
{
	float4 position : SV_Position;
	float4 normal : NORMAL;
	float2 texCoord : TEXCOORD;
};

struct VSOut
{
	float4 position : SV_Position;
	float2 coord0 : TEXCOORD0;
	float2 coord1 : TEXCOORD1;
	float2 coord2 : TEXCOORD2;
	float2 coord3 : TEXCOORD3;
	float2 coord4 : TEXCOORD4;
	float2 coord5 : TEXCOORD5;
	float2 coord6 : TEXCOORD6;
	float2 coord7 : TEXCOORD7;
};

VSOut VS_X(VertexData vertex)
{
	VSOut vsout = (VSOut)0;

	//vsout.position = vertex.position;

	//vsout.coord0 = vertex.texcoord + float2(samplePos[0].x, 0.0);
	//vsout.coord1 = vertex.texcoord + float2(samplePos[1].x, 0.0);
	//vsout.coord2 = vertex.texcoord + float2(samplePos[2].x, 0.0);
	//vsout.coord3 = vertex.texcoord + float2(samplePos[3].x, 0.0);
	//vsout.coord4 = vertex.texcoord + float2(samplePos[4].x, 0.0);
	//vsout.coord5 = vertex.texcoord + float2(samplePos[5].x, 0.0);
	//vsout.coord6 = vertex.texcoord + float2(samplePos[6].x, 0.0);
	//vsout.coord7 = vertex.texcoord + float2(samplePos[7].x, 0.0);

	return vsout;
}

VSOut VS_Y(VertexData vertex)
{
	VSOut vsout = (VSOut)0;

	//vsout.position = vertex.position;

	//vsout.coord0 = vertex.texcoord + float2(0.0, samplePos[0].y);
	//vsout.coord1 = vertex.texcoord + float2(0.0, samplePos[1].y);
	//vsout.coord2 = vertex.texcoord + float2(0.0, samplePos[2].y);
	//vsout.coord3 = vertex.texcoord + float2(0.0, samplePos[3].y);
	//vsout.coord4 = vertex.texcoord + float2(0.0, samplePos[4].y);
	//vsout.coord5 = vertex.texcoord + float2(0.0, samplePos[5].y);
	//vsout.coord6 = vertex.texcoord + float2(0.0, samplePos[6].y);
	//vsout.coord7 = vertex.texcoord + float2(0.0, samplePos[7].y);

	return vsout;
}

float4 PS_X(VSOut vertex) : SV_Target
{
	float4 reault = float4(0.0, 0.0, 0.0, 0.0);

	//reault = (src.Sample(tex_sampler, vertex.coord0) + src.Sample(tex_sampler, vertex.coord7 + offsetX)) * weights[0];
	//reault += (src.Sample(tex_sampler, vertex.coord1) + src.Sample(tex_sampler, vertex.coord6 + offsetX)) * weights[1];
	//reault += (src.Sample(tex_sampler, vertex.coord2) + src.Sample(tex_sampler, vertex.coord5 + offsetX)) * weights[2];
	//reault += (src.Sample(tex_sampler, vertex.coord3) + src.Sample(tex_sampler, vertex.coord4 + offsetX)) * weights[3];
	//reault += (src.Sample(tex_sampler, vertex.coord4) + src.Sample(tex_sampler, vertex.coord3 + offsetX)) * weights[4];
	//reault += (src.Sample(tex_sampler, vertex.coord5) + src.Sample(tex_sampler, vertex.coord2 + offsetX)) * weights[5];
	//reault += (src.Sample(tex_sampler, vertex.coord6) + src.Sample(tex_sampler, vertex.coord1 + offsetX)) * weights[6];
	//reault += (src.Sample(tex_sampler, vertex.coord7) + src.Sample(tex_sampler, vertex.coord0 + offsetX)) * weights[7];

	return result;
}

float4 PS_Y(VSOut vertex) : SV_Target
{
	float4 reault = float4(0.0, 0.0, 0.0, 0.0);

	//reault = (src.Sample(tex_sampler, vertex.coord0) + src.Sample(tex_sampler, vertex.coord7 + offsetY)) * weights[0];
	//reault += (src.Sample(tex_sampler, vertex.coord1) + src.Sample(tex_sampler, vertex.coord6 + offsetY)) * weights[1];
	//reault += (src.Sample(tex_sampler, vertex.coord2) + src.Sample(tex_sampler, vertex.coord5 + offsetY)) * weights[2];
	//reault += (src.Sample(tex_sampler, vertex.coord3) + src.Sample(tex_sampler, vertex.coord4 + offsetY)) * weights[3];
	//reault += (src.Sample(tex_sampler, vertex.coord4) + src.Sample(tex_sampler, vertex.coord3 + offsetY)) * weights[4];
	//reault += (src.Sample(tex_sampler, vertex.coord5) + src.Sample(tex_sampler, vertex.coord2 + offsetY)) * weights[5];
	//reault += (src.Sample(tex_sampler, vertex.coord6) + src.Sample(tex_sampler, vertex.coord1 + offsetY)) * weights[6];
	//reault += (src.Sample(tex_sampler, vertex.coord7) + src.Sample(tex_sampler, vertex.coord0 + offsetY)) * weights[7];

	return result;
}

technique10 MyTechnique
{
	pass Pass_X
	{
		SetVertexShader(CompileShader(vs_5_0, VS_X()));
		SetPixelShader(CompileShader(ps_5_0, PS_X()));
	}
	pass Pass_Y
	{
		SetVertexShader(CompileShader(vs_5_0, VS_Y()));
		SetPixelShader(CompileShader(ps_5_0, PS_Y()));
	}
}