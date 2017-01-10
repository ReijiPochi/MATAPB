#V
matrix PSR_world;
#end

#VS
vertex.position = mul(vertex.position, PSR_world);
vertex.normal = mul(vertex.normal, PSR_world);
#end