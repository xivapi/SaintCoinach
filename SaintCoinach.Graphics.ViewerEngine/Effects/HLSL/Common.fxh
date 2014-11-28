float3 CalculateNormal(float3 worldNormal, float3 worldTangent, float3 worldBinorm, float3 normalSample)
{
    // Input is assumed to be normalized
    float3 bumps = 2.0 * (normalSample - 0.5);
    float3 normal = bumps.x * worldTangent + bumps.y * worldBinorm + bumps.z * worldNormal;
    
    return normalize(normal);
}

#define SET_COMMON_VS_OUT \
    vout.WorldNormal = normalize(mul(vin.Normal, g_WorldInverseTranspose).xyz); \
    vout.WorldTangent = normalize(mul(vin.Tangent, g_WorldInverseTranspose).xyz); \
    vout.WorldBinormal = normalize(mul(vin.Binormal, g_WorldInverseTranspose).xyz); \
    vout.PositionPS = mul(vin.Position, g_WorldViewProjection); \
    vout.PositionWS = mul(vin.Position, g_World)

VSOutputDualTexture VSDualTexture(VSInputDualTexture vin)
{
    VSOutputDualTexture vout;
    
    vout.TexCoord0 = vin.TexCoord0;
    vout.TexCoord1 = vin.TexCoord1;
    vout.Blend = vin.Blend;
    
    SET_COMMON_VS_OUT;
    
    return vout;
};

VSOutputCommon VSCommon(VSInputCommon vin)
{
    VSOutputCommon vout;
    
    vout.TexCoord = vin.TexCoord;
    
    SET_COMMON_VS_OUT;
    
    return vout;
};