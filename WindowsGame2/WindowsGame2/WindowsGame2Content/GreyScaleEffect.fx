sampler s0;
float4 PixelShaderFunction(float2 coords: TEXCOORD0) : COLOR0
{
	float4 color = tex2D(s0, coords); //Get the default pixel color.
	float r = color.r;
	float g = color.g;
	float b = color.b;
	float a = color.a;
	float Gray = (r * 0.3 + g * 0.59 + b * 0.11);

	color.rgb = Gray;
	return color;
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}