float4 PSMain(PixelShaderInput pixel) : SV_Target
{
	return pixel.diffuse;
}