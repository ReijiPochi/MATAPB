Texture2D src;
float weights[16];
float2 samplePos[16];
float2 offsetX, offsetY;

SamplerState tex_sampler
{
	Filter = MIN_MAG_LINEAR_MIP_POINT;
	AddressU = MIRROR;
	AddressV = MIRROR;
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
	float2 coord0 : TEXCOORD;
	float2 coord1 : TEXCOORD1;
	float2 coord2 : TEXCOORD2;
	float2 coord3 : TEXCOORD3;
	float2 coord4 : TEXCOORD4;
	float2 coord5 : TEXCOORD5;
	float2 coord6 : TEXCOORD6;
	float2 coord7 : TEXCOORD7;
	float2 coord8 : TEXCOORD8;
	float2 coord9 : TEXCOORD9;
	float2 coord10 : TEXCOORD10;
	float2 coord11 : TEXCOORD11;
	float2 coord12 : TEXCOORD12;
	float2 coord13 : TEXCOORD13;
	float2 coord14 : TEXCOORD14;
	float2 coord15 : TEXCOORD15;
};

VSOut VS_X(VertexData vertex)
{
	VSOut vsout = (VSOut)0;

	vsout.position = vertex.position;

	vsout.coord0 = vertex.texCoord + float2(samplePos[0].x, 0.0);
	vsout.coord1 = vertex.texCoord + float2(samplePos[1].x, 0.0);
	vsout.coord2 = vertex.texCoord + float2(samplePos[2].x, 0.0);
	vsout.coord3 = vertex.texCoord + float2(samplePos[3].x, 0.0);
	vsout.coord4 = vertex.texCoord + float2(samplePos[4].x, 0.0);
	vsout.coord5 = vertex.texCoord + float2(samplePos[5].x, 0.0);
	vsout.coord6 = vertex.texCoord + float2(samplePos[6].x, 0.0);
	vsout.coord7 = vertex.texCoord + float2(samplePos[7].x, 0.0);
	vsout.coord8 = vertex.texCoord + float2(samplePos[8].x, 0.0);
	vsout.coord9 = vertex.texCoord + float2(samplePos[9].x, 0.0);
	vsout.coord10 = vertex.texCoord + float2(samplePos[10].x, 0.0);
	vsout.coord11 = vertex.texCoord + float2(samplePos[11].x, 0.0);
	vsout.coord12 = vertex.texCoord + float2(samplePos[12].x, 0.0);
	vsout.coord13 = vertex.texCoord + float2(samplePos[13].x, 0.0);
	vsout.coord14 = vertex.texCoord + float2(samplePos[14].x, 0.0);
	vsout.coord15 = vertex.texCoord + float2(samplePos[15].x, 0.0);

	return vsout;
}

VSOut VS_Y(VertexData vertex)
{
	VSOut vsout = (VSOut)0;

	vsout.position = vertex.position;

	vsout.coord0 = vertex.texCoord + float2(0.0, samplePos[0].y);
	vsout.coord1 = vertex.texCoord + float2(0.0, samplePos[1].y);
	vsout.coord2 = vertex.texCoord + float2(0.0, samplePos[2].y);
	vsout.coord3 = vertex.texCoord + float2(0.0, samplePos[3].y);
	vsout.coord4 = vertex.texCoord + float2(0.0, samplePos[4].y);
	vsout.coord5 = vertex.texCoord + float2(0.0, samplePos[5].y);
	vsout.coord6 = vertex.texCoord + float2(0.0, samplePos[6].y);
	vsout.coord7 = vertex.texCoord + float2(0.0, samplePos[7].y);
	vsout.coord8 = vertex.texCoord + float2(0.0, samplePos[8].y);
	vsout.coord9 = vertex.texCoord + float2(0.0, samplePos[9].y);
	vsout.coord10 = vertex.texCoord + float2(0.0, samplePos[10].y);
	vsout.coord11 = vertex.texCoord + float2(0.0, samplePos[11].y);
	vsout.coord12 = vertex.texCoord + float2(0.0, samplePos[12].y);
	vsout.coord13 = vertex.texCoord + float2(0.0, samplePos[13].y);
	vsout.coord14 = vertex.texCoord + float2(0.0, samplePos[14].y);
	vsout.coord15 = vertex.texCoord + float2(0.0, samplePos[15].y);

	return vsout;
}

float4 PS_X(VSOut vertex) : SV_Target
{
	float4 result;

	result = (src.Sample(tex_sampler, vertex.coord0) + src.Sample(tex_sampler, vertex.coord15 + offsetX)) * weights[0];
	result += (src.Sample(tex_sampler, vertex.coord1) + src.Sample(tex_sampler, vertex.coord14 + offsetX)) * weights[1];
	result += (src.Sample(tex_sampler, vertex.coord2) + src.Sample(tex_sampler, vertex.coord13 + offsetX)) * weights[2];
	result += (src.Sample(tex_sampler, vertex.coord3) + src.Sample(tex_sampler, vertex.coord12 + offsetX)) * weights[3];
	result += (src.Sample(tex_sampler, vertex.coord4) + src.Sample(tex_sampler, vertex.coord11 + offsetX)) * weights[4];
	result += (src.Sample(tex_sampler, vertex.coord5) + src.Sample(tex_sampler, vertex.coord10 + offsetX)) * weights[5];
	result += (src.Sample(tex_sampler, vertex.coord6) + src.Sample(tex_sampler, vertex.coord9 + offsetX)) * weights[6];
	result += (src.Sample(tex_sampler, vertex.coord7) + src.Sample(tex_sampler, vertex.coord8 + offsetX)) * weights[7];
	result += (src.Sample(tex_sampler, vertex.coord8) + src.Sample(tex_sampler, vertex.coord7 + offsetX)) * weights[8];
	result += (src.Sample(tex_sampler, vertex.coord9) + src.Sample(tex_sampler, vertex.coord6 + offsetX)) * weights[9];
	result += (src.Sample(tex_sampler, vertex.coord10) + src.Sample(tex_sampler, vertex.coord5 + offsetX)) * weights[10];
	result += (src.Sample(tex_sampler, vertex.coord11) + src.Sample(tex_sampler, vertex.coord4 + offsetX)) * weights[11];
	result += (src.Sample(tex_sampler, vertex.coord12) + src.Sample(tex_sampler, vertex.coord3 + offsetX)) * weights[12];
	result += (src.Sample(tex_sampler, vertex.coord13) + src.Sample(tex_sampler, vertex.coord2 + offsetX)) * weights[13];
	result += (src.Sample(tex_sampler, vertex.coord14) + src.Sample(tex_sampler, vertex.coord1 + offsetX)) * weights[14];
	result += (src.Sample(tex_sampler, vertex.coord15) + src.Sample(tex_sampler, vertex.coord0 + offsetX)) * weights[15];

	return result;
}

float4 PS_Y(VSOut vertex) : SV_Target
{
	float4 result = float4(0.0, 0.0, 0.0, 0.0);

	result = (src.Sample(tex_sampler, vertex.coord0) + src.Sample(tex_sampler, vertex.coord15 + offsetY)) * weights[0];
	result += (src.Sample(tex_sampler, vertex.coord1) + src.Sample(tex_sampler, vertex.coord14 + offsetY)) * weights[1];
	result += (src.Sample(tex_sampler, vertex.coord2) + src.Sample(tex_sampler, vertex.coord13 + offsetY)) * weights[2];
	result += (src.Sample(tex_sampler, vertex.coord3) + src.Sample(tex_sampler, vertex.coord12 + offsetY)) * weights[3];
	result += (src.Sample(tex_sampler, vertex.coord4) + src.Sample(tex_sampler, vertex.coord11 + offsetY)) * weights[4];
	result += (src.Sample(tex_sampler, vertex.coord5) + src.Sample(tex_sampler, vertex.coord10 + offsetY)) * weights[5];
	result += (src.Sample(tex_sampler, vertex.coord6) + src.Sample(tex_sampler, vertex.coord9 + offsetY)) * weights[6];
	result += (src.Sample(tex_sampler, vertex.coord7) + src.Sample(tex_sampler, vertex.coord8 + offsetY)) * weights[7];
	result += (src.Sample(tex_sampler, vertex.coord8) + src.Sample(tex_sampler, vertex.coord7 + offsetY)) * weights[8];
	result += (src.Sample(tex_sampler, vertex.coord9) + src.Sample(tex_sampler, vertex.coord6 + offsetY)) * weights[9];
	result += (src.Sample(tex_sampler, vertex.coord10) + src.Sample(tex_sampler, vertex.coord5 + offsetY)) * weights[10];
	result += (src.Sample(tex_sampler, vertex.coord11) + src.Sample(tex_sampler, vertex.coord4 + offsetY)) * weights[11];
	result += (src.Sample(tex_sampler, vertex.coord12) + src.Sample(tex_sampler, vertex.coord3 + offsetY)) * weights[12];
	result += (src.Sample(tex_sampler, vertex.coord13) + src.Sample(tex_sampler, vertex.coord2 + offsetY)) * weights[13];
	result += (src.Sample(tex_sampler, vertex.coord14) + src.Sample(tex_sampler, vertex.coord1 + offsetY)) * weights[14];
	result += (src.Sample(tex_sampler, vertex.coord15) + src.Sample(tex_sampler, vertex.coord0 + offsetY)) * weights[15];

	return result;
}

technique10 MyTechnique
{
	pass PassX
	{
		SetVertexShader(CompileShader(vs_5_0, VS_X()));
		SetPixelShader(CompileShader(ps_5_0, PS_X()));
	}
	pass PassY
	{
		SetVertexShader(CompileShader(vs_5_0, VS_Y()));
		SetPixelShader(CompileShader(ps_5_0, PS_Y()));
	}
}