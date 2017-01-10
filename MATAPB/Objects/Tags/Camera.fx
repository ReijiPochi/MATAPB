#V
matrix Camera_matrix;
#end

#VS
vertex.position = mul(vertex.position, Camera_matrix);
#end