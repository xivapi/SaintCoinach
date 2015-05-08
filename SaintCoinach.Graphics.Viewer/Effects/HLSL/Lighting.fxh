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
 

Lighting GetLight( float3 pos3D, float3 eyeVector, float3 worldNormal )
{
    float3x3 lightDirections = 0;
    float3x3 lightDiffuse = 0;
    float3x3 lightSpecular = 0;
    float3x3 halfVectors = 0;
    
    [unroll]
    for (int i = 0; i < 3; i++)
    {
        lightDirections[i] = float3x3(m_Light0.Direction,     m_Light1.Direction,     m_Light2.Direction)    [i];
        lightDiffuse[i]    = float3x3(m_Light0.Diffuse.xyz,   m_Light1.Diffuse.xyz,   m_Light2.Diffuse.xyz)  [i];
        lightSpecular[i]   = float3x3(m_Light0.Specular.xyz,  m_Light1.Specular.xyz,  m_Light2.Specular.xyz) [i];
        
        halfVectors[i] = normalize(eyeVector - lightDirections[i]);
    }

    float3 dotL = mul(-lightDirections, worldNormal);
    float3 dotH = mul(halfVectors, worldNormal);
    
    float3 zeroL = step(0, dotL);

    float3 diffuse  = zeroL * dotL;
    float3 specular = pow(max(dotH, 0) * zeroL, m_SpecularPower);

    Lighting result;
    
    result.Diffuse  = mul(diffuse,  lightDiffuse)  * m_DiffuseColor.rgb + m_EmissiveColor;
    result.Specular = mul(specular, lightSpecular) * m_SpecularColor;

    return result;
};