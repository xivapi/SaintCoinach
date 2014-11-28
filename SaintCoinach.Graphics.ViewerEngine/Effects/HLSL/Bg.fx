#include "Macros.fxh"
#include "Structures.fxh"

DECLARE_TEXTURE(g_Diffuse0,    0)
{
    AddressU = Wrap;
    AddressV = Wrap;
    Filter = MIN_MAG_MIP_LINEAR;
};
DECLARE_TEXTURE(g_Diffuse1,    1)
{
    AddressU = Wrap;
    AddressV = Wrap;
    Filter = MIN_MAG_MIP_LINEAR;
};
DECLARE_TEXTURE(g_Normal0,   2)
{
    AddressU = Wrap;
    AddressV = Wrap;
    Filter = MIN_MAG_MIP_LINEAR;
};
DECLARE_TEXTURE(g_Normal1,   3)
{
    AddressU = Wrap;
    AddressV = Wrap;
    Filter = MIN_MAG_MIP_LINEAR;
};
DECLARE_TEXTURE(g_Specular0, 4)
{
    AddressU = Wrap;
    AddressV = Wrap;
    Filter = MIN_MAG_MIP_LINEAR;
};
DECLARE_TEXTURE(g_Specular1, 5)
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
    float  m_SpecularPower    : packoffset(c3.w);
    DirectionalLight m_Light0 : packoffset(c4);
    DirectionalLight m_Light1 : packoffset(c7);
    DirectionalLight m_Light2 : packoffset(c10);
};

#include "Common.fxh"
#include "Lighting.fxh"

float4 ComputeCommon(VSOutputDualTexture pin, float4 diffuse, float4 mapNormal, float4 specular)
{
    float3 normal = CalculateNormal(pin.WorldNormal, pin.WorldTangent, pin.WorldBinormal, mapNormal.xyz);
    
    float3 eyeVector = normalize(m_EyePosition - pin.PositionWS);
    LightResult light = ComputeLights(eyeVector, normal, 3);
    
    float4 color = diffuse;
    
    color.rgb *= light.Diffuse.rgb;
    color.rgb += light.Specular.rgb * specular * color.a;
    
    return color;
};

float4 PSSingle(VSOutputDualTexture pin) : SV_Target0
{
    float4 texDiffuse0 = g_Diffuse0.Sample(g_Diffuse0Sampler, pin.TexCoord0);
    float4 texNormal0 = g_Normal0.Sample(g_Normal0Sampler, pin.TexCoord0);
    
    return ComputeCommon(pin, texDiffuse0, texNormal0, (0).xxxx);
}
float4 PSSingleSpecular(VSOutputDualTexture pin) : SV_Target0
{
    float4 texDiffuse0 = g_Diffuse0.Sample(g_Diffuse0Sampler, pin.TexCoord0);
    float4 texNormal0 = g_Normal0.Sample(g_Normal0Sampler, pin.TexCoord0);
    float4 texSpecular0 = g_Specular0.Sample(g_Specular0Sampler, pin.TexCoord0);
    
    return ComputeCommon(pin, texDiffuse0, texNormal0, float4(texSpecular0.rrr, 1));
}
float4 PSDual(VSOutputDualTexture pin) : SV_Target0
{
    float4 texDiffuse0 = g_Diffuse0.Sample(g_Diffuse0Sampler, pin.TexCoord0);
    float4 texNormal0 = g_Normal0.Sample(g_Normal0Sampler, pin.TexCoord0);
    
    float4 texDiffuse1 = g_Diffuse1.Sample(g_Diffuse1Sampler, pin.TexCoord1);
    float4 texNormal1 = g_Normal1.Sample(g_Normal1Sampler, pin.TexCoord1);
    
    float4 diffuse = lerp(texDiffuse0, texDiffuse1, pin.Blend.w);
    float4 normal = lerp(texNormal0, texNormal1, pin.Blend.w);
    normal.xyz = normalize(normal.xyz);
    
    return ComputeCommon(pin, diffuse, normal, (0).xxxx);
}
float4 PSDualSpecular(VSOutputDualTexture pin) : SV_Target0
{
    float4 texDiffuse0 = g_Diffuse0.Sample(g_Diffuse0Sampler, pin.TexCoord0);
    float4 texNormal0 = g_Normal0.Sample(g_Normal0Sampler, pin.TexCoord0);
    float4 texSpecular0 = g_Specular0.Sample(g_Specular0Sampler, pin.TexCoord0);
    
    float4 texDiffuse1 = g_Diffuse1.Sample(g_Diffuse1Sampler, pin.TexCoord1);
    float4 texNormal1 = g_Normal1.Sample(g_Normal1Sampler, pin.TexCoord1);
    float4 texSpecular1 = g_Specular1.Sample(g_Specular1Sampler, pin.TexCoord1);
    
    float4 diffuse = lerp(texDiffuse0, texDiffuse1, pin.Blend.w);
    float4 normal = lerp(texNormal0, texNormal1, pin.Blend.w);
    normal.xyz = normalize(normal.xyz);
    
    float3 specular = lerp(texSpecular0.rrr, texSpecular1.rrr, pin.Blend.w);
    
    return ComputeCommon(pin, diffuse, normal, float4(specular, 1));
}

technique11 Single
{
    pass P0 {
        SetGeometryShader(0);
        SetVertexShader(CompileShader(vs_4_0, VSDualTexture()));
        SetPixelShader(CompileShader(ps_4_0, PSSingle()));
    }
}
technique11 SingleSpecular
{
    pass P0 {
        SetGeometryShader(0);
        SetVertexShader(CompileShader(vs_4_0, VSDualTexture()));
        SetPixelShader(CompileShader(ps_4_0, PSSingleSpecular()));
    }
}
technique11 Dual
{
    pass P0 {
        SetGeometryShader(0);
        SetVertexShader(CompileShader(vs_4_0, VSDualTexture()));
        SetPixelShader(CompileShader(ps_4_0, PSDual()));
    }
}
technique11 DualSpecular
{
    pass P0 {
        SetGeometryShader(0);
        SetVertexShader(CompileShader(vs_4_0, VSDualTexture()));
        SetPixelShader(CompileShader(ps_4_0, PSDualSpecular()));
    }
}