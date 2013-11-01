cbuffer ConstantBuffer : register(b0)
{
	matrix World;
	matrix View;
	matrix Projection;
}

struct VOut
{
    float4 position : SV_POSITION;
    float4 color : COLOR;
};

VOut VShader(float4 position : POSITION, float4 color: COLOR)
{
	VOut output;
	output.position = mul(position, World);
	output.position = mul(output.position, View);
	output.position = mul(output.position, Projection);
	output.color = color;
	return output;
}

float4 PShader(float4 position : SV_POSITION, float4 color: COLOR) : SV_Target
{
	return color;
}