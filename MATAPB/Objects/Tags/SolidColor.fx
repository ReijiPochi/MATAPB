
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
#end
##end