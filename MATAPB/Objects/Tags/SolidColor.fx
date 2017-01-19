
##COLOR_OVERWRITE
#V
float4 SolidColor_color;
#end
#PS
result.rgb = SolidColor_color.rgb;
#end
##end

##COLOR_ALPHA_OVERWRITE
#V
float4 SolidColor_color;
#end
#PS
result = SolidColor_color;
#end
##end