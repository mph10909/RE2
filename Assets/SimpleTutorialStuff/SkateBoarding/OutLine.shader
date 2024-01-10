Shader "Custom/OutlineShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	_OutlineColor("Outline Color", Color) = (0, 0, 0, 1)
		_OutlineWidth("Outline Width", Range(0, 0.1)) = 0.01
	}

		SubShader
	{
		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }

		CGINCLUDE
#include "UnityCG.cginc"

		struct appdata
	{
		float4 vertex : POSITION;
		float2 uv : TEXCOORD0;
	};

	struct v2f
	{
		float4 vertex : SV_POSITION;
		float2 uv : TEXCOORD0;
	};
	ENDCG

		Pass
	{
		Cull Front // Show the outline on the front face of the object

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag

		float _OutlineWidth;
	float4 _OutlineColor;

	v2f vert(appdata v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);

		// Push the vertices outwards to create the outline effect
		float4 outlineVertex = v.vertex + normalize(v.vertex) * _OutlineWidth;
		o.vertex = UnityObjectToClipPos(outlineVertex);

		o.uv = v.uv;

		return o;
	}

	sampler2D _MainTex;

	fixed4 frag(v2f i) : SV_Target
	{
		// Sample the color from the texture
		fixed4 texColor = tex2D(_MainTex, i.uv);

	// Calculate the blended color with outline color
	fixed4 col = lerp(texColor, _OutlineColor, _OutlineWidth);

	return col;
	}
		ENDCG
	}

		Pass
	{
		Cull Back // Show the original object without outline

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag

		v2f vert(appdata v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.uv = v.uv;

		return o;
	}

	sampler2D _MainTex;

	fixed4 frag(v2f i) : SV_Target
	{
		// Show the original texture without any modification
		fixed4 col = tex2D(_MainTex, i.uv);
	return col;
	}
		ENDCG
	}
	}
}
