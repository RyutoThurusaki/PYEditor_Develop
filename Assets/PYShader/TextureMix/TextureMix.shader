Shader "Standard/TextureMix"
{
    Properties
    {
        _TexRed ("Albedo_R", 2D) = "red" {}
        [NoScaleOffset][Normal]
        _NormalRed ("Normal_R", 2D) = "bump" {}

         [Space(20)]

        _TexGreen ("Albedo_G", 2D) = "green" {}
        [NoScaleOffset][Normal]
        _NormalGreen ("Normal_G", 2D) = "bump" {}

         [Space(20)]

        _TexBlue ("Albedo_B", 2D) = "blue" {}
        [NoScaleOffset][Normal]
        _NormalBlue ("Normal_B", 2D) = "bump" {}

         [Space(20)]

        _MixStrength ("Strength", Range(1,10)) = 1.0
        _DiscardLine ("DiscardLine", Range(0,1)) = 0.4

         [Space(20)]

        _Glossiness_R ("Smoothness_Red", Range(0,1)) = 0.5
        _Metallic_R ("Metallic_Red", Range(0,1)) = 0.0

         [Space(20)]

        _Glossiness_G ("Smoothness_Green", Range(0,1)) = 0.5
        _Metallic_G ("Metallic_Green", Range(0,1)) = 0.0

         [Space(20)]

        _Glossiness_B ("Smoothness_Blue", Range(0,1)) = 0.5
        _Metallic_B ("Metallic_Blue", Range(0,1)) = 0.0
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

        sampler2D _NormalRed;
        sampler2D _NormalGreen;
        sampler2D _NormalBlue;

        struct Input
        {
            float2 uv_TexRed;
            float2 uv_TexGreen;
            float2 uv_TexBlue;

            float3 viewDir;
            float4 vertColor;
        };

        half _Glossiness_R;
        half _Metallic_R;
        half _Glossiness_G;
        half _Metallic_G;
        half _Glossiness_B;
        half _Metallic_B;

        float4 vertColor;

        half _MixStrength;
        half _DiscardLine;

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

            //頂点カラーからテクスチャの濃度を計算
            half negar = clamp((IN.vertColor.r - _DiscardLine ) * (_MixStrength * 5), 0, 1);
            half negag = clamp((IN.vertColor.g - _DiscardLine ) * (_MixStrength * 5), 0, 1);
            half negab = clamp((IN.vertColor.b - _DiscardLine ) * (_MixStrength * 5), 0, 1);

            //頂点カラーに応じてAlbedoテクスチャを加減
            fixed4 ar = tex2D (_TexRed, IN.uv_TexRed);
            fixed4 ag = tex2D (_TexGreen, IN.uv_TexGreen);
            fixed4 ab = tex2D (_TexBlue, IN.uv_TexBlue);

            o.Albedo = ar * negar + ag * negag + ab * negab;

            //頂点カラーに応じてAlbedoテクスチャを加減
            fixed4 nr = tex2D (_NormalRed, IN.uv_TexRed);
            fixed4 ng = tex2D (_NormalGreen, IN.uv_TexGreen);
            fixed4 nb = tex2D (_NormalBlue, IN.uv_TexBlue);

            o.Normal = nr * negar + ng * negag + nb * negab;

            //頂点カラーに応じてメタリックを加減
            o.Metallic = _Metallic_R * negar + _Metallic_G * negag + _Metallic_B * negab;
            o.Smoothness = _Glossiness_R * negar + _Glossiness_G * negag + _Glossiness_B * negab;

            o.Alpha = 1;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
