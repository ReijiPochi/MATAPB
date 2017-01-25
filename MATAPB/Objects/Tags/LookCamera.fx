#V
matrix LookCamera_matrix;
#end

#VS
result.position = mul(result.position, LookCamera_matrix);
result.coord = result.position;
#end