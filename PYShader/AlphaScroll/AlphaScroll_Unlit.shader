Shader "PYShader/AlphaScroll_Unlit"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _AlphaMask("MaskMap (Glay)", 2D) = "white" {}

        _Speed("Scroll Speed", Range(0.0, 25.0)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _AlphaMask;
            float4 _AlphaMask_ST;
            fixed4 _Color;
            half _Speed;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _AlphaMask);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 offset = float2(0, _Time.x * _Speed);

                fixed4 col = _Color;
                col.a = tex2D(_AlphaMask, i.uv + offset).r * _Color.a;

                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
