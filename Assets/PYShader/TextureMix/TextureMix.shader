Shader "Standard/TextureMix"
{
    Properties
    {
        _TexRed ("Albedo_R", 2D) = "red" {}
        _TexGreen ("Albedo_G", 2D) = "green" {}
        _TexBlue ("Albedo_B", 2D) = "blue" {}
        _MixStrength ("Strength", Range(1,1000)) = 1.0
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows Lambert vertex:vert

        #pragma target 3.5

        sampler2D _TexRed;
        sampler2D _TexGreen;
        sampler2D _TexBlue;

        struct Input
        {
            float2 uv_TexRed;
            float2 uv_TexGreen;
            float2 uv_TexBlue;

            float3 viewDir;
            float4 vertColor;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        float4 vertColor;

        half _MixStrength;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        //頂点カラーの取得
        void vert(inout appdata_full v, out Input o){
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.vertColor = v.color;
		}

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            //頂点カラーに応じてテクスチャを加減
            half rv = clamp(IN.vertColor.r * _MixStrength, 0, 0.5);
            fixed4 r = tex2D (_TexRed, IN.uv_TexRed) * rv;

            half gv = clamp(IN.vertColor.g * _MixStrength, 0, 0.5);
            fixed4 g = tex2D (_TexGreen, IN.uv_TexGreen) * gv;

            half bv = clamp(IN.vertColor.b * _MixStrength, 0, 0.5);
            fixed4 b = tex2D (_TexBlue, IN.uv_TexBlue) * bv;

            o.Albedo = r + g + b * _MixStrength;

            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = 1;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
