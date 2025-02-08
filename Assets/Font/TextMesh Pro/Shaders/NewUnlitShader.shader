Shader "Custom/MonsterDissolve"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DissolveAmount ("Dissolve Amount", Range(0, 1)) = 0
        _DissolveColor ("Dissolve Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100

        CGPROGRAM
        #pragma surface surf Lambert alpha

        sampler2D _MainTex;
        float _DissolveAmount;
        fixed4 _DissolveColor;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            
            // 그라데이션 마스크: y 위치 기반 투명도 설정
            float dissolveFactor = IN.worldPos.y - _DissolveAmount;

            if (dissolveFactor < 0)
                c.a = 0; // 아래쪽은 투명하게
            else
                c.a = c.a; // 위쪽은 원래 색

            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
}