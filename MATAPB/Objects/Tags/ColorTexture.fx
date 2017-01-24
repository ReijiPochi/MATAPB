##FILTER_ANISOTROPIC
#V
Texture2D ColorTexture_texture;
float ColorTexture_opacity;
SamplerState ColorTexture_sampler
{
	Filter = ANISOTROPIC;
};
#end
##end

##FILTER_CLEAR
#V
Texture2D ColorTexture_texture;
float ColorTexture_opacity;
SamplerState ColorTexture_sampler
{
	Filter = MIN_LINEAR_MAG_POINT_MIP_LINEAR;
};
#end
##end

##COMMON
#PS
result.color = ColorTexture_texture.Sample(ColorTexture_sampler, vertex.texCoord);
result.color.a *= ColorTexture_opacity;
#end
##end