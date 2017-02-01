#V
Texture2D ColorTexture_texture;
float ColorTexture_opacity;
SamplerState ColorTexture_sampler;
#end

#PS
result.color = ColorTexture_texture.Sample(ColorTexture_sampler, vertex.texCoord);
result.color.a *= ColorTexture_opacity;
if (gzBufferOn)
{
	result.g.a = result.color.a;
	result.z.a = result.color.a;
}
#end