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
};

VSOutput VSCommon(VSInput vin)
{
    VSOutput vout;
    
    vout.UV = vin.UV;

    vout.PositionPS = mul(vin.Position, g_WorldViewProjection);
    vout.PositionWS = mul(vin.Position, g_World);
    vout.NormalWS = normalize(mul(vin.Normal, g_WorldInverseTranspose));

    vout.UV = vin.UV;
    vout.BlendWeight = vin.BlendWeight;
    vout.BlendIndices = vin.BlendIndices;
    vout.Color = vin.Color;

    // Going to pretend these are tangents
    float4 t1 = (vin.Tangent1 - 0.5) * 2.0;
    vout.Tangent1WS.xyz = normalize(mul(t1.xyz, g_WorldInverseTranspose));
    vout.Tangent1WS.w = t1.w;

    float4 t2 = (vin.Tangent2 - 0.5) * 2.0;
    vout.Tangent2WS.xyz = normalize(mul(t2.xyz, g_WorldInverseTranspose));
    vout.Tangent2WS.w = t2.w;
    
    return vout;
};