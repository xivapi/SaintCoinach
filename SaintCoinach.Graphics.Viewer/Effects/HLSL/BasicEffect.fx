struct VS_IN
{
    float4 pos : POSITION;
    float4 col : COLOR;
};

struct PS_IN
{
    float4 pos : SV_POSITION;
    float4 col : COLOR;
};

float4x4 worldViewProj;

PS_IN VS(VS_IN input)
{
    PS_IN output = (PS_IN) 0;

    output.pos = mul(input.pos, worldViewProj);
    output.col = input.col;

    return output;
}

float4 PS(PS_IN input) : SV_Target
{
    return input.col;
}