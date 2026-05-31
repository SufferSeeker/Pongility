Shader "Custom/SpriteWhiteFlash"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _FlashColor ("Flash Color", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #include "UnityCG.cginc"

            struct AppData
            {
                float4 Vertex : POSITION;
                float2 UV : TEXCOORD0;
                fixed4 Color : COLOR;
            };

            struct VertexOutput
            {
                float4 Vertex : SV_POSITION;
                float2 UV : TEXCOORD0;
            };

            sampler2D _MainTex;
            fixed4 _FlashColor;

            VertexOutput Vert(AppData Input)
            {
                VertexOutput Output;
                Output.Vertex = UnityObjectToClipPos(Input.Vertex);
                Output.UV = Input.UV;

                return Output;
            }

            fixed4 Frag(VertexOutput Input) : SV_Target
            {
                fixed4 TextureColor = tex2D(_MainTex, Input.UV);

                fixed4 FinalColor;
                FinalColor.rgb = _FlashColor.rgb;
                FinalColor.a = TextureColor.a * _FlashColor.a;

                return FinalColor;
            }
            ENDCG
        }
    }

    Fallback "Sprites/Default"
}