// Combines two images into one warped Side-by-Side image

sampler2D TexMap0;
sampler2D TexMap1;



float2 Warp(float2 Tex : TEXCOORD0)
{
    float a = 0.20f;
	float b = 0.00f;
	float c = 0.00f;
	float d = 1 - (a + b + c);
	float2 fragPos = Tex * 2.0f - float2(1.f,1.f);
	float destR = length(fragPos);
	float srcR = a * pow(destR,4) + b * pow(destR,3) + c * pow(destR,2) + d * destR;
	float2 correctedRadial = normalize(fragPos) * srcR;
	float2 uv = (correctedRadial/2.0f) + float2(0.5f, 0.5f);
    return uv  ;
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
