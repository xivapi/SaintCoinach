
#define LightResult ColorPair

LightResult ComputeLights(float3 eyeVector, float3 worldNormal, uniform int numLights)
{
    float3x3 lightDirections = 0;
    float3x3 lightDiffuse = 0;
    float3x3 lightSpecular = 0;
    float3x3 halfVectors = 0;
    [unroll]
    for (int i = 0; i < numLights; i++)
    {
        lightDirections[i] = float3x3(m_Light0.Direction, m_Light1.Direction, m_Light2.Direction) [i];
        lightDiffuse[i] =    float3x3(m_Light0.Diffuse,   m_Light1.Diffuse,   m_Light2.Diffuse)   [i];
        lightSpecular[i] =   float3x3(m_Light0.Specular,  m_Light1.Specular,  m_Light2.Specular)  [i];
        halfVectors[i] = normalize(eyeVector - lightDirections[i]);
    }
    
    float3 dotL = mul(-lightDirections, worldNormal);
    float3 dotH = mul(halfVectors, worldNormal);
    float3 zeroL = step(0, dotL);
    
    float3 diffuse = zeroL * dotL;
    float3 specular = pow(max(dotH, 0) * zeroL, m_SpecularPower);

    LightResult result;
    result.Diffuse = float4(mul(diffuse, lightDiffuse) * m_DiffuseColor.rgb + m_EmissiveColor, 1);
    result.Specular = float4(mul(specular, lightSpecular) * m_SpecularColor, 1);
    return result;
}