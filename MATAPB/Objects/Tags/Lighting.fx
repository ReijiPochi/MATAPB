//#V
//cbuffer LightingConstantBuffer
//{
//	float2 Lighting_lambertConstant;
//	float2 Lighting_reserved;
//}
//#end

##SOFT_LAMBERT
#PS
float lambert = dot(-light1_direction, vertex.normal) * light1_lambertConstant.x + light1_lambertConstant.y;
result.color.rgb *= lambert * light1_color;
result.color += light1_ambient;
#end
##end

##LAMBERT
#PS
float lambert = dot(-light1_direction, vertex.normal) * light1_lambertConstant.x + light1_lambertConstant.y;
result.color.rgb *= lambert * lambert * light1_color;
result.color += light1_ambient;
#end
##end