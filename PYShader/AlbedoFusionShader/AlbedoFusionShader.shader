Shader "Standard/AlbedoFusionShader"
{
    Properties
    {
        [Enum(Additive,0,Multiply,1)] _LayerMode("LayerMode", Float) = 0

        [Space(20)]
        _MainTex("Albedo_A_UV1 (RGB)", 2D) = "white" {}

        [Space(10)]
        _SubTex("Albedo_B_UV4 (RGB)", 2D) = "black" {}

        [Space(20)]
        _Glossiness("Smoothness", Range(0,1)) = 0.5
        _Metallic("Metallic", Range(0,1)) = 0.0

        _BlendDegree("BlendDegree", Range(0,1)) = 0.5

        [Space(20)]
        _SubTexScrollSpeed("SubTex Scroll Speed", Range(-5,5)) = 0.5
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            LOD 200

            CGPROGRAM
            // Physically based Standard lighting model, and enable shadows on all light types
            #pragma surface surf Standard fullforwardshadows

            // Use shader model 3.0 target, to get nicer looking lighting
            #pragma target 3.0

            sampler2D _MainTex;
            sampler2D _SubTex;

            struct Input
            {
                float2 uv_MainTex;
                float2 uv4_SubTex;
            };

            half _Glossiness;
            half _Metallic;
            half _BlendDegree;
            half _LayerMode;
            half _SubTexScrollSpeed;

            // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
            // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
            // #pragma instancing_options assumeuniformscaling
            UNITY_INSTANCING_BUFFER_START(Props)
                // put more per-instance properties here
            UNITY_INSTANCING_BUFFER_END(Props)

            void surf(Input IN, inout SurfaceOutputStandard o)
            {
                // Albedo comes from a texture tinted by color
                float2 scrollOffset = _Time.y * _SubTexScrollSpeed;
                fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * (tex2D(_SubTex, IN.uv4_SubTex.xy + scrollOffset) * _BlendDegree * (_LayerMode)) //乗算Multiply
                    + (tex2D(_MainTex, IN.uv_MainTex) + (tex2D(_SubTex, IN.uv4_SubTex.xy + scrollOffset) * _BlendDegree)) * (1 - _LayerMode); //加算Additive
                o.Albedo = c.rgb;

                // Metallic and smoothness come from slider variables
                o.Metallic = _Metallic;
                o.Smoothness = _Glossiness;
                o.Alpha = c.a;
            }
            ENDCG
        }
            FallBack "Diffuse"
}
