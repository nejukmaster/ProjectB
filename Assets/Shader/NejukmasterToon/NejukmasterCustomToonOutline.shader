Shader"Nejukmaster/NejukmasterCustomToonOutline"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _OutlineWidth("Outline Width", Range(0,50)) = 1
        _OutlineColor("Outline Color", Color) = (0,0,0,1)
        _OutlineCutoff("Outline Cutoff", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "RenderPipeline" = "UniversalPipeline"}
        LOD 300
        Cull Front

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vertexOutline
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            
            CBUFFER_START(UnityPerMaterial)
                float _OutlineWidth;
                half4 _OutlineColor;
                float _OutlineCutoff;
            CBUFFER_END

            struct VertexInput
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct VertexOutput
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS :VAR_POSITION;
                float2 uv : TEXCOORD0;
                float3 viewDIr : TEXCOORD1;
                float3 normalWS : NORMAL;
            };

            VertexOutput vertexOutline(VertexInput v)
            {
                VertexOutput o = (VertexOutput)0;
                o.positionWS = TransformObjectToWorld(v.vertex.xyz);
                o.positionCS = TransformObjectToHClip(v.vertex.xyz);
    
                float3 clipNormal = TransformObjectToHClip(v.normal);
                float2 offset = normalize(clipNormal.xy) / _ScreenParams.xy * _OutlineWidth * o.positionCS.w;
                o.viewDIr = _WorldSpaceCameraPos.xyz - TransformObjectToWorld(v.vertex.xyz);
                o.normalWS = TransformObjectToWorldNormal(v.normal);

                float ndv = dot(normalize(o.normalWS), normalize(o.viewDIr));
                o.positionCS.xy += offset*(1.0-ndv);

                o.uv = v.uv;

                return o;
            }
            half4 frag (VertexOutput i) : SV_Target
            {
                return _OutlineColor;
            }
            ENDHLSL
        }
    }
}
