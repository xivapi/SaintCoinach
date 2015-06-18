#include "Structures.fxh"

Texture2D<float4> g_Diffuse     : register(t0);
Texture2D<float4> g_Normal      : register(t1);
Texture2D<float4> g_Specular    : register(t2);
Texture2D<float4> g_Mask        : register(t3);
Texture2D<float4> g_Table       : register(t4);

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
sampler g_NormalPointSampler    : register(s2)
{
    AddressU = Wrap;
    AddressV = Wrap;
    Filter = MIN_MAG_MIP_POINT;
};
sampler g_SpecularSampler       : register(s3)
{
    AddressU = Wrap;
    AddressV = Wrap;
    Filter = MIN_MAG_MIP_LINEAR;
};
sampler g_MaskSampler           : register(s4)
{
    AddressU = Wrap;
    AddressV = Wrap;
    Filter = MIN_MAG_MIP_LINEAR;
};
sampler g_TableSampler          : register(s5)
{
    AddressU = Wrap;
    AddressV = Wrap;
    Filter = MIN_MAG_MIP_LINEAR;
};

#include "Common.Skinned.fxh"
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

float ApplyTable(float2 uv, inout float4 diffuse, inout float4 specular, float specBlendFactor)
{
    TableSamples table = GetTableSamples(uv);

    diffuse.rgb *= lerp(table.Diffuse.rgb, table.Specular.rgb * specBlendFactor, table.Blend.w);
    specular.rgb *= lerp(table.Specular.rgb, specular.rgb, table.Blend.w);

    return table.Specular.w;
};

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

float4 PSDiffuse(VSOutput pin) : SV_Target0
{
    float4 texDiffuse = g_Diffuse.Sample(g_DiffuseSampler, pin.UV.xy);

    return ComputeCommon(pin, texDiffuse, (0).xxxx);
}
float4 PSDiffuseSpecular(VSOutput pin) : SV_Target0
{
    float4 texDiffuse = g_Diffuse.Sample(g_DiffuseSampler, pin.UV.xy);
    float4 texSpecular = g_Specular.Sample(g_SpecularSampler, pin.UV.xy);

    return ComputeCommon(pin, texDiffuse, float4(texSpecular.ggg, 1));
};
float4 PSDiffuseSpecularTable(VSOutput pin) : SV_Target0
{
    float4 texDiffuse = g_Diffuse.Sample(g_DiffuseSampler, pin.UV.xy);
    float4 texSpecular = g_Specular.Sample(g_SpecularSampler, pin.UV.xy);

    float4 diffuse = texDiffuse;
    diffuse.a = 1;
    float4 specular = float4(texSpecular.rrr, 1);
        ApplyTable(pin.UV.xy, diffuse, specular, texSpecular.g);

    return ComputeCommon(pin, diffuse, specular);
};
float4 PSDiffuseTable(VSOutput pin) : SV_Target0
{
    float4 texDiffuse = g_Diffuse.Sample(g_DiffuseSampler, pin.UV.xy);

    float4 diffuse = texDiffuse;
    float4 specular = (1).xxxx;
    ApplyTable(pin.UV.xy, diffuse, specular, 1);

    return ComputeCommon(pin, diffuse, specular);
};
float4 PSMaskTable(VSOutput pin) : SV_Target0
{
    float4 texMask = g_Mask.Sample(g_MaskSampler, pin.UV.xy);

    float4 diffuse = (1).xxxx;
    float4 specular = (1).xxxx;

    float specPow = ApplyTable(pin.UV.xy, diffuse, specular, texMask.g);

    diffuse.rgb *= texMask.r;
    specular.rgb *= texMask.b;

    return ComputeCommon(pin, diffuse, specular);
};
float4 PSMask(VSOutput pin) : SV_Target0
{
    float4 texMask = g_Mask.Sample(g_MaskSampler, pin.UV.xy);

    float4 diffuse = texMask;
    float4 specular = (1).xxxx;

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
technique11 DiffuseTable
{
    pass P0 {
        SetGeometryShader(0);
        SetVertexShader(CompileShader(vs_4_0, VSCommon()));
        SetPixelShader(CompileShader(ps_4_0, PSDiffuseTable()));
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
technique11 Mask
{
    pass P0 {
        SetGeometryShader(0);
        SetVertexShader(CompileShader(vs_4_0, VSCommon()));
        SetPixelShader(CompileShader(ps_4_0, PSMask()));
    }
}