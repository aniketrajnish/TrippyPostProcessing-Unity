Shader "Makra/Trippy"
{
    Properties
    {
        _MainTex("Base (RGB)", 2D) = "white" {}
        _EdgeColor("Edge Color", Color) = (0,1,0,1)        
        _EdgeWidth("Edge Width", Range(0.0,2.0)) = 2
        _BaseColor("Base Color", Color) = (0,0,0,1)
        _Intensity("Intensity", Range(0.0,5.0)) = 1
        _UseGradient("Use Gradient (Toggle)", Float) = 0 
        _GradientTex("Gradient", 2D) = "white" {}
    }

    HLSLINCLUDE // for basic shader functionality
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl" 
   
    struct Attributes
    {
        float4 vertex : POSITION;
        float2 uv : TEXCOORD0;
    };

    Texture2D _MainTex;
    SamplerState sampler_MainTex;
    Texture2D _CameraDepthTexture;
    SamplerState sampler_CameraDepthTexture; 
    float _EdgeWidth;
    float _Intensity;
    float4 _EdgeColor;
    float4 _BaseColor;
    float _UseGradient;
    Texture2D _GradientTex;
    SamplerState sampler_GradientTex;

    struct Varyings
    {
        float4 vertex : POSITION;
        float2 uv : TEXCOORD0;
    };

    Varyings Vert(Attributes input)
    {
        Varyings output = (Varyings) 0;
        output.vertex = TransformObjectToHClip(input.vertex.xyz);
        output.uv = input.uv;
        return output;
    }       
    // edge detection for outline using sobel filters
    float SobelEdgeDetection(float2 uv, float edgeWidth)
    {
        float2 texelSize = 1.0 / float2(_ScreenParams.xy); // size of one texel

        // sobel operator kernel
        float3x3 sobelX = float3x3(-1, 0, 1, -2, 0, 2, -1, 0, 1);
        float3x3 sobelY = float3x3(-1, -2, -1, 0, 0, 0, 1, 2, 1);

        float gx = 0;
        float gy = 0;
        
        // using sobel operator to find edges
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                float4 sample = _MainTex.Sample(sampler_MainTex, uv + texelSize * float2(x, y));
                float intensity = dot(sample.rgb, _Intensity * float3(1, 1, 1)); // grayscale amt based on intensity
                gx += intensity * sobelX[x + 1][y + 1];
                gy += intensity * sobelY[x + 1][y + 1];
            }
        }

        // grad magnitude for edge strength
        float edge = sqrt(gx * gx + gy * gy);

        // normalization
        edge = saturate(edge / edgeWidth);

        return edge;
    }

    float4 Frag(Varyings input) : SV_Target
    {
        float edge; 
        float4 colorChoice, finalColor;
    
        if (_UseGradient == 1) // check weather gradient or single color to be used
        { 
            edge =  SobelEdgeDetection(input.uv, 0.18 * _EdgeWidth);
            colorChoice = _GradientTex.Sample(sampler_GradientTex, float2(edge, 0.5)); 
            finalColor = lerp(_BaseColor, colorChoice, edge);
        }
        else
        {
            edge = SobelEdgeDetection(input.uv, _EdgeWidth);
            colorChoice = lerp(_BaseColor, _EdgeColor, edge);            
            finalColor = lerp(colorChoice, _BaseColor, step(_EdgeWidth, edge));
        }     

        return finalColor;
    }

    ENDHLSL

    SubShader
    {
        Tags{ "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }
        LOD 100

        Pass
        {
            Name"TrippyPostProcess"
            ZTest Always Cull Off ZWrite Off

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            ENDHLSL
        }
    }
    
}
