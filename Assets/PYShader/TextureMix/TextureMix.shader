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

        _ShadeColor ("ShadeColor", Color) = (1,1,1,1)

        _MixStrength ("MixStrength", Range(1,10)) = 1.0
        _ShadeStrength("ShadeStrength", Range(0,1)) = 1.0
        _DiscardLine ("DiscardLine", Range(0,1)) = 0.4

         [Space(20)]

        _Glossiness_R ("Smoothness_Red", Range(0,1)) = 0.5
        _Metallic_R ("Metallic_Red", Range(0,1)) = 0.0

         [Space(20)]

        _Glossiness_G ("Smoothness_Green", Range(0,1)) = 0.5
        _Metallic_G ("Metallic_Green", Range(0,1)) = 0.0

        
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

        fixed4 _ShadeColor;

        sampler2D _NormalRed;
        sampler2D _NormalGreen;

        struct Input
        {
            float2 uv_TexRed;
            float2 uv_TexGreen;
            float2 uv_TexBlue;

            float3 viewDir;
            float4 vertColor;

            float2 lightmapUV;
        };

        half _Glossiness_R;
        half _Metallic_R;
        half _Glossiness_G;
        half _Metallic_G;

        float4 vertColor;

        half _MixStrength;
        half _ShadeStrength;
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

			o.lightmapUV = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
		}

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            //頂点カラーからテクスチャの濃度を計算
            half negar = clamp((IN.vertColor.r - _DiscardLine ) * (_MixStrength * 5), 0, 1);
            half negag = clamp((IN.vertColor.g - _DiscardLine ) * (_MixStrength * 5), 0, 1);

            //頂点カラーに応じてAlbedoテクスチャを加減
            fixed4 ar = tex2D (_TexRed, IN.uv_TexRed);
            fixed4 ag = tex2D (_TexGreen, IN.uv_TexGreen);

            //o.Albedo = ar * negar + ag * negag;
            //Blue頂点に応じたShadeカラーのミックス
            fixed4 car = lerp(ag,ar,negar);
            o.Albedo = lerp(car,_ShadeColor,IN.vertColor.b * _ShadeColor.a * _ShadeStrength);

            //頂点カラーに応じてNormalテクスチャを加減
            fixed4 nr = tex2D (_NormalRed, IN.uv_TexRed);
            fixed4 ng = tex2D (_NormalGreen, IN.uv_TexGreen);

            fixed4 cnr = lerp(ng,nr,negar);
            o.Normal= UnpackNormal(cnr);

            //頂点カラーに応じてメタリックを加減
            o.Metallic = _Metallic_R * negar + _Metallic_G * negag - IN.vertColor.b * _ShadeStrength;
            o.Smoothness = _Glossiness_R * negar + _Glossiness_G * negag - IN.vertColor.b * _ShadeStrength;

            half3 lightmap = DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, IN.lightmapUV));
			o.Occlusion = lightmap;


            o.Alpha = 1;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
