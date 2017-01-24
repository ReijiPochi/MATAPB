[maxvertexcount(3)]
void MyGeometryShader(triangle VertexOutput vertices[3], inout TriangleStream<VertexOutput> stream)
{
	stream.Append(vertices[0]);
	stream.Append(vertices[1]);
	stream.Append(vertices[2]);
	stream.RestartStrip();
}