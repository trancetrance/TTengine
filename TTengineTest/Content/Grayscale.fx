// (c) 2010-2017 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt

sampler TextureSampler : register(s0);

float4 PixelShaderFunction(float4 position : SV_Position, float4 color : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float4 tex = tex2D(TextureSampler, coords);
    tex.rgb = dot(tex.rgb, float3(0.3, 0.59, 0.11));
    return tex;
}


technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_4_0_level_9_1 PixelShaderFunction();
    }
}