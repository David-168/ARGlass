Shader "Custom/Texture Simple"
{
    Properties
    {
        _MainTex ("_MainTex", 2D) = "white" {}
        _SecondTex ("_SecondTex", 2D) = "black" {}
    }
    SubShader
    {
        Tags
        {
            "Queue" = "Background"
            "RenderType" = "Background"
            "ForceNoShadowCasting" = "True"
        }

        Pass
        {
            Cull Off
            ZTest Always
            ZWrite On
            Lighting Off
            LOD 100
            Tags
            {
                "LightMode" = "Always"
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            struct fragment_output
            {
                float4 color : SV_Target;
            //    float depth : SV_Depth;
            };

            sampler2D _MainTex;
            sampler2D _SecondTex;
            float4x4 _UnityDisplayTransform;
            float4x4 _ViewProjectionTransform;
            float4x4 _UVObjectToClipTransform;

            v2f vert (appdata v)
            {
                v2f o;
                //117 o.vertex = UnityObjectToClipPos(v.vertex);
                o.vertex = mul(_ViewProjectionTransform,float4(v.vertex.xyz,1.0));
                float4 coord;
                //117 coord = o.vertex.xy/o.vertex.w;
                coord = mul(_UVObjectToClipTransform,v.vertex);
                //coord = UnityObjectToClipPos(v.vertex);
                coord.xy = coord.xy/coord.w;
                // Remap the texture coordinates based on the device rotation.
                coord.x = (coord.x)*0.5+0.5;
                coord.y = (coord.y)*0.5+0.5;

                o.uv = mul(float3(coord.xy, 1.0f), _UnityDisplayTransform).xy;

                return o;
            }

            uniform float _UnityCameraForwardScale;

            float ConvertDistanceToDepth(float d)
            {
                d = _UnityCameraForwardScale > 0.0 ? _UnityCameraForwardScale * d : d;

                float zBufferParamsW = 1.0 / _ProjectionParams.y;
                float zBufferParamsY = _ProjectionParams.z * zBufferParamsW;
                float zBufferParamsX = 1.0 - zBufferParamsY;
                float zBufferParamsZ = zBufferParamsX * _ProjectionParams.w;

                // Clip any distances smaller than the near clip plane, and compute the depth value from the distance.
                return (d < _ProjectionParams.y) ? 1.0f : ((1.0 / zBufferParamsZ) * ((1.0 / d) - zBufferParamsW));
            }

            float4 Sample_Image( sampler2D s,sampler2D s2 , in float2 p )
            {

                matrix <float, 3, 3> yuv2rgbMatrix = {
                    1.0f,0.0f,1.5748f,// row 1
                    1.0f,-0.1873f,-0.4681,// row 2
                    1.0f,1.8556f,0// row 3
                               };
            matrix <float, 4, 4> s_YCbCrToSRGB = {
                1.0h,  0.0000h,  1.4020h, -0.7010h,
                1.0h, -0.3441h, -0.7141h,  0.5291h,
                1.0h,  1.7720h,  0.0000h, -0.8860h,
                0.0h,  0.0000h,  0.0000h,  1.0000h
            };
                  float2 coord;
                //coord.x = (-p.x)*0.5+0.5;
                //coord.y = (-p.y)*0.5+0.5;
                coord = p.xy;
	            float3 color = tex2D( s, coord.yx );
                //color = UNITY_SAMPLE_TEX2D( s, coord.yx );
                // Sample the video textures (in YCbCr).
                float4 ycbcr = float4(tex2D(s, coord.xy).r ,
                                    tex2D(s2, coord.xy).rg,
                                    1.0h);

                // Convert from YCbCr to sRGB.
                //float4 videoColor = {mul(yuv2rgbMatrix, ycbcr.rgb),0.0f};
                float4 videoColor = mul(s_YCbCrToSRGB, ycbcr);
                videoColor.x = videoColor.x*0.8;
                videoColor.xyz = GammaToLinearSpace(videoColor);
	            //float3 color = {tex2D( s, coord.yx ).r,tex2D( s2, coord.yx ).r,tex2D( s2, coord.yx ).g};
                //float3 color2 = mul(yuv2rgbMatrix,color);
                //float4 color4 = {0.0f,0.99f,0.0f,0.0f};
                float4 color4 = {color,0.0f};
 	            return videoColor;
            }

            fragment_output frag (v2f i)
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                col = Sample_Image(_MainTex,_SecondTex,i.uv);
                fragment_output o;
                o.color = col;
                //float depth = tex2D(_SecondTex, i.uv).x;
                //o.depth = 1 - ConvertDistanceToDepth(depth);
           //     o.depth = -0.99;
                return o;
            }
            ENDCG
        }
    }
}
