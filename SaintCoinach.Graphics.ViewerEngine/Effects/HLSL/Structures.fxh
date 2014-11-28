struct DirectionalLight
{
    float3 Direction;
    float3 Diffuse;
    float3 Specular;
};

struct ColorPair
{
    float4 Diffuse;
    float4 Specular;
};

struct VSInputDualTexture
{
    float4 Position  : SV_Position;
    float3 Normal    : NORMAL;
    float3 Tangent   : TANGENT;
    float3 Binormal  : BINORMAL;

    float2 TexCoord0 : TEXCOORD0;
    float2 TexCoord1 : TEXCOORD1;
    float4 Blend     : TEXCOORD2;
};

struct VSOutputDualTexture
{
    float4 PositionPS     : SV_Position;
    float3 PositionWS     : TEXCOORD3;
    float3 WorldNormal    : NORMAL;
    float3 WorldTangent   : TANGENT;
    float3 WorldBinormal  : BINORMAL;
    
    float2 TexCoord0  : TEXCOORD0;
    float2 TexCoord1  : TEXCOORD1;
    float4 Blend      : TEXCOORD2;
};

struct VSInputCommon
{
    float4 Position  : SV_Position;
    float3 Normal    : NORMAL;
    float3 Tangent   : TANGENT;
    float3 Binormal  : BINORMAL;

    float2 TexCoord  : TEXCOORD0;
};

struct VSOutputCommon
{
    float4 PositionPS     : SV_Position;
    float3 PositionWS     : TEXCOORD1;
    float3 WorldNormal    : NORMAL;
    float3 WorldTangent   : TANGENT;
    float3 WorldBinormal  : BINORMAL;
    
    float2 TexCoord       : TEXCOORD0;
};