Shader "Custom/DrawingShader"
{
	Properties
	{
		_MainTex("Main Texture", 2D) = "white" {}
	_DrawingTex("Drawing Texture", 2D) = "white" {}
	}

		SubShader
	{
		Tags{ "RenderType" = "Opaque" }

		CGPROGRAM
#pragma surface surf Lambert

		sampler2D _MainTex;
	sampler2D _DrawingTex;

	struct Input
	{
		float2 uv_MainTex;
		float2 uv_DrawingTex;
	};

	void surf(Input IN, inout SurfaceOutput o)
	{
		// Sample the main texture.
		o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;

		// Sample the drawing texture and blend it with the main texture.
		fixed4 drawingColor = tex2D(_DrawingTex, IN.uv_DrawingTex);
		o.Albedo = lerp(o.Albedo, drawingColor.rgb, drawingColor.a);
	}
	ENDCG
	}

		FallBack "Diffuse"
}
