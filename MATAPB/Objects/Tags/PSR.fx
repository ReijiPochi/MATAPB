#V
matrix PSR_world;
#end

#VS
result.position = mul(result.position, PSR_world);
result.normal = mul(result.normal, PSR_world);
#end