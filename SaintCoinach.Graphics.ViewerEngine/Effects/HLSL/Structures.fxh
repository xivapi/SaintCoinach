struct PointLight
{
    float3 Direction;
    float3 Diffuse;
    float3 Specular;
};

struct VSInputBg
{
    float4 Position  : SV_Position;
    float3 Normal    : NORMAL;
    float3 Tangent   : TANGENT;
    float3 Binormal  : BINORMAL;

    float2 TexCoord0 : TEXCOORD0;
    float2 TexCoord1 : TEXCOORD1;
    float4 Blend     : TEXCOORD2;
};

struct VSOutputBg
{
    float2 TexCoord0  : TEXCOORD0;
    float2 TexCoord1  : TEXCOORD1;
    float4 Blend      : TEXCOORD2;

    float3 WorldNormal    : NORMAL;
    float3 WorldTangent   : TANGENT;
    float3 WorldBinormal  : BINORMAL;
    float4 PositionPS       : SV_Position;
    float4 PositionWS       : TEXCOORD3;
};

struct VSInputCharacter
{
    float4 Position  : SV_Position;
    float3 Normal    : NORMAL;
    float3 Tangent   : TANGENT;
    float3 Binormal  : BINORMAL;

    float2 TexCoord  : TEXCOORD;
};

struct VSOutputCharacter
{
    float2 TexCoord   : TEXCOORD0;

    float3 WorldNormal    : NORMAL;
    float3 WorldTangent   : TANGENT;
    float3 WorldBinormal  : BINORMAL;
    float4 PositionPS       : SV_Position;
    float4 PositionWS       : TEXCOORD1;
};