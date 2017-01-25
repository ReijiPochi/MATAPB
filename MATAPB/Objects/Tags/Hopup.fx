#V
matrix Hopup_world;
#end

#VS
result.position = mul(result.position, Hopup_world);
result.coord = result.position;
result.normal = mul(result.normal, Hopup_world);
#end