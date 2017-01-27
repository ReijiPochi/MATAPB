
##COLOR_OVERWRITE
#V
float4 SolidColor_color;
#end
#PS
result.color.rgb = SolidColor_color.rgb;
#end
##end

##COLOR_ALPHA_OVERWRITE
#V
float4 SolidColor_color;
#end
#PS
result.color = SolidColor_color;
result.g.a = result.color.a;
result.z.a = result.color.a;
#end
##end