PixelShaderInput VSMain(VertexShaderInput vertex)
{
    PixelShaderInput result = (PixelShaderInput)0;

    // Change the position vector to be 4 units for matrix transformation
    vertex.position.w = 1.0;
	result.position = mul(vertex.position, vertex.wvproj);
    result.diffuse = vertex.color;
    
    // We use the inverse transpose of the world so that if there is non uniform
    // scaling the normal is transformed correctly. We also use a 3x3 so that 
    // the normal is not affected by translation (i.e. a vector has the same direction
    // and magnitude regardless of translation)
	result.worldNormal = mul(vertex.normal, (float3x3)vertex.winvtrans);
    
    result.worldPosition = mul(vertex.position, vertex.world).xyz;
    
    return result;
}