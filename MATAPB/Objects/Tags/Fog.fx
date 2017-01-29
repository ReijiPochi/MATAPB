#PS
float fog_dist = result.g > fog_constant.y ? 0 : result.g;
float fog = saturate(fog_dist - fog_constant.x);
result.color.rgb += fog_color * fog * fog_constant.z;
#end