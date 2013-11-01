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

struct VIn
{
	float3 position: POSITION;
	float4 color: COLOR;
	matrix instance: INSTANCE;
};

VOut VShader(VIn input)
{
	VOut output;
	output.position = mul(input.position, input.instance);
	output.position = mul(output.position, View);
	output.position = mul(output.position, Projection);
	output.color = input.color;
	return output;
}

float4 PShader(float4 position : SV_POSITION, float4 color: COLOR) : SV_Target
{
	return color;
}