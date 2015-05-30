#include "Structures.fxh"

Texture2D<float4> g_Diffuse     : register(t0);
Texture2D<float4> g_Normal      : register(t1);
Texture2D<float4> g_Mask        : register(t2);

sampler g_DiffuseSampler        : register(s0)
{
    AddressU = Wrap;
    AddressV = Wrap;
    Filter = MIN_MAG_MIP_LINEAR;
};
sampler g_NormalSampler         : register(s1)
{
    AddressU = Wrap;
    AddressV = Wrap;
    Filter = MIN_MAG_MIP_LINEAR;
};
sampler g_MaskSampler           : register(s2)
{
    AddressU = Wrap;
    AddressV = Wrap;
    Filter = MIN_MAG_MIP_LINEAR;
};

#include "Common.Skinned.fxh"
#include "Lighting.fxh"
#include "CustomizeParameter.fxh"

/*
float4 ComputeCommon(VSOutputCommon pin, float4 diffuse, float4 specular)
{
float4 texNormal = g_Normal.Sample(g_NormalSampler, pin.TexCoord);

float3 normal = CalculateNormal(pin.WorldNormal, pin.WorldTangent, pin.WorldBinormal, texNormal.xyz);

float3 eyeVector = normalize(m_EyePosition - pin.PositionWS);
LightResult light = ComputeLights(eyeVector, normal, 3);

float4 color = float4(diffuse.rgb, texNormal.a);

color.rgb *= light.Diffuse.rgb;
color.rgb += light.Specular.rgb * specular.rgb * color.a;

return color;
};*/

float4 ComputeCommon(VSOutput pin, float4 diffuse, float4 specular)
{
    float4 texNormal = g_Normal.Sample(g_NormalSampler, pin.UV.xy);
    float a = texNormal.b;
    clip(a <= 0.5 ? -1 : 1);
    float3 bump = (texNormal.xyz - 0.5) * 2.0;

    float3 binorm = cross(pin.NormalWS.xyz, pin.Tangent1WS.xyz);
    float3 bumpNormal = (bump.x * pin.Tangent1WS) + (bump.y * binorm) + (bump.z * pin.NormalWS);
    bumpNormal = normalize(bumpNormal);

    float3 eyeVector = normalize(m_EyePosition - pin.PositionWS);
    Lighting light = GetLight(m_EyePosition, eyeVector, bumpNormal);

    float4 color = float4(diffuse.rgb, a);

    color.rgb *= light.Diffuse.rgb;
    color.rgb += light.Specular.rgb * specular.rgb * color.a;

    return color;
};

float4 PSSkin(VSOutput pin) : SV_Target0
{
    float4 texDiffuse = g_Diffuse.Sample(g_DiffuseSampler, pin.UV.xy);
    float4 texMask = g_Mask.Sample(g_MaskSampler, pin.UV.xy);

    float3 diffSq = texDiffuse.rgb * texDiffuse.rgb;
    float3 skinMask = m_SkinColor.rgb * texMask.r;
    float3 skinMaskSq = skinMask * diffSq;

    float3 lip = m_LipColor.rgb;
    float3 lipAmount = texMask.b * m_LipColor.a;

    float3 f = lerp(skinMask, lip, lipAmount);

    float4 diffuse = float4(f, 1);

    float4 specular = (0.1).xxxx;

    return ComputeCommon(pin, diffuse, specular);
}

technique11 Skin
{
    pass P0 {
        SetGeometryShader(0);
        SetVertexShader(CompileShader(vs_4_0, VSCommon()));
        SetPixelShader(CompileShader(ps_4_0, PSSkin()));
    }
}