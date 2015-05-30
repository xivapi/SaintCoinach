#define MAX_BONES 64

row_major float4x4 g_World;
row_major float4x4 g_WorldInverseTranspose;
row_major float4x4 g_WorldViewProjection;

cbuffer g_CameraParameters : register(b0)
{
    row_major float3x4 m_View                       : packoffset(c0);
    row_major float4x3 m_ViewInverse                : packoffset(c4);
    row_major float4x4 m_ViewProjection             : packoffset(c8);
    row_major float4x4 m_ViewProjectionInverse      : packoffset(c12);
    row_major float4x4 m_Projection                 : packoffset(c16);
    row_major float4x4 m_ProjectionInverse          : packoffset(c20);

    float3 m_EyePosition                            : packoffset(c24);
    float4x3 g_JointMatrixArray[64]       : packoffset(c25);
};

void ApplySkinning(inout VSInput vin)
{
    float4 pos = 0;
    float3 norm = 0;
    float3 t1 = 0;
    float3 t2 = 0;

    [unroll]
    for(int i = 0; i < 4; i++)
    {
        float4x3 joint = g_JointMatrixArray[vin.BlendIndices[i]];
        float w = vin.BlendWeight[i];

        pos.xyz += mul(vin.Position, joint) * w;
        norm += mul(vin.Normal, joint) * w;
        t1 += mul(vin.Tangent1, joint) * w;
        t2 += mul(vin.Tangent2, joint) * w;
    }

    vin.Position.xyz = pos.xyz;

    vin.Normal = normalize(norm);
    vin.Tangent1.xyz = normalize(t1);
    vin.Tangent2.xyz = normalize(t2);
};

VSOutput VSCommon(VSInput vin)
{
    VSOutput vout;
    
    vout.UV = vin.UV;

    vin.Tangent1 = (vin.Tangent1 - 0.5) * 2.0;
    vin.Tangent2 = (vin.Tangent2 - 0.5) * 2.0;

    ApplySkinning(vin);

    vout.PositionPS = mul(vin.Position, g_WorldViewProjection);
    vout.PositionWS = mul(vin.Position, g_World);
    vout.NormalWS = normalize(mul(vin.Normal, g_WorldInverseTranspose));

    vout.UV = vin.UV;
    vout.BlendWeight = vin.BlendWeight;
    vout.BlendIndices = vin.BlendIndices;
    vout.Color = vin.Color;

    // Going to pretend these are tangents
    vout.Tangent1WS.xyz = normalize(mul(vin.Tangent1.xyz, g_WorldInverseTranspose));
    vout.Tangent1WS.w = vin.Tangent1.w;

    vout.Tangent2WS.xyz = normalize(mul(vin.Tangent2.xyz, g_WorldInverseTranspose));
    vout.Tangent2WS.w = vin.Tangent2.w;
    
    return vout;
};