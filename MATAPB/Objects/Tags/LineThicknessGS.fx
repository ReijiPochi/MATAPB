#V
float LineThicknessGS_halfThickness;
#end

#GS
#define GS
[maxvertexcount(6)]
void MyGeometryShader(line VertexData vertices[2], inout TriangleStream<VertexData> stream)
{
	float3 lineVector = vertices[1].position.xyz - vertices[0].position.xyz;
	float4 v = (float4)0;
	v.xyz = normalize(cross(lineVector, float3(0, 0, 1)));
	v.xyz *= LineThicknessGS_halfThickness;

	VertexData p1 = (VertexData)0, p2 = (VertexData)0, p3 = (VertexData)0, p4 = (VertexData)0;

	float4 pos1 = vertices[0].position - v;
	float4 pos2 = vertices[0].position + v;
	float4 pos3 = vertices[1].position + v;
	float4 pos4 = vertices[1].position - v;

	p1.position = pos1;
	p2.position = pos2;
	p3.position = pos3;
	p4.position = pos4;

	p1.normal = float4(0, 0, 1, 0);
	p2.normal = float4(0, 0, 1, 0);
	p3.normal = float4(0, 0, 1, 0);
	p4.normal = float4(0, 0, 1, 0);

	stream.Append(p1);
	stream.Append(p2);
	stream.Append(p3);
	stream.RestartStrip();

	stream.Append(p1);
	stream.Append(p3);
	stream.Append(p4);
	stream.RestartStrip();
}
#end