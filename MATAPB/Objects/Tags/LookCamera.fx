#V
matrix LookCamera_matrix;
#end

#VS
vertex.position = mul(vertex.position, LookCamera_matrix);
#end