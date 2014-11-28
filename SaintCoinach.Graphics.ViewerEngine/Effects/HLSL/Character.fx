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
sampler g_NormalPointSampler : register(s2)
{
    AddressU = Wrap;
    AddressV = Wrap;
    Filter = MIN_MAG_MIP_POINT;
};
DECLARE_TEXTURE(g_Specular, 3)
{
    AddressU = Wrap;
    AddressV = Wrap;
    Filter = MIN_MAG_MIP_LINEAR;
};
DECLARE_TEXTURE(g_Mask, 4)
{
    AddressU = Wrap;
    AddressV = Wrap;
    Filter = MIN_MAG_MIP_LINEAR;
};
DECLARE_TEXTURE(g_Table, 5)
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

struct TableSamples
{
    float4 Diffuse;
    float4 Specular;
    float4 Blend;
};

TableSamples GetTableSamples(float2 uv)
{
    float4 texNormalPoint = g_Normal.Sample(g_NormalPointSampler, uv);

        int tableKey = floor(texNormalPoint.a * 15);
    float tableY = (tableKey + 0.5) * 0.0625;

    float2 diffuseUV = float2(0.125, tableY);
        float2 specularUV = float2(0.375, tableY);
        float2 blendUV = float2(0.625, tableY);

        TableSamples samples;

    samples.Diffuse = g_Table.Sample(g_TableSampler, diffuseUV);
    samples.Specular = g_Table.Sample(g_TableSampler, specularUV);
    samples.Blend = g_Table.Sample(g_TableSampler, blendUV);

    return samples;
};

float ApplyTable(float2 uv, inout float4 diffuse, inout float4 specular, bool blendSpecIntoDiff)
{
    TableSamples table = GetTableSamples(uv);

    float3 diff = table.Diffuse.rgb;
        if (blendSpecIntoDiff)
        {
            diff += table.Blend.w * table.Specular.rgb;
        }
    diffuse.rgb *= diff;
    specular = table.Specular * specular.g;

    return table.Specular.w;
};

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

float4 PSDiffuse(VSOutputCommon pin) : SV_Target0
{
    float4 texDiffuse = g_Diffuse.Sample(g_DiffuseSampler, pin.TexCoord);

    return ComputeCommon(pin, texDiffuse, (0).xxxx);
};
float4 PSDiffuseSpecular(VSOutputCommon pin) : SV_Target0
{
    float4 texDiffuse = g_Diffuse.Sample(g_DiffuseSampler, pin.TexCoord);
    float4 texSpecular = g_Specular.Sample(g_SpecularSampler, pin.TexCoord);

    return ComputeCommon(pin, texDiffuse, texSpecular);
};
float4 PSDiffuseSpecularTable(VSOutputCommon pin) : SV_Target0
{
    float4 texDiffuse = g_Diffuse.Sample(g_DiffuseSampler, pin.TexCoord);
    float4 texSpecular = g_Specular.Sample(g_SpecularSampler, pin.TexCoord);

    float4 diffuse = texDiffuse;
    float4 specular = float4(texSpecular.ggg, 1);

    ApplyTable(pin.TexCoord, diffuse, specular, false);

    return ComputeCommon(pin, diffuse, diffuse);
};
float4 PSMaskTable(VSOutputCommon pin) : SV_Target0
{
    float4 texMask = g_Mask.Sample(g_MaskSampler, pin.TexCoord);

    float4 diffuse = (1).xxxx;
    float4 specular = (1).xxxx;


    float specPow = ApplyTable(pin.TexCoord, diffuse, specular, true);

    diffuse.rgb *= texMask.b;


    return ComputeCommon(pin, diffuse, specular);
};


technique11 Diffuse
{
    pass P0 {
        SetGeometryShader(0);
        SetVertexShader(CompileShader(vs_4_0, VSCommon()));
        SetPixelShader(CompileShader(ps_4_0, PSDiffuse()));
    }
}
technique11 DiffuseSpecular
{
    pass P0 {
        SetGeometryShader(0);
        SetVertexShader(CompileShader(vs_4_0, VSCommon()));
        SetPixelShader(CompileShader(ps_4_0, PSDiffuseSpecular()));
    }
}
technique11 DiffuseSpecularTable
{
    pass P0 {
        SetGeometryShader(0);
        SetVertexShader(CompileShader(vs_4_0, VSCommon()));
        SetPixelShader(CompileShader(ps_4_0, PSDiffuseSpecularTable()));
    }
}
technique11 MaskTable
{
    pass P0 {
        SetGeometryShader(0);
        SetVertexShader(CompileShader(vs_4_0, VSCommon()));
        SetPixelShader(CompileShader(ps_4_0, PSMaskTable()));
    }
}