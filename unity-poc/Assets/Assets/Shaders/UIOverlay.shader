// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "UI/Overlay"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255

        _ColorMask ("Color Mask", Float) = 15

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
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

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        ColorMask [_ColorMask]

//        Pass
//        {
//            Blend One One
//            Name "Default"
//        CGPROGRAM
//            #pragma vertex vert
//            #pragma fragment frag
//            #pragma target 2.0
//
//            #include "UnityCG.cginc"
//            #include "UnityUI.cginc"
//
//            #pragma multi_compile __ UNITY_UI_CLIP_RECT
//            #pragma multi_compile __ UNITY_UI_ALPHACLIP
//
//            struct appdata_t
//            {
//                float4 vertex   : POSITION;
//                float4 color    : COLOR;
//                float2 texcoord : TEXCOORD0;
//                UNITY_VERTEX_INPUT_INSTANCE_ID
//            };
//
//            struct v2f
//            {
//                float4 vertex   : SV_POSITION;
//                fixed4 color    : COLOR;
//                float2 texcoord  : TEXCOORD0;
//                float4 worldPosition : TEXCOORD1;
//                UNITY_VERTEX_OUTPUT_STEREO
//            };
//
//            fixed4 _Color;
//            fixed4 _TextureSampleAdd;
//            float4 _ClipRect;
//
//            v2f vert(appdata_t v)
//            {
//                v2f OUT;
//                UNITY_SETUP_INSTANCE_ID(v);
//                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
//                OUT.worldPosition = v.vertex;
//                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);
//
//                OUT.texcoord = v.texcoord;
//
//                OUT.color = v.color * _Color;
//                return OUT;
//            }
//
//            sampler2D _MainTex;
//
//            fixed4 frag(v2f IN) : SV_Target
//            {
//                half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;
//                half blend = color.a;
//                
//                color = max(half4(0.5,0.5,0.5,1), color);
//                color -= 0.5;
//                color *= 2;                
//                color *= blend;
//                
//                color.a = 1;
//
//                #ifdef UNITY_UI_CLIP_RECT
//                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
//                #endif
//
//                #ifdef UNITY_UI_ALPHACLIP
//                clip (color.a - 0.001);
//                #endif
//
//                return color;
//            }
//        ENDCG
//        }
        
        Pass
        {
            Blend DstColor SrcColor 
            Name "Default"
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            #pragma multi_compile __ UNITY_UI_CLIP_RECT
            #pragma multi_compile __ UNITY_UI_ALPHACLIP

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;

            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

                OUT.texcoord = v.texcoord;

                OUT.color = v.color * _Color;
                return OUT;
            }

            sampler2D _MainTex;

            fixed4 frag(v2f IN) : SV_Target
            {
                half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;
                half blend = color.a;
//                
//                color = min(0.5, color);
//                color *= 2;
                
//                color = 1 - color;
//                color *= blend;
//                color = 1 - color;
                
                color.a = 1;
                

                #ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif

                #ifdef UNITY_UI_ALPHACLIP
                clip (color.a - 0.001);
                #endif

                return color;
            }
        ENDCG
        }
    }
}
