#V
matrix Camera_matrix;
#end

#VS
result.position = mul(result.position, Camera_matrix);
#end