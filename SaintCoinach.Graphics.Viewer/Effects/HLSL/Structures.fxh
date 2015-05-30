struct Lighting
{
    float3 Diffuse;
    float3 Specular;
};
 
struct DirectionalLight
{
	float3 Direction;
	float4 Diffuse;
	float4 Specular;
};

struct VSInput 
{
    float4 Position     : SV_Position;
    float3 Normal       : NORMAL;
    float4 UV           : TEXCOORD0;
    float4 BlendWeight  : BLENDWEIGHT;
    int4   BlendIndices : BLENDINDICES;
    float4 Color        : COLOR;
    float4 Tangent1     : TANGENT0;
    float4 Tangent2     : TANGENT1;
};

struct VSOutput
{
    float4 PositionPS   : SV_Position;
    float3 PositionWS   : POSITIONT;
    float3 NormalWS     : NORMAL;

    float4 UV           : TEXCOORD0;
    float4 BlendWeight  : BLENDWEIGHT;
    int4   BlendIndices : BLENDINDICES;
    float4 Color        : COLOR;
    float4 Tangent1WS   : TEXCOORD5;
    float4 Tangent2WS   : TEXCOORD6;
};