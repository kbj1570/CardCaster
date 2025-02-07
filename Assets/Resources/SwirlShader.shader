Shader "Custom/SwirlShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {} // 소용돌이 이미지
        _SwirlStrength ("Swirl Strength", Range(0, 2)) = 0.5 // 회전 강도
        _Speed ("Swirl Speed", Range(0, 5)) = 1.0 // 회전 속도
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha // 투명도 설정
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _SwirlStrength;
            float _Speed;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                
                // UV 중심 좌표 변환 (-0.5 ~ 0.5)
                float2 uv = v.uv - 0.5;
                
                // 소용돌이 각도 계산 (시간에 따라 회전)
                float angle = atan2(uv.y, uv.x);
                float radius = length(uv);
                angle += _SwirlStrength * sin(_Time.y * _Speed) * radius;
                
                // 회전된 UV 계산
                uv = float2(cos(angle), sin(angle)) * radius;
                uv += 0.5; // 원래 위치로 복구
                
                o.uv = uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return tex2D(_MainTex, i.uv);
            }
            ENDCG
        }
    }
}
