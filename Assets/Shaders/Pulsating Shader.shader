Shader "Custom/PulsatingShader"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint Color", Color) = (1,1,1,1)
        
        // Custom controls for the pulse
        _PulseColor ("Pulse Glow Color", Color) = (1,1,1,1)
        _PulseSpeed ("Pulse Speed", Float) = 2.0
        _MinBrightness ("Min Brightness", Range(0, 1)) = 0.5
    }

    SubShader
    {
        Tags 
        { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _PulseColor;
            float _PulseSpeed;
            float _MinBrightness;

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                // Grabbing the color from the Sprite Renderer component
                OUT.color = IN.color * _Color;
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                fixed4 c = tex2D(_MainTex, IN.texcoord) * IN.color;
                
                // Oscilating between 0 and 1 using time since game started
                half pulse = (sin(_Time.y * _PulseSpeed) * 0.5 + 0.5);
                
                // Keeping pulse above minimum brightness
                pulse = pulse * (1.0 - _MinBrightness) + _MinBrightness;
                
                // Applying the pulse to the RGB channels
                c.rgb *= _PulseColor.rgb * pulse;
                
                // Preserving transparency
                c.rgb *= c.a; 
                return c;
            }
            ENDCG
        }
    }
}
