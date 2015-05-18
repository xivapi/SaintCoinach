#include "Structures.fxh"

Texture2D<float4> g_Diffuse0     : register(t0);
Texture2D<float4> g_Normal0      : register(t1);
Texture2D<float4> g_Specular0    : register(t2);
Texture2D<float4> g_Diffuse1     : register(t3);
Texture2D<float4> g_Normal1      : register(t4);
Texture2D<float4> g_Specular1    : register(t5);

sampler g_Diffuse0Sampler        : register(s0)
{
    AddressU = Wrap;
    AddressV = Wrap;
    Filter = MIN_MAG_MIP_LINEAR;
};
sampler g_Normal0Sampler         : register(s1)
{
    AddressU = Wrap;
    AddressV = Wrap;
    Filter = MIN_MAG_MIP_LINEAR;
};
sampler g_Specular0Sampler       : register(s2)
{
    AddressU = Wrap;
    AddressV = Wrap;
    Filter = MIN_MAG_MIP_LINEAR;
};
sampler g_Diffuse1Sampler        : register(s3)
{
    AddressU = Wrap;
    AddressV = Wrap;
    Filter = MIN_MAG_MIP_LINEAR;
};
sampler g_Normal1Sampler         : register(s4)
{
    AddressU = Wrap;
    AddressV = Wrap;
    Filter = MIN_MAG_MIP_LINEAR;
};
sampler g_Specular1Sampler       : register(s5)
{
    AddressU = Wrap;
    AddressV = Wrap;
    Filter = MIN_MAG_MIP_LINEAR;
};

#include "Common.fxh"
#include "Lighting.fxh"

/*
float4 ComputeCommon(VSOutputDualTexture pin, float4 diffuse, float4 mapNormal, float4 specular)
{
    //float3 normal = CalculateNormal(pin.WorldNormal, pin.WorldTangent, pin.WorldBinormal, mapNormal.xyz);
    float3 normal = pin.WorldNormal; // XXX: My tangent calculation results in 0/0/0 at times, that makes things fail.
    
    float3 eyeVector = normalize(m_EyePosition - pin.PositionWS);
    LightResult light = ComputeLights(eyeVector, normal, 3);
    
    float4 color = diffuse;
    
    color.rgb *= light.Diffuse.rgb;
    color.rgb += light.Specular.rgb * specular * color.a;
    
    return color;
};
*/
float4 ComputeCommon(VSOutput pin, float4 diffuse, float4 specular, float3 bump, float3 tangent)
{
    float3 binorm = cross(pin.NormalWS.xyz, tangent.xyz);
    float3 bumpNormal = (bump.x * tangent) + (bump.y * binorm) + (bump.z * pin.NormalWS);
    bumpNormal = normalize(bumpNormal);

    float3 eyeVector = normalize(m_EyePosition - pin.PositionWS);
    Lighting light = GetLight(m_EyePosition, eyeVector, bumpNormal);

    float4 color = diffuse;
    clip(color.a <= 0.5 ? -1 : 1);
    color.a = 1;

    color.rgb *= light.Diffuse.rgb;
    color.rgb += light.Specular.rgb * specular.rgb * color.a;
    return color;
};

bool IsDummy(float4 value)
{
    // TODO: Actually figure out what's wrong here.
    if (value.x == value.y && value.y == value.z && value.z == value.w && value.w == 1)
    {
        return true;
    }
    return false;
}


VSOutput VSUvScroll(VSInput vin)
{
    VSOutput vout;

    vout.PositionPS = mul(vin.Position, g_WorldViewProjection);
    vout.PositionWS = mul(vin.Position, g_World);
    vout.NormalWS = normalize(mul(vin.Normal, g_WorldInverseTranspose));

    vout.UV = vin.UV;   // TODO: Figured this out.
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

float4 PSSingle(VSOutput pin) : SV_Target0
{
    float4 texDiffuse0 = g_Diffuse0.Sample(g_Diffuse0Sampler, pin.UV.xy);
    float4 texNormal0 = g_Normal0.Sample(g_Normal0Sampler, pin.UV.xy);

    return ComputeCommon(pin, texDiffuse0, (0).xxxx, (texNormal0.xyz - 0.5) * 2.0, pin.Tangent1WS.xyz);
}
float4 PSSingleSpecular(VSOutput pin) : SV_Target0
{
    float4 texDiffuse0 = g_Diffuse0.Sample(g_Diffuse0Sampler, pin.UV.xy);
    float4 texSpecular0 = g_Specular0.Sample(g_Specular0Sampler, pin.UV.xy);
    float4 texNormal0 = g_Normal0.Sample(g_Normal0Sampler, pin.UV.xy);

    return ComputeCommon(pin, texDiffuse0, float4(texSpecular0.rrr, 1), (texNormal0.xyz - 0.5) * 2.0, pin.Tangent1WS.xyz);
}

float4 PSDual(VSOutput pin) : SV_Target0
{
    float4 texDiffuse0 = g_Diffuse0.Sample(g_Diffuse0Sampler, pin.UV.xy);
    float4 texNormal0 = g_Normal0.Sample(g_Normal0Sampler, pin.UV.xy);

    float4 texDiffuse1 = g_Diffuse1.Sample(g_Diffuse1Sampler, pin.UV.zw);
    float4 texNormal1 = g_Normal1.Sample(g_Normal1Sampler, pin.UV.zw);

    if (IsDummy(texDiffuse1))
    {
        texDiffuse1 = texDiffuse0;
    }
    if (IsDummy(texNormal1))
    {
        texNormal1 = texNormal0;
    }

    float4 diffuse = lerp(texDiffuse0, texDiffuse1, pin.Color.w);
    float3 bump = (texNormal0.xyz - 0.5) * 2.0;// lerp((texNormal0.xyz - 0.5) * 2.0, (texNormal1.xyz - 0.5) * 2.0, pin.Color.w);
    float4 specular = (0).xxxx;
    float3 tangent = lerp(pin.Tangent1WS.xyz, pin.Tangent2WS.xyz, pin.Color.w);

    return ComputeCommon(pin, diffuse, specular, bump, tangent);
}

float4 PSDualSpecular(VSOutput pin) : SV_Target0
{
    float4 texDiffuse0 = g_Diffuse0.Sample(g_Diffuse0Sampler, pin.UV.xy);
    float4 texNormal0 = g_Normal0.Sample(g_Normal0Sampler, pin.UV.xy);
    float4 texSpecular0 = g_Specular0.Sample(g_Specular0Sampler, pin.UV.xy);

    float4 texDiffuse1 = g_Diffuse1.Sample(g_Diffuse1Sampler, pin.UV.zw);
    float4 texNormal1 = g_Normal1.Sample(g_Normal1Sampler, pin.UV.zw);
    float4 texSpecular1 = g_Specular1.Sample(g_Specular1Sampler, pin.UV.zw);

    if (IsDummy(texDiffuse1))
    {
        texDiffuse1 = texDiffuse0;
    }
    if (IsDummy(texNormal1))
    {
        texNormal1 = texNormal0;
    }
    if (IsDummy(texSpecular1))
    {
        texSpecular1 = texSpecular0;
    }

    float4 diffuse = lerp(texDiffuse0, texDiffuse1, pin.Color.w);
    float3 bump = (texNormal0.xyz - 0.5) * 2.0;// lerp((texNormal0.xyz - 0.5) * 2.0, (texNormal1.xyz - 0.5) * 2.0, pin.Color.w);
    float3 specular = lerp(texSpecular0.rrr, texSpecular1.rrr, pin.Color.w);
    float3 tangent = lerp(pin.Tangent1WS.xyz, pin.Tangent2WS.xyz, pin.Color.w);

    return ComputeCommon(pin, diffuse, float4(specular, 1), bump, pin.Tangent1WS.xyz);
}

technique11 Single
{
    pass P0 {
        SetGeometryShader(0);
        SetVertexShader(CompileShader(vs_4_0, VSUvScroll()));
        SetPixelShader(CompileShader(ps_4_0, PSSingle()));
    }
}
technique11 SingleSpecular
{
    pass P0 {
        SetGeometryShader(0);
        SetVertexShader(CompileShader(vs_4_0, VSUvScroll()));
        SetPixelShader(CompileShader(ps_4_0, PSSingleSpecular()));
    }
}
technique11 Dual
{
    pass P0 {
        SetGeometryShader(0);
        SetVertexShader(CompileShader(vs_4_0, VSUvScroll()));
        SetPixelShader(CompileShader(ps_4_0, PSDual()));
    }
}
technique11 DualSpecular
{
    pass P0 {
        SetGeometryShader(0);
        SetVertexShader(CompileShader(vs_4_0, VSUvScroll()));
        SetPixelShader(CompileShader(ps_4_0, PSDualSpecular()));
    }
}