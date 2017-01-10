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
result *= lambert * light1_color;
result += light1_ambient;
#end
##end

##LAMBERT
#PS
float lambert = dot(-light1_direction, vertex.normal) * light1_lambertConstant.x + light1_lambertConstant.y;
result *= lambert * lambert * light1_color;
result += light1_ambient;
#end
##end