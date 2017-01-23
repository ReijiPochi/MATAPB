[maxvertexcount(3)]
void MyGeometryShader(triangle VertexData vertices[3], inout TriangleStream<VertexData> stream)
{
	stream.Append(vertices[0]);
	stream.Append(vertices[1]);
	stream.Append(vertices[2]);
	stream.RestartStrip();
}