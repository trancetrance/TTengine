
// Input: It uses texture coords as the random number seed.
// Output: Random number: [0,1), that is between 0.0 and 0.999999... inclusive.
// Author: Michael Pohoreski
// Copyright: Copyleft 2012 :-)

float random( float2 p )
{
  // We need irrationals for pseudo randomness.
  // Most (all?) known transcendental numbers will (generally) work.
  const float2 r = float2(
    23.1406926327792690,  // e^pi (Gelfond's constant)
     2.6651441426902251); // 2^sqrt(2) (Gelfond�Schneider constant)
  return frac( cos( 123456789. % 1e-7 + 256. * dot(p,r) ) );  
}

float4 PixelShaderFunction(float2 coords: TEXCOORD0) : COLOR0  
{  
    return float4( 1, 0, 0, 1);  
}  
  
technique Technique1  
{  
    pass Pass1  
    {  
        PixelShader = compile ps_4_0_level_9_3 PixelShaderFunction();
    }  
}