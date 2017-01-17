#V
Texture2D ColorTexture_texture;
float ColorTexture_opacity;
SamplerState ColorTexture_sampler
{
	Filter = ANISOTROPIC;
};
#end

#PS
result = ColorTexture_texture.Sample(ColorTexture_sampler, vertex.texCoord);
result.a *= ColorTexture_opacity;
#end