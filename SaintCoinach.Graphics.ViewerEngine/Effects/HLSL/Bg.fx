#include "Macros.fxh"
#include "Structures.fxh"

DECLARE_TEXTURE(Diffuse0, 0);
DECLARE_TEXTURE(Diffuse1, 1);
DECLARE_TEXTURE(Specular0, 2);
DECLARE_TEXTURE(Specular1, 3);
DECLARE_TEXTURE(Normal0, 4);
DECLARE_TEXTURE(Normal1, 5);


BEGIN_CONSTANTS

// Camera
float3 EyePosition                          _vs(c12)  _ps(c1)  _cb(c0);

// Scene
PointLight Light0                           _vs(c13)  _ps(c2)   _cb(c1);
PointLight Light1                           _vs(c16)  _ps(c5)   _cb(c4);
PointLight Light2                           _vs(c19)  _ps(c8)   _cb(c7);
float3 EmissiveColor                        _vs(c22)  _ps(c11)  _cb(c10);
float3 AmbientColor                         _vs(c23)  _ps(c12)  _cb(c11);

MATRIX_CONSTANTS

row_major float4x4 World                    _vs(c0)             _cb(c0);
row_major float3x3 WorldInverseTranspose    _vs(c4)             _cb(c4);

row_major float4x4 WorldViewProj            _vs(c8)             _cb(c8);

END_CONSTANTS

#include "Common.fxh"
#include "Lighting.fxh"

float Bumpy = 1.0;
float SpecularPower = 16;


float4 ComputeBgSingleTexturePS(VSOutputBg pin) : SV_Target0
{
    float4 texDiffuse0 = SAMPLE_TEXTURE(Diffuse0, pin.TexCoord0);
    float4 texNormal0 = SAMPLE_TEXTURE(Normal0, pin.TexCoord0);

    float3 eyeVector = normalize(EyePosition - pin.PositionWS.xyz);
    float3 worldNormal = CalculateNormalFromMap(Bumpy, pin.WorldNormal, pin.WorldTangent, pin.WorldBinormal, texNormal0.xyz);

    ColorPair lightResult = ComputePointLights(eyeVector, worldNormal, SpecularPower, 3);

    return ComputeCommonPSOutput(texDiffuse0, float4((0).xxx, 1), lightResult, texDiffuse0.a);
}

float4 ComputeBgSingleTextureSpecularPS(VSOutputBg pin) : SV_Target0
{
    float4 texDiffuse0 = SAMPLE_TEXTURE(Diffuse0, pin.TexCoord0);
    float4 texNormal0 = SAMPLE_TEXTURE(Normal0, pin.TexCoord0);
    float4 texSpecular0 = SAMPLE_TEXTURE(Specular0, pin.TexCoord0);

    float4 specular = float4(texSpecular0.ggg, 1);

    float3 eyeVector = normalize(EyePosition - pin.PositionWS.xyz);
    float3 worldNormal = CalculateNormalFromMap(Bumpy, pin.WorldNormal, pin.WorldTangent, pin.WorldBinormal, texNormal0.xyz);

    ColorPair lightResult = ComputePointLights(eyeVector, worldNormal, SpecularPower, 3);

    return ComputeCommonPSOutput(texDiffuse0, specular, lightResult, texDiffuse0.a);
}

float4 ComputeBgDualTexturePS(VSOutputBg pin) : SV_Target0
{
    float4 texDiffuse0 = SAMPLE_TEXTURE(Diffuse0, pin.TexCoord0);
    float4 texNormal0 = SAMPLE_TEXTURE(Normal0, pin.TexCoord0);

    float4 texDiffuse1 = SAMPLE_TEXTURE(Diffuse1, pin.TexCoord1);
    float4 texNormal1 = SAMPLE_TEXTURE(Normal1, pin.TexCoord1);

    float4 s = pin.Blend;

    float4 diffuse = lerp(texDiffuse0, texDiffuse1, s.w);
    float4 normal = lerp(texNormal0, texNormal1, s.w);

    float3 eyeVector = normalize(EyePosition - pin.PositionWS.xyz);
    float3 worldNormal = CalculateNormalFromMap(Bumpy, pin.WorldNormal, pin.WorldTangent, pin.WorldBinormal, normal.xyz);

    ColorPair lightResult = ComputePointLights(eyeVector, worldNormal, SpecularPower, 3);

    return ComputeCommonPSOutput(diffuse, float4((0).xxx, 1), lightResult, diffuse.a);
}

float4 ComputeBgDualTextureSpecularPS(VSOutputBg pin) : SV_Target0
{
    float4 texDiffuse0 = SAMPLE_TEXTURE(Diffuse0, pin.TexCoord0);
    float4 texNormal0 = SAMPLE_TEXTURE(Normal0, pin.TexCoord0);
    float4 texSpecular0 = SAMPLE_TEXTURE(Specular0, pin.TexCoord0);

    float4 texDiffuse1 = SAMPLE_TEXTURE(Diffuse1, pin.TexCoord1);
    float4 texNormal1 = SAMPLE_TEXTURE(Normal1, pin.TexCoord1);
    float4 texSpecular1 = SAMPLE_TEXTURE(Specular1, pin.TexCoord1);

    float4 s = pin.Blend;

    float4 diffuse = lerp(texDiffuse0, texDiffuse1, s.w);
    float4 normal = lerp(texNormal0, texNormal1, s.w);
    float4 specular = lerp(texSpecular0, texSpecular1, s.w);

    specular = float4(specular.ggg, 1);

    float3 eyeVector = normalize(EyePosition - pin.PositionWS.xyz);
        float3 worldNormal = CalculateNormalFromMap(Bumpy, pin.WorldNormal, pin.WorldTangent, pin.WorldBinormal, normal.xyz);

        ColorPair lightResult = ComputePointLights(eyeVector, worldNormal, SpecularPower, 3);

    return ComputeCommonPSOutput(diffuse, specular, lightResult, diffuse.a);
}

float4 ThisIsJustToProtectAgainstOptimizationsWhileTesting(VSOutputBg pin) : SV_Target0
{
    float4 texDiffuse0 = SAMPLE_TEXTURE(Diffuse0, pin.TexCoord0);
    float4 texNormal0 = SAMPLE_TEXTURE(Normal0, pin.TexCoord0);
    float4 texSpecular0 = SAMPLE_TEXTURE(Specular0, pin.TexCoord0);

    float4 texDiffuse1 = SAMPLE_TEXTURE(Diffuse1, pin.TexCoord1);
    float4 texNormal1 = SAMPLE_TEXTURE(Normal1, pin.TexCoord1);
    float4 texSpecular1 = SAMPLE_TEXTURE(Specular1, pin.TexCoord1);

    float4 s = pin.Blend;

    float4 diffuse = lerp(texDiffuse0, texDiffuse1, s);
    float4 normal = lerp(texNormal0, texNormal1, s);
    float4 specular = lerp(texSpecular0, texSpecular1, s);

    float3 eyeVector = normalize(EyePosition - pin.PositionWS.xyz);
    float3 worldNormal = CalculateNormalFromMap(Bumpy, pin.WorldNormal, pin.WorldTangent, pin.WorldBinormal, normal.xyz);

    ColorPair lightResult = ComputePointLights(eyeVector, worldNormal, SpecularPower, 3);

    return ComputeCommonPSOutput(diffuse, specular, lightResult, diffuse.a);
}

technique10 BgSingleTexture
{
    pass P0 {
        SetGeometryShader(0);
        SetVertexShader(CompileShader(vs_4_0, ComputeBgVSOutput()));
        SetPixelShader(CompileShader(ps_4_0, ComputeBgSingleTexturePS()));
    }
}
technique10 BgSingleTextureSpecular
{
    pass P0 {
        SetGeometryShader(0);
        SetVertexShader(CompileShader(vs_4_0, ComputeBgVSOutput()));
        SetPixelShader(CompileShader(ps_4_0, ComputeBgSingleTextureSpecularPS()));
    }
}
technique10 BgDualTexture
{
    pass P0 {
        SetGeometryShader(0);
        SetVertexShader(CompileShader(vs_4_0, ComputeBgVSOutput()));
        SetPixelShader(CompileShader(ps_4_0, ComputeBgDualTexturePS()));
    }
}
technique10 BgDualTextureSpecular
{
    pass P0 {
        SetGeometryShader(0);
        SetVertexShader(CompileShader(vs_4_0, ComputeBgVSOutput()));
        SetPixelShader(CompileShader(ps_4_0, ComputeBgDualTextureSpecularPS()));
    }
}


technique10 ShaderWithEverything
{
    pass P0 {
        SetGeometryShader(0);
        SetVertexShader(CompileShader(vs_4_0, ComputeBgVSOutput()));
        SetPixelShader(CompileShader(ps_4_0, ThisIsJustToProtectAgainstOptimizationsWhileTesting()));
    }
}

