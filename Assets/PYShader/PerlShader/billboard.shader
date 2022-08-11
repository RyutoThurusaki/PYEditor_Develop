// Unity Y-Axis Billboard Shader by @gam0022
// https://gam0022.net/blog/2019/07/23/unity-y-axis-billboard-shader/
Shader "Unlit/Billboard"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [KeywordEnum(OFF, ALL_AXIS, Y_AXIS)] _BILLBOARD("Billboard Mode", Float) = 2
        _Cutoff ("Alpha Cutoff", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags{ "Queue" = "AlphaTest" "RenderType" = "TransparentCutout"
                "IgnoreProjector" = "True" "DisableBatching" = "True" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #pragma multi_compile _BILLBOARD_OFF _BILLBOARD_ALL_AXIS _BILLBOARD_Y_AXIS

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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            
            float _Cutoff;

            v2f vert (appdata v)
            {
                v2f o;

                #if _BILLBOARD_OFF
                {
                    // �r���{�[�h�Ȃ��̒ʏ�̍��W�ϊ�
                    o.vertex = UnityObjectToClipPos(v.vertex);
                }
                #elif _BILLBOARD_ALL_AXIS
                {                   
                    // Mesh�̌��_��ModelView�ϊ�
                    float3 viewPos = UnityObjectToViewPos(float3(0, 0, 0));
                    
                    // �X�P�[���Ɖ�]�i���s�ړ��Ȃ��j����Model�ϊ����āAView�ϊ��̓X�L�b�v
                    float3 scaleRotatePos = mul((float3x3)unity_ObjectToWorld, v.vertex);
                    
                    // scaleRotatePos���E��n�ɕϊ����āAviewPos�ɉ��Z
                    // �{����View�ϊ��ňÖٓI��Z�����]����Ă���̂ŁA
                    // View�ϊ����X�L�b�v����ꍇ�͖����I��Z�𔽓]����K�v������
                    viewPos += float3(scaleRotatePos.xy, -scaleRotatePos.z);
                    
                    o.vertex = mul(UNITY_MATRIX_P, float4(viewPos, 1));
                }
                #elif _BILLBOARD_Y_AXIS
                {
                    // Mesh�̌��_��ModelView�ϊ�
                    float3 viewPos = UnityObjectToViewPos(float3(0, 0, 0));
                    
                    // �X�P�[���Ɖ�]�i���s�ړ��Ȃ��j����Model�ϊ����āAView�ϊ��̓X�L�b�v
                    float3 scaleRotatePos = mul((float3x3)unity_ObjectToWorld, v.vertex);                
                    
                    // View�s�񂩂��]�s���Y�������݂̂𒊏o
                    float3x3 ViewRotateY = float3x3(
                        1, UNITY_MATRIX_V._m01, 0,
                        0, UNITY_MATRIX_V._m11, 0,
                        0, UNITY_MATRIX_V._m21, -1// Z�̕����𔽓]���ĉE��n�ɕϊ�
                    );
                    viewPos += mul(ViewRotateY, scaleRotatePos);
                    
                    o.vertex = mul(UNITY_MATRIX_P, float4(viewPos, 1));
                }
                #endif

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                clip(col.a - _Cutoff);
                
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}