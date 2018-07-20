sampler s0;

float ambientRed;
float ambientGreen;
float ambientBlue;

float timeOfDay;

float4x4 MatrixTransform;



float4 PixelShaderFunction(float2 coords: TEXCOORD0) : COLOR0
{

	float4 color = tex2D(s0, coords); //Get the default pixel color.
	[flatten]if (timeOfDay < 2100) {
		float r = color.r*(ambientRed / 255);
		float g = color.g*(ambientGreen / 255);
		float b = color.b*(ambientBlue / 255);
		float a = color.a;
		float Gray = (r * 0.3 + g * 0.59 + b * 0.11); //.3,.59,.11

		color.rgb = Gray;
		return color;
	}
	else {
		float r = color.r*(ambientRed / 255);
		float g = color.g*(ambientGreen / 255);
		float b = color.b*(ambientBlue / 255);
		float a = color.a;
		float Gray = (r * 0.11 + g * 0.11 + b * 0.78); //.3,.59,.11

		color.rgb = Gray;
		return color;
	}
	
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}