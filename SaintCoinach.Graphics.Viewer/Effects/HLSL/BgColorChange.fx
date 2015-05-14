#include "Structures.fxh"

float4 g_Color                   : register(b2);

Texture2D<float4> g_ColorMap0    : register(t0);
Texture2D<float4> g_NormalMap0   : register(t1);
Texture2D<float4> g_SpecularMap0 : register(t2);

sampler g_ColorMap0Sampler       : register(s0)
{
    AddressU = Wrap;
    AddressV = Wrap;
    Filter = MIN_MAG_MIP_LINEAR;
};
sampler g_Normal0MapSampler      : register(s1)
{
    AddressU = Wrap;
    AddressV = Wrap;
    Filter = MIN_MAG_MIP_LINEAR;
};
sampler g_Specular0MapSampler    : register(s2)
{
    AddressU = Wrap;
    AddressV = Wrap;
    Filter = MIN_MAG_MIP_LINEAR;
};

#include "Common.fxh"
#include "Lighting.fxh"

float4 ComputeCommon(VSOutput pin, float4 diffuse, float4 specular, float3 bump, float3 tangent)
{
    float3 binorm = cross(pin.NormalWS.xyz, tangent.xyz);
    float3 bumpNormal = (bump.x * tangent) + (bump.y * binorm) + (bump.z * pin.NormalWS);
    bumpNormal = normalize(bumpNormal);

    float3 eyeVector = normalize(m_EyePosition - pin.PositionWS);
    Lighting light = GetLight(m_EyePosition, eyeVector, bumpNormal);

    float4 color = (1).xxxx;
    color.rgb = diffuse.rgb * lerp((1).xxx, g_Color.rgb, diffuse.a);

    color.rgb *= light.Diffuse.rgb;
    color.rgb += light.Specular.rgb * specular.rgb * color.a;
    return color;
};

float4 PSColorChange(VSOutput pin) : SV_Target0
{
    float4 texColor0 = g_ColorMap0.Sample(g_ColorMap0Sampler, pin.UV.xy);
    float4 texNormal0 = g_NormalMap0.Sample(g_Normal0MapSampler, pin.UV.xy);
    float4 texSpecular0 = g_SpecularMap0.Sample(g_Specular0MapSampler, pin.UV.xy);

    return ComputeCommon(pin, texColor0, texSpecular0.gggg, (texNormal0.xyz - 0.5) * 2.0, pin.Tangent1WS.xyz);
}

technique11 BgColorChange
{
    pass P0 {
        SetGeometryShader(0);
        SetVertexShader(CompileShader(vs_4_0, VSCommon()));
        SetPixelShader(CompileShader(ps_4_0, PSColorChange()));
    }
}