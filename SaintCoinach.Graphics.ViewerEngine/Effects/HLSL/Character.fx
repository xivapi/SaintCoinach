#include "Macros.fxh"
#include "Structures.fxh"

DECLARE_TEXTURE(Diffuse, 0);
DECLARE_TEXTURE(Specular, 2);
DECLARE_TEXTURE(Normal, 4);
sampler NormalPointSampler : register(s5)
{
    AddressU = Clamp;
    AddressV = Clamp;
    Filter = MIN_MAG_MIP_POINT;
    MinLOD = 0;
    MaxLOD = 0;
};
DECLARE_TEXTURE(Mask, 6);
DECLARE_TEXTURE(Table, 8);


BEGIN_CONSTANTS

// Camera
float3 EyePosition                          _vs(c12)  _ps(c1)  _cb(c0);

// Scene
PointLight Light0                           _vs(c13)  _ps(c2)   _cb(c1);
PointLight Light1                           _vs(c16)  _ps(c5)   _cb(c4);
PointLight Light2                           _vs(c19)  _ps(c8)   _cb(c7);
float3 EmissiveColor                        _vs(c22)  _ps(c11)  _cb(c10);
float3 AmbientColor                         _vs(c23)  _ps(c12)  _cb(c11);

// Instance
float3 MulColor                             _vs(c24)  _ps(c13)  _cb(c12);

MATRIX_CONSTANTS

row_major float4x4 World                    _vs(c0)             _cb(c0);
row_major float3x3 WorldInverseTranspose    _vs(c4)             _cb(c4);

row_major float4x4 WorldViewProj            _vs(c8)             _cb(c8);

END_CONSTANTS

#include "Common.fxh"
#include "Lighting.fxh"

float Bumpy = 1.0;
float SpecularPower = 16;

struct TableSamples
{
    float Y;
    float4 Diffuse;
    float4 Specular;
};
TableSamples GetTableSamples(float4 normal)
{
    int tableKey = floor(normal.a * 15);
    float tableY = (tableKey + 0.5) * 0.0625;
    
    float2 diffuseCoord = float2(0.125, tableY);
    float2 specularCoord = float2(0.375, tableY);
    
    TableSamples samples;

    samples.Y = tableY;
    samples.Diffuse = SAMPLE_TEXTURE(Table, diffuseCoord);
    samples.Specular = SAMPLE_TEXTURE(Table, specularCoord);
    
    return samples;
};

float4 ComputeDiffusePS(VSOutputCharacter pin) : SV_Target0
{
    float4 texDiffuse = SAMPLE_TEXTURE(Diffuse, pin.TexCoord);
    float4 texNormal = SAMPLE_TEXTURE(Normal, pin.TexCoord);

    float3 eyeVector = normalize(EyePosition - pin.PositionWS.xyz);
    float3 worldNormal = CalculateNormalFromMap(Bumpy, pin.WorldNormal, pin.WorldTangent, pin.WorldBinormal, texNormal.xyz);

    ColorPair lightResult = ComputePointLights(eyeVector, worldNormal, SpecularPower, 3);

    float4 color = ComputeCommonPSOutput(texDiffuse, float4((0).xxx, 1), lightResult, texNormal.b);
    color.rgb *= MulColor;
    return color;
}

float4 ComputeDiffuseSpecularPS(VSOutputCharacter pin) : SV_Target0
{
    float4 texDiffuse = SAMPLE_TEXTURE(Diffuse, pin.TexCoord);
    float4 texNormal = SAMPLE_TEXTURE(Normal, pin.TexCoord);
    float4 texSpecular = SAMPLE_TEXTURE(Specular, pin.TexCoord);

    float3 eyeVector = normalize(EyePosition - pin.PositionWS.xyz);
    float3 worldNormal = CalculateNormalFromMap(Bumpy, pin.WorldNormal, pin.WorldTangent, pin.WorldBinormal, texNormal.xyz);
    
    ColorPair lightResult = ComputePointLights(eyeVector, worldNormal, SpecularPower, 3);
    
    float4 color = ComputeCommonPSOutput(texDiffuse, texSpecular, lightResult, texNormal.b);
    color.rgb *= MulColor;
    return color;
}

float4 ComputeDiffuseSpecularTablePS(VSOutputCharacter pin) : SV_Target0
{
    float4 texDiffuse = SAMPLE_TEXTURE(Diffuse, pin.TexCoord);
    float4 texNormal = SAMPLE_TEXTURE(Normal, pin.TexCoord);
    float4 texSpecular = SAMPLE_TEXTURE(Specular, pin.TexCoord);

    float4 texNPoint = Normal.Sample(NormalPointSampler, pin.TexCoord);
    TableSamples samples = GetTableSamples(texNPoint);


    float4 diffuse = texDiffuse * samples.Diffuse;
    float4 specular = samples.Specular * texSpecular.g;

    float3 eyeVector = normalize(EyePosition - pin.PositionWS.xyz);
    float3 worldNormal = CalculateNormalFromMap(Bumpy, pin.WorldNormal, pin.WorldTangent, pin.WorldBinormal, texNormal.xyz);

    ColorPair lightResult = ComputePointLights(eyeVector, worldNormal, SpecularPower, 3);

    float4 color = ComputeCommonPSOutput(diffuse, specular, lightResult, texNormal.b);
    color.rgb *= MulColor;
    return color;
}

float4 ComputeMaskTablePS(VSOutputCharacter pin) : SV_Target0
{
    float4 texNormal = SAMPLE_TEXTURE(Normal, pin.TexCoord);
    float4 texMask = SAMPLE_TEXTURE(Mask, pin.TexCoord);

    float4 texNPoint = Normal.Sample(NormalPointSampler, pin.TexCoord);
    TableSamples samples = GetTableSamples(texNPoint);
    
    float4 diffuse = samples.Diffuse;
    diffuse.rgb *= texMask.b;
    float4 specular = samples.Specular;
    
    float3 eyeVector = normalize(EyePosition - pin.PositionWS.xyz);
    float3 worldNormal = CalculateNormalFromMap(Bumpy, pin.WorldNormal, pin.WorldTangent, pin.WorldBinormal, texNormal.xyz);
    
    ColorPair lightResult = ComputePointLights(eyeVector, worldNormal, SpecularPower, 3);
    
    float4 color = ComputeCommonPSOutput(diffuse, specular, lightResult, texNormal.b);
    color.rgb *= MulColor;
    return color;
}

technique10 CharacterDiffuse
{
    pass P0 {
        SetGeometryShader(0);
        SetVertexShader(CompileShader(vs_4_0, ComputeCharacterVSOutput()));
        SetPixelShader(CompileShader(ps_4_0, ComputeDiffusePS()));
    }
}

technique10 CharacterDiffuseSpecular
{
    pass P0 {
        SetGeometryShader(0);
        SetVertexShader(CompileShader(vs_4_0, ComputeCharacterVSOutput()));
        SetPixelShader(CompileShader(ps_4_0, ComputeDiffuseSpecularPS()));
    }
}

technique10 CharacterDiffuseSpecularTable
{
    pass P0 {
        SetGeometryShader(0);
        SetVertexShader(CompileShader(vs_4_0, ComputeCharacterVSOutput()));
        SetPixelShader(CompileShader(ps_4_0, ComputeDiffuseSpecularTablePS()));
    }
}

technique10 CharacterMaskTable
{
    pass P0 {
        SetGeometryShader(0);
        SetVertexShader(CompileShader(vs_4_0, ComputeCharacterVSOutput()));
        SetPixelShader(CompileShader(ps_4_0, ComputeMaskTablePS()));
    }
}