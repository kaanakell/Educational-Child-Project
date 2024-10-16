Shader "Unlit/LiquidProgressBar"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FillAmount ("Fill Amount", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

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
            float _FillAmount;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                // Simulate a liquid wave effect
                float wave = sin(i.uv.x * 10 + _Time.y * 2) * 0.05; // Wave effect
                if(i.uv.y < _FillAmount + wave)
                {
                    return tex2D(_MainTex, i.uv);
                }
                else
                {
                    return float4(0, 0, 0, 0);
                }
            }
            ENDCG
        }
    }
}
