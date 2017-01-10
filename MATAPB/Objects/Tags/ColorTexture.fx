#V
Texture2D ColorTexture_texture;
SamplerState ColorTexture_sampler
{
};
#end

#PS
result = ColorTexture_texture.Sample(ColorTexture_sampler, vertex.texCoord);
#end