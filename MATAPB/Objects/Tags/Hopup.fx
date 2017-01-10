#V
matrix Hopup_world;
#end

#VS
vertex.position = mul(vertex.position, Hopup_world);
vertex.normal = mul(vertex.normal, Hopup_world);
#end