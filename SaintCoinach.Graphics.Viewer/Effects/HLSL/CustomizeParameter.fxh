
cbuffer g_CustomizeParameters : register(b2)
{
    float4 m_SkinColor      : packoffset(c0);
    float4 m_LipColor       : packoffset(c1);
    float3 m_RightColor     : packoffset(c2);
    float3 m_HairColor      : packoffset(c3);
    float3 m_MeshColor      : packoffset(c4);
    float3 m_LeftColor      : packoffset(c5);
};