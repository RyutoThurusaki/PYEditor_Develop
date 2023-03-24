Shader "PYShader/Powderfall"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        [HDR]_EmissiveColor("EmissionColor", Color) = (1,1,1,1)
        _ShadeColor("ShadeColor", Color) = (0.8,0.8,0.8,1)

        [Space(20)]
        _SnowDetail("Detail (RGB)", 2D) = "black" {}
        [Normal]
        _Noise("Noise (Normal)", 2D) = "bump" {}

        [Space(10)]
        _BaseSmoothness("BaseSmoothness", Range(0,1)) = 0.5
        _BaseMetallic("BaseMetallic", Range(0,1)) = 0.0
         [Space(10)]
        _SubSmoothness("SubSmoothness", Range(0,1)) = 0.5
        _SubMetallic("SubMetallic", Range(0,1)) = 0.0
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
            sampler2D _SnowDetail;

            struct Input
            {
                float2 uv_MainTex;
                float2 uv_SnowDetail;
            };

            half _BaseSmoothness;
            half _BaseMetallic;

            half _SubSmoothness;
            half _SubMetallic;

            fixed4 _Color;
            fixed4 _EmissiveColor;

            // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
            // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
            // #pragma instancing_options assumeuniformscaling
            UNITY_INSTANCING_BUFFER_START(Props)
                // put more per-instance properties here
            UNITY_INSTANCING_BUFFER_END(Props)

            void surf(Input IN, inout SurfaceOutputStandard o)
            {
                // Albedo comes from a texture tinted by color
                float2 scrollOffset = (1.222,1.222);
                o.Albedo = _Color;
                o.Emission = _EmissiveColor * (tex2D(_SnowDetail, IN.uv_SnowDetail) + tex2D(_SnowDetail, IN.uv_SnowDetail * scrollOffset));

                // Metallic and smoothness come from slider variables
                o.Metallic = _BaseMetallic + _SubMetallic * tex2D(_SnowDetail, IN.uv_SnowDetail);
                o.Smoothness = _BaseSmoothness + _SubSmoothness * tex2D(_SnowDetail, IN.uv_SnowDetail);

                o.Alpha = 1;
            }
            ENDCG
        }
            FallBack "Diffuse"
}
