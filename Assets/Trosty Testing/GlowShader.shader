Shader "Custom/GlowShader" {
    Properties {
        _Color ("Main Color", Color) = (1, 1, 1, 1)
        _EmissionColor ("Emission Color", Color) = (1, 1, 1, 1)
        _GlowIntensity ("Glow Intensity", Float) = 1.0
        _MainTex ("Base (RGB)", 2D) = "white" {}
    }
    SubShader {
        Tags { "RenderType" = "Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        sampler2D _MainTex;
        fixed4 _Color;
        fixed4 _EmissionColor;
        half _GlowIntensity;

        struct Input {
            float2 uv_MainTex;
        };

        // Surface shader to calculate the base color and glow (emission)
        void surf (Input IN, inout SurfaceOutputStandard o) {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Emission = _EmissionColor.rgb * _GlowIntensity;  // Glow comes from emission
        }
        ENDCG
    }

    // Fall back to a standard diffuse shader if not supported
    Fallback "Diffuse"
}
