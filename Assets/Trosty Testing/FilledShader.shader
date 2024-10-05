Shader "Trosty/SketchWithFill" {
    Properties {
        _FillColor ("Fill Color", Color) = (1, 1, 1, 1)
        _ContourColor ("Contour Color", Color) = (0, 0, 0, 0.5)
        _ContourWidth ("Contour Width", Float) = 0.01
        _ContourTex ("Contour Texture", 2D) = "black" {}
        _Amplitude ("Amplitude", Float) = 0.01
        _Speed ("Speed", Float) = 6.0
    }
    CGINCLUDE
        #pragma multi_compile _ NPR_CONTOUR_TEX
        #include "UnityCG.cginc"

        // Random hash function to create sketchy effect
        float hash (float2 seed) {
            return frac(sin(dot(seed.xy, float2(12.9898, 78.233))) * 43758.5453);
        }

        struct v2f {
            float4 pos : SV_POSITION;
            float4 scrpos : TEXCOORD0;
        };

        // Global properties
        fixed4 _ContourColor;
        fixed4 _FillColor;
        half _ContourWidth, _Speed, _Amplitude;
        sampler2D _ContourTex;
        float4 _OutlineTex_ST;
    ENDCG

    SubShader {
        Tags { "Queue" = "Overlay" "IgnoreProjector" = "True" }

        // First Pass: Solid Fill
        Pass {
            Cull Back
            Blend SrcAlpha OneMinusSrcAlpha
            Lighting Off
            CGPROGRAM
                #pragma vertex vert
                #pragma fragment fillFrag
                v2f vert (appdata_base v) {
                    v2f o;
                    // Apply slight distortion for sketchy look
                    float4 os = float4(v.normal, 0) * (_Amplitude * hash(v.texcoord.xy + floor(_Time.y * _Speed)));
                    o.pos = UnityObjectToClipPos(v.vertex - os);
                    o.scrpos = o.pos;
                    return o;
                }
                
                // Fragment shader for the fill color
                fixed4 fillFrag(v2f IN) : COLOR {
                    return _FillColor;
                }
            ENDCG
        }

        // Second Pass: Contour Lines
        Pass {
            Cull Front
            Blend SrcAlpha OneMinusSrcAlpha
            Lighting Off
            CGPROGRAM
                #pragma vertex vert
                #pragma fragment contourFrag
                v2f vert (appdata_base v) {
                    v2f o;
                    // Apply distortion for contour lines
                    float4 os = float4(v.normal, 0) * (_ContourWidth + _Amplitude * (hash(v.texcoord.xy + floor(_Time.y * _Speed)) - 0.5));
                    o.pos = UnityObjectToClipPos(v.vertex + os);
                    o.scrpos = o.pos;
                    return o;
                }
                
                // Fragment shader for contour lines
                fixed4 contourFrag(v2f IN) : COLOR {
#ifdef NPR_CONTOUR_TEX
                    float2 sp = IN.scrpos.xy / IN.scrpos.w;
                    sp.xy = (sp.xy + 1) * 0.5;
                    sp.xy *= _OutlineTex_ST.xy;
                    return tex2D(_ContourTex, sp);  // Apply contour texture
#else
                    return _ContourColor;  // Apply solid contour color
#endif
                }
            ENDCG
        }
    }

    Fallback "Diffuse"
}