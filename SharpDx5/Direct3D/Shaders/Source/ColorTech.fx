cbuffer ConstantBuffer : register(b0)
{
	matrix World;
	matrix View;
	matrix Projection;
}

Texture2D image;
SamplerState imageSampler;

struct VOut
{
    float4 position : SV_POSITION;
	float2 texcoord : TEXCOORD;
};

struct VIn
{
	float4 position: POSITION;
	float2 texcoord: TEXCOORD;
	matrix instance: INSTANCE;
};

VOut VShader(VIn input)
{
	VOut output;
	output.position = mul(input.position, input.instance);
	output.position = mul(output.position, View);
	output.position = mul(output.position, Projection);
	output.texcoord = input.texcoord;
	return output;
}

float4 PShader(float4 position : SV_POSITION, float4 color : COLOR) : SV_Target
{
	return image.Sample(imageSampler, input.texcoord);
}

RasterizerState WireframeRS
{
	FillMode = Wireframe;
	CullMode = Back;
	FrontCounterClockwise = false;
};

technique11 ColorTech
{
	pass P0
	{
		SetGeometryShader(0);
		SetVertexShader(CompileShader(vs_5_0, VShader()));
		SetPixelShader(CompileShader(ps_5_0, PShader()));
		
		//SetRasterizerState(WireframeRS);
	}
};