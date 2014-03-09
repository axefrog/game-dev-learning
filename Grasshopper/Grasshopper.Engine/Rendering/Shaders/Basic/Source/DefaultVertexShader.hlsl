PixelShaderInput VSMain(VertexShaderInput input)
{
    PixelShaderInput output = (PixelShaderInput)0;

    // Change the position vector to be 4 units for matrix transformation
    output.position.w = 1.0;
	//result.position = mul(vertex.position, vertex.wvproj);
	output.position = mul(input.position, input.instance);
	output.position = mul(output.position, view);
	output.position = mul(output.position, projection);
	output.diffuse = input.color;
    
    // We use the inverse transpose of the world so that if there is non uniform
    // scaling the normal is transformed correctly. We also use a 3x3 so that 
    // the normal is not affected by translation (i.e. a vector has the same direction
    // and magnitude regardless of translation)
	//result.worldNormal = mul(vertex.normal, (float3x3)vertex.winvtrans);
    
    //result.worldPosition = mul(vertex.position, vertex.world).xyz;
    
    return output;
}