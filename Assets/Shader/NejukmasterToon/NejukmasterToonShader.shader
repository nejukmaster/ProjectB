Shader"Nejukmaster/CustomToon"
{
    Properties
    {
        // Specular vs Metallic workflow
        [HideInInspector] _WorkflowMode("WorkflowMode", Float) = 1.0

        [MainTexture] _BaseMap("Albedo", 2D) = "white" {}
        [MainColor] _BaseColor("Color", Color) = (1,1,1,1)

        _Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5

        //StylizedPBR
        //[Header(StylizedDiffuse)]
        [Space(10)]
        _MedColor("Med Tone Color", Color) = (1,1,1,1)
        _MedThreshold ("Med Tone Threshold", Range(0,1)) = 1
        _MedSmooth ("Med Tone Smooth", Range(0,0.5)) = 0.25
        
        [Space(10)]
        _ShadowColor ("Shadow Color", Color) = (0,0,0,1)
        _ShadowThreshold ("Shadow Threshold", Range(0,1)) = 0.5
        _ShadowSmooth ("Shadow Smooth", Range(0,0.5)) = 0.25
        
        [Space(10)]
        _ReflectColor ("Reflect Color", Color) = (0,0,0,0)
        _ReflectThreshold ("Reflect Threshold", Range(0,1)) = 0
        _ReflectSmooth ("Reflect Smooth", Range(0,0.5)) = 0.25
        
        [Space(10)]
        _GIIntensity("GI Intensity", Range(0,10)) = 1
        _GIThreshold("GI Threshold", Range(0,2)) = 1
        _GISmooth("GI Smooth", Range(0,1)) = 0.5
        
        //[Header(StylizedReflection)]
        [Toggle] _GGXSpecular ("GGX Specular", float) = 0
        _SpecularLightOffset("Specular Light Offset", Vector) = (0,0,0,0)
        _SpecularThreshold("Specular Threshold", Range(0.1,2)) = 0.5
        _SpecularSmooth ("Specular Smooth", Range(0,0.5)) = 0.5
        _SpecularIntensity("Specular Intensity", float) = 1
        
        [Space(10)]
        [Toggle] _DirectionalFresnel ("Directional Fresnel", float) = 0
        _FresnelThreshold("Fresnel Threshold", Range(0,1)) = 0.5
        _FresnelSmooth("Fresnel Smooth", Range(0,0.5) ) = 0.5
        _FresnelIntensity("Fresnel Intensity", float) = 1

        _ReflProbeIntensity("Non Metal Reflection Probe Intensity", float) = 1
        _MetalReflProbeIntensity ("Metal Reflection Probe Intensity", float) = 1

        [Space(10)]
        [ToggleOff] _UseOutline("Use Outline", Float) = 1.0
        _OutlineColor("OutlineColor", Color) = (0,0,0,1)
        _OutlineThreshold("Outline Threshold", Range(0,1)) = 1.0
        _OutlineSmooth("Outline Smooth", Range(0,1)) = 0.5
        
        [Space(30)]
        _Smoothness("Smoothness", Range(0.0, 1.0)) = 0.5
        _GlossMapScale("Smoothness Scale", Range(0.0, 1.0)) = 1.0
        _SmoothnessTextureChannel("Smoothness texture channel", Float) = 0

        _Metallic("Metallic", Range(0.0, 1.0)) = 0.0
        _MetallicGlossMap("Metallic", 2D) = "white" {}

        _SpecColor("Specular", Color) = (0.2, 0.2, 0.2)
        _SpecGlossMap("Specular", 2D) = "white" {}

        [ToggleOff] _SpecularHighlights("Specular Highlights", Float) = 1.0
        [ToggleOff] _EnvironmentReflections("Environment Reflections", Float) = 1.0

        _BumpScale("Scale", Float) = 1.0
        _BumpMap("Normal Map", 2D) = "bump" {}

        _OcclusionStrength("Strength", Range(0.0, 1.0)) = 1.0
        _OcclusionMap("Occlusion", 2D) = "white" {}

        _EmissionColor("Color", Color) = (0,0,0)
        _EmissionMap("Emission", 2D) = "white" {}

        _ShadowMask("Shadow Mask", 2D) = "white"{}

        // Blending state
        [HideInInspector] _Surface("__surface", Float) = 0.0
        [HideInInspector] _Blend("__blend", Float) = 0.0
        [HideInInspector] _AlphaClip("__clip", Float) = 0.0
        [HideInInspector] _SrcBlend("__src", Float) = 1.0
        [HideInInspector] _DstBlend("__dst", Float) = 0.0
        [HideInInspector] _ZWrite("__zw", Float) = 1.0
        [HideInInspector] _Cull("__cull", Float) = 2.0

        _ReceiveShadows("Receive Shadows", Range(0,1)) = 1.0     //얘도 살려둠 
        // Editmode props
        [HideInInspector] _QueueOffset("Queue offset", Float) = 0.0

        // ObsoleteProperties
        [HideInInspector] _MainTex("BaseMap", 2D) = "white" {}
        [HideInInspector] _Color("Base Color", Color) = (1, 1, 1, 1)
        [HideInInspector] _GlossMapScale("Smoothness", Float) = 0.0
        [HideInInspector] _Glossiness("Smoothness", Float) = 0.0
        [HideInInspector] _GlossyReflections("EnvironmentReflections", Float) = 0.0
    }

    SubShader
    {
        Tags{"RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "UniversalMaterialType" = "Lit" "IgnoreProjector" = "True" "ShaderModel"="4.5"}
        LOD 300
        Pass
        {
            Name "ForwardLit"
            Tags{"LightMode" = "UniversalForward"}

            Blend[_SrcBlend][_DstBlend]
            ZWrite[_ZWrite]
            Cull[_Cull]

            HLSLPROGRAM
            #pragma exclude_renderers gles gles3 glcore
            #pragma target 4.5

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature_local _NORMALMAP
            #pragma shader_feature_local _PARALLAXMAP
            #pragma shader_feature_local _RECEIVE_SHADOWS_OFF
            #pragma shader_feature_local _ _DETAIL_MULX2 _DETAIL_SCALED
            #pragma shader_feature_local_fragment _SURFACE_TYPE_TRANSPARENT
            #pragma shader_feature_local_fragment _ALPHATEST_ON
            #pragma shader_feature_local_fragment _ALPHAPREMULTIPLY_ON
            #pragma shader_feature_local_fragment _EMISSION
            #pragma shader_feature_local_fragment _METALLICSPECGLOSSMAP
            #pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
            #pragma shader_feature_local_fragment _OCCLUSIONMAP
            #pragma shader_feature_local_fragment _SPECULARHIGHLIGHTS_OFF
            #pragma shader_feature_local_fragment _ENVIRONMENTREFLECTIONS_OFF
            #pragma shader_feature_local_fragment _SPECULAR_SETUP
            #pragma shader_feature _USEOUTLINE_OFF

            // -------------------------------------
            // Universal Pipeline keywords
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile_fragment _ _REFLECTION_PROBE_BLENDING
            #pragma multi_compile_fragment _ _REFLECTION_PROBE_BOX_PROJECTION
            #pragma multi_compile_fragment _ _SHADOWS_SOFT
            #pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
            #pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
            #pragma multi_compile_fragment _ _LIGHT_LAYERS
            #pragma multi_compile_fragment _ _LIGHT_COOKIES
            #pragma multi_compile _ _CLUSTERED_RENDERING

            // -------------------------------------
            // Unity defined keywords
            #pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
            #pragma multi_compile _ SHADOWS_SHADOWMASK
            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile _ DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma multi_compile_fragment _ DEBUG_DISPLAY

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing
            #pragma instancing_options renderinglayer
            #pragma multi_compile _ DOTS_INSTANCING_ON

            #pragma vertex LitPassVertex
            #pragma fragment LitPassFragment

            #include "NejukmasterToonInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            #ifndef UNIVERSAL_FORWARD_LIT_PASS_INCLUDED
            #define UNIVERSAL_FORWARD_LIT_PASS_INCLUDED

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float3 normalOS     : NORMAL;
                float4 tangentOS    : TANGENT;
                float2 texcoord     : TEXCOORD0;
                float2 lightmapUV   : TEXCOORD1;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float2 uv                       : TEXCOORD0;
                DECLARE_LIGHTMAP_OR_SH(lightmapUV, vertexSH, 1);

            #if defined(REQUIRES_WORLD_SPACE_POS_INTERPOLATOR)
                float3 positionWS               : TEXCOORD2;
            #endif

                float3 normalWS                 : TEXCOORD3;
            #ifdef _NORMALMAP
                float4 tangentWS                : TEXCOORD4;    // xyz: tangent, w: sign
            #endif
                float3 viewDirWS                : TEXCOORD5;

                half4 fogFactorAndVertexLight   : TEXCOORD6; // x: fogFactor, yzw: vertex light

            #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
                float4 shadowCoord              : TEXCOORD7;
            #endif

                float4 positionCS               : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };
            
            //인풋 BRDF데이터 (Lit쉐이더 커스터마이징시 이거 건들지 말기)
            void InitializeInputData(Varyings input, half3 normalTS, out InputData inputData)
            {
                inputData = (InputData)0;

            #if defined(REQUIRES_WORLD_SPACE_POS_INTERPOLATOR)
                inputData.positionWS = input.positionWS;
            #endif

                half3 viewDirWS = SafeNormalize(input.viewDirWS);
            #if defined(_NORMALMAP) || defined(_DETAIL)
                float sgn = input.tangentWS.w;      // should be either +1 or -1
                float3 bitangent = sgn * cross(input.normalWS.xyz, input.tangentWS.xyz);
                half3x3 tangentToWorld = half3x3(input.tangentWS.xyz, bitangent.xyz, input.normalWS.xyz);

                #if defined(_NORMALMAP)
                inputData.tangentToWorld = tangentToWorld;
                #endif
                inputData.normalWS = TransformTangentToWorld(normalTS, tangentToWorld);
            #else
                inputData.normalWS = input.normalWS;
            #endif

                inputData.normalWS = NormalizeNormalPerPixel(inputData.normalWS);
                inputData.viewDirectionWS = viewDirWS;

            #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
                inputData.shadowCoord = input.shadowCoord;
            #elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
                inputData.shadowCoord = TransformWorldToShadowCoord(inputData.positionWS);
            #else
                inputData.shadowCoord = float4(0, 0, 0, 0);
            #endif

                inputData.fogCoord = input.fogFactorAndVertexLight.x;
                inputData.vertexLighting = input.fogFactorAndVertexLight.yzw;
                inputData.bakedGI = SAMPLE_GI(input.lightmapUV, input.vertexSH, inputData.normalWS);
                inputData.normalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(input.positionCS);
                inputData.shadowMask = SAMPLE_SHADOWMASK(input.lightmapUV);
            }
            //버텍스 쉐이더 (이것 역시 Lit쉐이더 커스터마이징 시 건들지 말기)
            Varyings LitPassVertex(Attributes input)
            {
                Varyings output = (Varyings)0;

                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                
                // normalWS and tangentWS already normalize.
                // this is required to avoid skewing the direction during interpolation
                // also required for per-vertex lighting and SH evaluation
                VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);
                float3 viewDirWS = GetCameraPositionWS() - vertexInput.positionWS;
                half3 vertexLight = VertexLighting(vertexInput.positionWS, normalInput.normalWS);
                half fogFactor = ComputeFogFactor(vertexInput.positionCS.z);

                output.uv = TRANSFORM_TEX(input.texcoord, _BaseMap);

                // already normalized from normal transform to WS.
                output.normalWS = normalInput.normalWS;
                output.viewDirWS = viewDirWS;

                OUTPUT_LIGHTMAP_UV(input.lightmapUV, unity_LightmapST, output.lightmapUV);
                OUTPUT_SH(output.normalWS.xyz, output.vertexSH);

                output.fogFactorAndVertexLight = half4(fogFactor, vertexLight);

            #if defined(REQUIRES_WORLD_SPACE_POS_INTERPOLATOR)
                output.positionWS = vertexInput.positionWS;
            #endif

            #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
                output.shadowCoord = GetShadowCoord(vertexInput);
            #endif

                output.positionCS = vertexInput.positionCS;

                return output;
            }

            half LinearStep(half minValue, half maxValue, half In){
                //연산이 많아 무거운 smoothstep대신 일반적인 선형보간을 포화시킨 LinearStep이라는 함수를 만듬
                return saturate((In-minValue) / (maxValue - minValue));
            }

            half grayscale(half3 rgb)
            {
                return 0.299 * rgb.r + 0.587 * rgb.b + 0.114 * rgb.g;
            }

            /*half4 UniversalFragmentStylizedPBR(InputData inputData, half3 albedo, half metallic, half3 specular, half smoothness, half occlusion, half3 emission, half alpha, half2 uv)
            {
                BRDFData brdfData;
                InitializeBRDFData(albedo, metallic, specular, smoothness, alpha, brdfData);
    
                Light mainLight = GetMainLight(inputData.shadowCoord);
    
                MixRealtimeAndBakedGI(mainLight,inputData.normalWS,inputData.bakedGI,half4(0,0,0,0));
    
                float ndotl = LinearStep( _ShadowThreshold - _ShadowSmooth, _ShadowThreshold + _ShadowSmooth, dot(mainLight.direction, inputData.normalWS) * 0.5 + 0.5 );

                half3 color = StylizedGlobalIllumination(brdfData, radiance, inputData.bakedGI, occlusion, inputData.normalWS, inputData.viewDirectionWS, metallic, lerp(1,ndotl, _DirectionalFresnel)  );
                color += LightingStylizedPhysicallyBased(brdfData, radiance, mainLight, inputData.normalWS, inputData.viewDirectionWS);

            #ifdef _ADDITIONAL_LIGHTS
                uint pixelLightCount = GetAdditionalLightsCount();
                for (uint lightIndex = 0u; lightIndex < pixelLightCount; ++lightIndex)
                {
                    Light light = GetAdditionalLight(lightIndex, inputData.positionWS);
                    color += LightingPhysicallyBased(brdfData, light, inputData.normalWS, inputData.viewDirectionWS);
                }
            #endif

            #ifdef _ADDITIONAL_LIGHTS_VERTEX
                color += inputData.vertexLighting * brdfData.diffuse;
            #endif

                color += emission;
                return half4(color, alpha);
            }*/

            
            half3 Calculateradiance(Light light, half3 normalWS)
            {
                half NdotL = dot(normalWS, light.direction);
                half halflambert = NdotL * 0.5 + 0.5;
                half smoothMedTone = LinearStep(_MedThreshold - _MedSmooth, _MedThreshold + _MedSmooth, halflambert);
                half3 MedToneColor = lerp(_MedColor.rgb, 1, smoothMedTone);
                half smoothShadow = LinearStep(_ShadowThreshold - _ShadowSmooth, _ShadowThreshold + _ShadowSmooth, halflambert);
                half3 ShadowColor = lerp(_ShadowColor.rgb, MedToneColor, smoothShadow * lerp(1,light.distanceAttenuation * light.shadowAttenuation, _ReceiveShadows));
                half smoothReflect = LinearStep(_ReflectThreshold - _ReflectSmooth, _ReflectThreshold + _ReflectSmooth, halflambert);
                half3 reflectColor = lerp(_ReflectColor.rgb, ShadowColor, smoothReflect);
                half3 radiance = light.color * reflectColor;
                return radiance;
            }

            half3 Calculateradiance(Light light, half3 normalWS, half mask)
            {
                half NdotL = saturate(dot(normalWS, light.direction) + (1-mask));
                half halflambert = NdotL * 0.5 + 0.5;
                half smoothMedTone = LinearStep(_MedThreshold - _MedSmooth, _MedThreshold + _MedSmooth, halflambert);
                half3 MedToneColor = lerp(_MedColor.rgb, 1, smoothMedTone);
                half smoothShadow = LinearStep(_ShadowThreshold - _ShadowSmooth, _ShadowThreshold + _ShadowSmooth, halflambert);
                half3 ShadowColor = lerp(_ShadowColor.rgb, MedToneColor, smoothShadow * lerp(1,light.distanceAttenuation * light.shadowAttenuation, _ReceiveShadows));
                half smoothReflect = LinearStep(_ReflectThreshold - _ReflectSmooth, _ReflectThreshold + _ReflectSmooth, halflambert);
                half3 reflectColor = lerp(_ReflectColor.rgb, ShadowColor, smoothReflect);
                half3 radiance = light.color * reflectColor;
                return radiance;
            }

            half3 DirectToonBRDF(BRDFData brdfData, half3 normalWS, half3 lightDirectionWS, half3 viewDirectionWS)
            {
                #ifndef SPECULARHIGHLIGHTS_OFF
                    float3 halfDir = SafeNormalize(float3(lightDirectionWS) + float3(viewDirectionWS));

                    float NoH = saturate(dot(normalWS, halfDir));
                    half LoH = saturate(dot(lightDirectionWS, halfDir));
    
                    float d = NoH * NoH * brdfData.roughness2MinusOne + 1.00001f;

                    half LoH2 = LoH * LoH;
                    half specularTerm = brdfData.roughness2 / ((d * d) * max(0.1h, LoH2) * brdfData.normalizationTerm);
                #if defined (SHADER_API_MOBILE) || defined (SHADER_API_SWITCH)
                    specularTerm = specularTerm - HALF_MIN;
                    specularTerm = clamp(specularTerm, 0.0, 100.0); // Prevent FP16 overflow on mobiles
                #endif
                    //스펙큘러 계산
                    half3 color = LinearStep(_SpecularThreshold - _SpecularSmooth, _SpecularThreshold + _SpecularSmooth, specularTerm) * brdfData.specular * max(0, _SpecularIntensity) + brdfData.diffuse;
                    return color;
                #else
                    return brdfData.diffuse;
                #endif
            }

            half3 EnvironmentBRDFCustom(BRDFData brdfData, half3 indirectDiffuse, half3 indirectSpecular, half fresnelTerm)
            {
                //환경을 디퓨즈에 적용
                half3 c = indirectDiffuse * brdfData.diffuse;
                float surfaceReduction = 1.0/(brdfData.roughness2 + 1.0);
                c += surfaceReduction * indirectSpecular * lerp(brdfData.specular, brdfData.grazingTerm, fresnelTerm);
                return c;
            }

            half3 ToonGlobalIllumination(BRDFData brdfData, half3 bakedGI, half occlusion, half3 normalWS, half3 viewDirectionWS, half metallic, half ndotl)
            {
                half3 reflectVector = reflect(-viewDirectionWS, normalWS);
                half fresnelTerm = LinearStep( _FresnelThreshold - _FresnelSmooth, _FresnelThreshold += _FresnelSmooth, 1.0 - saturate(dot(normalWS, viewDirectionWS))) * max(0,_FresnelIntensity) * ndotl;

                half3 indirectDiffuse = bakedGI * occlusion * _GIIntensity;
                half indirectTerm = grayscale(indirectDiffuse);
                indirectTerm = LinearStep(_GIThreshold - _GISmooth, _GIThreshold + _GISmooth, indirectTerm);
                indirectDiffuse = lerp(0, indirectDiffuse,indirectTerm);
                half3 indirectSpecular = GlossyEnvironmentReflection(reflectVector, brdfData.perceptualRoughness, occlusion) * lerp(max(0,_ReflProbeIntensity), max(0,_MetalReflProbeIntensity), metallic) ;

                return EnvironmentBRDFCustom(brdfData, indirectDiffuse, indirectSpecular, fresnelTerm);
            }

            half3 Outline(half3 color, InputData inputData)
            {
  #ifndef _USEOUTLINE_OFF
                half3 viewDir = inputData.viewDirectionWS;
                half3 normal = inputData.normalWS;
                float ndv = dot(normalize(viewDir),normalize(normal));
                half outline = LinearStep(_OutlineThreshold - _OutlineSmooth, _OutlineThreshold + _OutlineSmooth, ndv);
                half3 outlinedColor = lerp(_OutlineColor.rgb,color,outline);
                return outlinedColor;
  #else
                return color;
  #endif
            }

            half4 NejukmasterToonPBR(InputData inputData, half3 albedo, half metallic, half3 specular, half smoothness, half occlusion, half3 emission, half alpha, half2 uv)
            {
                BRDFData brdfData;
                InitializeBRDFData(albedo, metallic, specular, smoothness, alpha, brdfData);
    
                half shadowMask = SAMPLE_TEXTURE2D(_ShadowMask, sampler_ShadowMask, uv);
    
                Light mainLight = GetMainLight(inputData.shadowCoord);
    
                half3 radiance = Calculateradiance(mainLight,inputData.normalWS, shadowMask);
    
                MixRealtimeAndBakedGI(mainLight,inputData.normalWS,inputData.bakedGI,half4(0,0,0,0));
                
                float ndotl = smoothstep( _ShadowThreshold - _ShadowSmooth, _ShadowThreshold + _ShadowSmooth, dot(mainLight.direction, inputData.normalWS) * 0.5 + 0.5 );
    
                half3 color = ToonGlobalIllumination(brdfData, inputData.bakedGI, occlusion, inputData.normalWS, inputData.viewDirectionWS, metallic, ndotl);
    
                color += DirectToonBRDF(brdfData, inputData.normalWS, mainLight.direction, inputData.viewDirectionWS);
                
                color *= radiance;
    
                color = Outline(color, inputData);
    
                return half4(color,1);
            }

            half4 LitPassFragment(Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
    
                SurfaceData surfaceData;
                InitializeStandardLitSurfaceData(input.uv, surfaceData);

                InputData inputData;
                InitializeInputData(input, surfaceData.normalTS, inputData);
    
                Light mainLight = GetMainLight(inputData.shadowCoord);
    
                half4 color = NejukmasterToonPBR(inputData, surfaceData.albedo, surfaceData.metallic, surfaceData.specular, surfaceData.smoothness, surfaceData.occlusion, surfaceData.emission, surfaceData.alpha, input.uv);
    
                color.rgb = MixFog(color.rgb, inputData.fogCoord);
                return color;
            }
            #endif
            ENDHLSL
        }

        Pass
        {
            Name "ShadowCaster"
            Tags{"LightMode" = "ShadowCaster"}

            ZWrite On
            ZTest LEqual
            ColorMask 0
            Cull[_Cull]

            HLSLPROGRAM
            #pragma exclude_renderers gles gles3 glcore
            #pragma target 4.5

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature_local_fragment _ALPHATEST_ON
            #pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing
            #pragma multi_compile _ DOTS_INSTANCING_ON

            // -------------------------------------
            // Universal Pipeline keywords

            // This is used during shadow map generation to differentiate between directional and punctual light shadows, as they use different formulas to apply Normal Bias
            #pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW

            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment

            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/ShadowCasterPass.hlsl"
            ENDHLSL
        }

        Pass
        {
            Name "DepthOnly"
            Tags{"LightMode" = "DepthOnly"}

            ZWrite On
            ColorMask 0
            Cull[_Cull]

            HLSLPROGRAM
            #pragma exclude_renderers gles gles3 glcore
            #pragma target 4.5

            #pragma vertex DepthOnlyVertex
            #pragma fragment DepthOnlyFragment

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature_local_fragment _ALPHATEST_ON
            #pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing
            #pragma multi_compile _ DOTS_INSTANCING_ON

            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/DepthOnlyPass.hlsl"
            ENDHLSL
        }

        // This pass is used when drawing to a _CameraNormalsTexture texture
        Pass
        {
            Name "DepthNormals"
            Tags{"LightMode" = "DepthNormals"}

            ZWrite On
            Cull[_Cull]

            HLSLPROGRAM
            #pragma exclude_renderers gles gles3 glcore
            #pragma target 4.5

            #pragma vertex DepthNormalsVertex
            #pragma fragment DepthNormalsFragment

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature_local _NORMALMAP
            #pragma shader_feature_local _PARALLAXMAP
            #pragma shader_feature_local _ _DETAIL_MULX2 _DETAIL_SCALED
            #pragma shader_feature_local_fragment _ALPHATEST_ON
            #pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing
            #pragma multi_compile _ DOTS_INSTANCING_ON

            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitDepthNormalsPass.hlsl"
            ENDHLSL
        }

        // This pass it not used during regular rendering, only for lightmap baking.
        Pass
        {
            Name "Meta"
            Tags{"LightMode" = "Meta"}

            Cull Off

            HLSLPROGRAM
            #pragma exclude_renderers gles gles3 glcore
            #pragma target 4.5

            #pragma vertex UniversalVertexMeta
            #pragma fragment UniversalFragmentMetaLit

            #pragma shader_feature EDITOR_VISUALIZATION
            #pragma shader_feature_local_fragment _SPECULAR_SETUP
            #pragma shader_feature_local_fragment _EMISSION
            #pragma shader_feature_local_fragment _METALLICSPECGLOSSMAP
            #pragma shader_feature_local_fragment _ALPHATEST_ON
            #pragma shader_feature_local_fragment _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
            #pragma shader_feature_local _ _DETAIL_MULX2 _DETAIL_SCALED

            #pragma shader_feature_local_fragment _SPECGLOSSMAP

            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitMetaPass.hlsl"

            ENDHLSL
        }
    }

    FallBack "Hidden/Universal Render Pipeline/FallbackError"
    CustomEditor "UnityEditor.Rendering.Universal.ShaderGUI.NejukmasterToonShader"
}
