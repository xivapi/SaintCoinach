#include "Macros.fxh"
#include "Structures.fxh"

DECLARE_TEXTURE(g_Diffuse, 0)
{
    AddressU = Wrap;
    AddressV = Wrap;
    Filter = MIN_MAG_MIP_LINEAR;
};
DECLARE_TEXTURE(g_Normal, 1)
{
    AddressU = Wrap;
    AddressV = Wrap;
    Filter = MIN_MAG_MIP_LINEAR;
};
DECLARE_TEXTURE(g_Mask, 2)
{
    AddressU = Wrap;
    AddressV = Wrap;
    Filter = MIN_MAG_MIP_LINEAR;
};

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

cbuffer g_LightingParameters : register(b1)
{
    float4 m_DiffuseColor     : packoffset(c0);
    float3 m_EmissiveColor    : packoffset(c1);
    float3 m_AmbientColor     : packoffset(c2);
    float3 m_SpecularColor    : packoffset(c3);
    float  m_SpecularPower : packoffset(c3.w);
    DirectionalLight m_Light0 : packoffset(c4);
    DirectionalLight m_Light1 : packoffset(c7);
    DirectionalLight m_Light2 : packoffset(c10);
};

cbuffer g_CustomizeParameters : register(b2)
{
    float4 m_SkinColor      : packoffset(c0);
    float4 m_LipColor       : packoffset(c1);
};

#include "Common.fxh"
#include "Lighting.fxh"


float4 ComputeCommon(VSOutputCommon pin, float4 diffuse, float4 specular)
{
    float4 texNormal = g_Normal.Sample(g_NormalSampler, pin.TexCoord);

        float3 normal = CalculateNormal(pin.WorldNormal, pin.WorldTangent, pin.WorldBinormal, texNormal.xyz);

        float3 eyeVector = normalize(m_EyePosition - pin.PositionWS);
        LightResult light = ComputeLights(eyeVector, normal, 3);

    float4 color = float4(diffuse.rgb, texNormal.b);

        color.rgb *= light.Diffuse.rgb;
    color.rgb += light.Specular.rgb * specular.rgb * color.a;

    return color;
};

float4 PSSkin(VSOutputCommon pin) : SV_Target0
{
    float4 texDiffuse = g_Diffuse.Sample(g_DiffuseSampler, pin.TexCoord);
    float4 texMask = g_Mask.Sample(g_MaskSampler, pin.TexCoord);

    float3 diffSq = texDiffuse.rgb * texDiffuse.rgb;
    float3 skinMask = m_SkinColor.rgb * texMask.r;
    float3 skinMaskSq = skinMask * diffSq;

    // Double tex coord...
    float3 lip = m_LipColor.rgb;
    float3 lipAmount = texMask.b * m_LipColor.a;

    float3 f = lerp(skinMask, lip, lipAmount);

    float4 diffuse = float4(f, 1);

    return ComputeCommon(pin, diffuse, (0).xxxx);
}

technique11 Skin
{
    pass P0 {
        SetGeometryShader(0);
        SetVertexShader(CompileShader(vs_4_0, VSCommon()));
        SetPixelShader(CompileShader(ps_4_0, PSSkin()));
    }
}