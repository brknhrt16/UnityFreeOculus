// Combines two images into one warped Side-by-Side image

sampler2D TexMap0;
sampler2D TexMap1;

float2 Warp(float2 Tex : TEXCOORD0)
{
  float2 newPos = Tex;
  float c = -81.0f/10.0f;
  float u = Tex.x*2.0f - 1.0f;
  float v = Tex.y*2.0f - 1.0f;
  newPos.x = c*u/(pow(v, 2) + c);
  newPos.y = c*v/(pow(u, 2) + c);
  newPos.x = (newPos.x + 1.0f)*0.5f;
  newPos.y = (newPos.y + 1.0f)*0.5f;
  return newPos;
}

float4 SBSRift(float2 Tex : TEXCOORD0) : COLOR
{
	float4 tColor;
	float2 newPos = Tex;
	float2 warpPos;

	if(newPos.x < 0.5)
	{
		newPos.x = newPos.x * 2.0f;
		warpPos = Warp(newPos);
		warpPos.x = warpPos.x * 0.5f;
		tColor = tex2D(TexMap0, warpPos);
	}
	else 
	{
		newPos.x = (newPos.x - 0.5f) * 2.0f;
		warpPos = Warp(newPos);
		warpPos.x = warpPos.x * 0.5f + 0.5f;
		tColor = tex2D(TexMap0, warpPos);
	}

	if(warpPos.x < 0.0f || warpPos.x > 1.0f || warpPos.y < 0.0f || warpPos.y > 1.0f) {
		tColor[0] = 0.0f;
		tColor[1] = 0.0f;
		tColor[2] = 0.0f;
	}

	return tColor;
}

technique ViewShader
{
	pass P0
    {
		VertexShader = null;
        PixelShader  = compile ps_2_0 SBSRift();
    }
}
