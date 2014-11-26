#include "Macros.fxh"
#include "Structures.fxh"

DECLARE_TEXTURE(Diffuse, 0);
DECLARE_TEXTURE(Specular, 2);
DECLARE_TEXTURE(Normal, 4);
DECLARE_TEXTURE(Mask, 6);
DECLARE_TEXTURE(Table, 8);


BEGIN_CONSTANTS

// Camera
float3 EyePosition                          _vs(c12)  _ps(c1)  _cb(c0);

// Scene
PointLight Light                            _vs(c13)  _ps(c2)   _cb(c1);
float3 EmissiveColor                        _vs(c16)  _ps(c5)   _cb(c4);
float3 AmbientColor                         _vs(c17)  _ps(c6)   _cb(c5);

// Instance
float3 MulColor                             _vs(c18)  _ps(c7)   _cb(c6);

MATRIX_CONSTANTS

row_major float4x4 World                    _vs(c0)             _cb(c0);
row_major float3x3 WorldInverseTranspose    _vs(c4)             _cb(c4);

row_major float4x4 WorldViewProj            _vs(c8)             _cb(c8);

END_CONSTANTS

#include "Common.fxh"
#include "Lighting.fxh"

float Bumpy = 1.0;
float SpecularPower = 32;

float4 ComputeCommonPSOutput(float4 diffuse, float4 specular, ColorPair lightResult)
{
    float4 color = float4(saturate(AmbientColor + lightResult.Diffuse) * diffuse.rgb, 1);
    float3 totalSpecular = lightResult.Specular * specular.rgb;
    
    AddSpecular(color, totalSpecular);
    color.rgb += EmissiveColor;
    
    return float4(color.rgb, 1);
};

struct TableSamples
{
    float4 Diffuse;
    float4 Specular;
};
TableSamples GetTableSamples(float4 normal)
{
    int tableKey = floor(normal.a * 15);
    float tableY = tableKey * 0.0625;
    
    float2 diffuseCoord = float2(0.125, tableY);
    float2 specularCoord = float2(0.375, tableY);
    
    TableSamples samples;
    
    samples.Diffuse = SAMPLE_TEXTURE(Table, diffuseCoord);
    samples.Specular = SAMPLE_TEXTURE(Table, specularCoord);
    
    return samples;
};

float4 ComputeDiffuseSpecularPS(VSOutputCharacter pin) : SV_Target0
{
    float4 texDiffuse = SAMPLE_TEXTURE(Diffuse, pin.TexCoord);
    float4 texNormal = SAMPLE_TEXTURE(Normal, pin.TexCoord);
    float4 texSpecular = SAMPLE_TEXTURE(Specular, pin.TexCoord);
    
    float3 eyeVector = normalize(EyePosition - pin.PositionWS.xyz);
    float3 worldNormal = CalculateNormalFromMap(Bumpy, pin.WorldNormal, pin.WorldTangent, pin.WorldBinormal, texNormal.rgb);
    
    ColorPair lightResult = ComputePointLight(Light, eyeVector, worldNormal, SpecularPower);
    
    return ComputeCommonPSOutput(texDiffuse, texSpecular, lightResult);
}

float4 ComputeDiffuseSpecularTablePS(VSOutputCharacter pin) : SV_Target0
{
    float4 texDiffuse = SAMPLE_TEXTURE(Diffuse, pin.TexCoord);
    float4 texNormal = SAMPLE_TEXTURE(Normal, pin.TexCoord);
    float4 texSpecular = SAMPLE_TEXTURE(Specular, pin.TexCoord);
    
    TableSamples samples = GetTableSamples(texNormal);
    
    float4 diffuse = texDiffuse * samples.Diffuse;
    float4 specular = float4(texSpecular.rgb * samples.Specular.rgb, texSpecular.a);
    
    float3 eyeVector = normalize(EyePosition - pin.PositionWS.xyz);
    float3 worldNormal = CalculateNormalFromMap(Bumpy, pin.WorldNormal, pin.WorldTangent, pin.WorldBinormal, texNormal.rgb);
    
    ColorPair lightResult = ComputePointLight(Light, eyeVector, worldNormal, SpecularPower);
    
    return ComputeCommonPSOutput(diffuse, specular, lightResult);
}

float4 ComputeDiffuseMaskTablePS(VSOutputCharacter pin) : SV_Target0
{
    float4 texDiffuse = SAMPLE_TEXTURE(Diffuse, pin.TexCoord);
    float4 texNormal = SAMPLE_TEXTURE(Normal, pin.TexCoord);
    float4 texMask = SAMPLE_TEXTURE(Mask, pin.TexCoord);
    
    TableSamples samples = GetTableSamples(texNormal);
    
    float4 diffuseStrength = float4(texMask.rrr, 1);
    
    float4 diffuse = texDiffuse * samples.Diffuse * diffuseStrength;
    float4 specular = float4(samples.Specular.rgb, 1);
    
    float3 eyeVector = normalize(EyePosition - pin.PositionWS.xyz);
    float3 worldNormal = CalculateNormalFromMap(Bumpy, pin.WorldNormal, pin.WorldTangent, pin.WorldBinormal, texNormal.rgb);
    
    ColorPair lightResult = ComputePointLight(Light, eyeVector, worldNormal, SpecularPower);
    
    return ComputeCommonPSOutput(diffuse, specular, lightResult);
}

float4 ComputeMaskTablePS(VSOutputCharacter pin) : SV_Target0
{
    float4 texNormal = SAMPLE_TEXTURE(Normal, pin.TexCoord);
    float4 texMask = SAMPLE_TEXTURE(Mask, pin.TexCoord);
    
    TableSamples samples = GetTableSamples(texNormal);
    
    float4 diffuseStrength = float4(texMask.rrr, 1);
    
    float4 diffuse = samples.Diffuse * diffuseStrength;
    float4 specular = float4(samples.Specular.rgb, 1);
    
    float3 eyeVector = normalize(EyePosition - pin.PositionWS.xyz);
    float3 worldNormal = CalculateNormalFromMap(Bumpy, pin.WorldNormal, pin.WorldTangent, pin.WorldBinormal, texNormal.rgb);
    
    ColorPair lightResult = ComputePointLight(Light, eyeVector, worldNormal, SpecularPower);
    
    return ComputeCommonPSOutput(diffuse, specular, lightResult);
}

Technique CharacterDiffuseSpecular
{
    Pass {
        EffectName = "character.shpk";
        Profile = 9.1;

        VertexShader = compile vs_2_0 ComputeCharacterVSOutput();
        PixelShader = compile ps_4_0 ComputeDiffuseSpecularPS();
    }
}

Technique CharacterDiffuseSpecularTable
{
    Pass {
        EffectName = "character.shpk";
        Profile = 9.1;

        VertexShader = compile vs_2_0 ComputeCharacterVSOutput();
        PixelShader = compile ps_4_0 ComputeDiffuseSpecularTablePS();
    }
}

Technique CharacterDiffuseMaskTable
{
    Pass {
        EffectName = "character.shpk";
        Profile = 9.1;

        VertexShader = compile vs_2_0 ComputeCharacterVSOutput();
        PixelShader = compile ps_4_0 ComputeDiffuseMaskTablePS();
    }
}

Technique CharacterMaskTable
{
    Pass {
        EffectName = "character.shpk";
        Profile = 9.1;

        VertexShader = compile vs_2_0 ComputeCharacterVSOutput();
        PixelShader = compile ps_4_0 ComputeMaskTablePS();
    }
}