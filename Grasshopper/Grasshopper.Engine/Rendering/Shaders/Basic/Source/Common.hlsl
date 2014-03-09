cbuffer PerFrame : register(b0)
{
	float3 cameraPosition;
};

struct VertexShaderInput
{
	float4 position  : POSITION;
	float3 normal    : NORMAL;
	float4 color     : COLOR;
	float2 uv        : TEXTUREUV;
	matrix wvproj    : WVPROJ;
	matrix world     : WORLD;
	matrix winvtrans : WINVTRANS;
};

struct PixelShaderInput
{
	float4 position      : SV_Position;
	float4 diffuse       : COLOR;
	float3 worldNormal   : NORMAL;
	float3 worldPosition : WORLDPOS;
};