ProjectB
3D 농장 경영 시뮬레이션 게임 포트폴리오

[작동영상]


1.3DtoPixelGraphic
3D화면을 픽셀 그래픽으로 바꾸는 작업을 진행

    public class PixelizePass : ScriptableRenderPass
    {
      ...
      public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
      {
          colorBuffer = renderingData.cameraData.renderer.cameraColorTargetHandle;
          RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;
    
          pixelScreenHeight = settings.screenHeight;
          pixelScreenWidth = (int)(pixelScreenHeight * renderingData.cameraData.camera.aspect + 0.5f);
    
          material.SetVector("_BlockCount", new Vector2(pixelScreenWidth, pixelScreenHeight));
          material.SetVector("_BlockSize", new Vector2(1.0f / pixelScreenWidth, 1.0f / pixelScreenHeight));
          material.SetVector("_HalfBlockSize", new Vector2(0.5f / pixelScreenWidth, 0.5f / pixelScreenHeight));
      
          descriptor.height = pixelScreenHeight;
          descriptor.width = pixelScreenWidth;
      
          cmd.GetTemporaryRT(pixelBufferID, descriptor, FilterMode.Point);
          pixelBuffer = new RenderTargetIdentifier(pixelBufferID);
      }
      ...
    }

<Assets/Scripts/PixelizePass.cs>

    public class PixelizeFeature : ScriptableRendererFeature
    {
        ...
        public override void Create()
        {
            customPass = new PixelizePass(settings);
        }
        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
    #if UNITY_EDITOR
            if (renderingData.cameraData.isSceneViewCamera) return;
    #endif
            renderer.EnqueuePass(customPass);
        }
    }

<Assets/Scripts/PixelizeFeature.cs>

Unity의 ScriptableRendererFeature 기능을 통해 Pixelize.shader를 Postprocessing으로 적용합니다

    Shader"Hidden/Pixelize"
    {
        ...
            Pass
            {
                Name "Pixelation"
    
                HLSLPROGRAM
    
                #define E 2.71828182846
    
                void Compare(inout float depthOutline, inout float normalOutline,float2 uv) {
                    //float3 neighborNormal = SampleSceneNormals(uv + _BlockSize.xy * offset);
                    //float neighborDepth = SampleSceneDepth(uv + _BlockSize.xy * offset);
    
                    float3x3 verticalOutlineConv = {1,0,-1,
                                                    2,0,-2,
                                                    1,0,-1};
                    float3x3 horizontalOutlineConv = {1,2,1,
                                                    0,0,0,
                                                    -1,-2,-1};
    
                    float depthDifferency_vert = 0;
    
                    for(int i = 0; i < 9; i ++){
                        int x = i/3;
                        int y = i%3;
    
                        depthDifferency_vert += verticalOutlineConv[x][y] * SampleSceneDepth(uv + _BlockSize.xy * float2(x-2,y-2));
                    }
    
                    depthDifferency_vert = abs(depthDifferency_vert);
    
                    float depthDifferency_horizon = 0;
    
                    for(int i = 0; i < 9; i ++){
                        int x = i/3;
                        int y = i%3;
    
                        depthDifferency_horizon += horizontalOutlineConv[x][y] * SampleSceneDepth(uv + _BlockSize.xy * float2(x-2,y-2));
                    }
    
                    depthDifferency_horizon = abs(depthDifferency_horizon);
    
                    //float3 normalDifference = baseNormal - neighborNormal;
                    //normalDifference = normalDifference.r + normalDifference.g + normalDifference.b;
                    //normalOutline = normalOutline + normalDifference;
    
                    depthOutline = depthDifferency_horizon + depthDifferency_vert / 2;
                }
    
                half4 frag(Varyings IN) : SV_TARGET
                {
                    float2 blockPos = floor(IN.uv * _BlockCount);
                    float2 blockCenter = blockPos * _BlockSize + _HalfBlockSize;
    
                    half4 tex = 0.0;
    
                    #if GAUSS
                    if(_StandardDeviation == 0)
                            tex = SAMPLE_TEXTURE2D(_MainTex, sampler_point_clamp, blockCenter);
                    else{
                            float sum = 0;
                            for (int i = 0; i < _BlockSize.x / _MainTex_TexelSize.x; i++) {
                                for (int j = 0; j < _BlockSize.y / _MainTex_TexelSize.y; j++) {
                                    float offset = length(blockPos * _BlockSize + _MainTex_TexelSize * float2(i, j) - blockCenter);
                                    float stDevSquared = _StandardDeviation * _StandardDeviation;
                                    float gauss = (1 / sqrt(2 * PI * stDevSquared)) * pow(E, -((offset * offset) / (2 * stDevSquared)));
                                    sum += gauss;
                                    tex += SAMPLE_TEXTURE2D(_MainTex, sampler_point_clamp, blockPos * _BlockSize + _MainTex_TexelSize * float2(i, j)) * gauss;
                                }
                            }
                            tex = tex/sum;
                    }
                    #else
                    int sum = 0;
                    for (int i = 0; i < _BlockSize.x / _MainTex_TexelSize.x; i++) {
                            for (int j = 0; j < _BlockSize.y / _MainTex_TexelSize.y; j++) {
                            tex = tex + SAMPLE_TEXTURE2D(_MainTex, sampler_point_clamp, blockPos * _BlockSize + _MainTex_TexelSize * float2(i, j));
                            sum++;
                            }
                    }
                    tex = tex/sum;
                    #endif
                    
                    #if OUTLINE
                        float3 normal = SampleSceneNormals(blockCenter);
                        float depth = SampleSceneDepth(blockCenter);
                        float normalDifference = 0;
                        float depthDifference = 0;
    
                        Compare(depthDifference, normalDifference, blockCenter);
    
                        //normalDifference = normalDifference * _NormalMult;
                        //normalDifference = saturate(normalDifference);
                        //normalDifference = pow(normalDifference, _NormalBias);
    
                        depthDifference = depthDifference * _DepthMult;
                        depthDifference = saturate(depthDifference);
                        depthDifference = pow(depthDifference, _DepthBias);
    
                        float outline = depthDifference;
    
                        float4 color = lerp(tex, _OutlineColor, outline);
                        return color;
                    #endif
                    return tex;
                }
                ENDHLSL
            }
        }
    }

Outline은 Sobel필터를 통한 가장자리 검출 알고리즘을 SceneDepth Texture에 사용하여 구현했으며, Scene의 도트 느낌을 최대한 주기 위하여 Orthonormal 투영 카메라를 사용하므로 보다 깔끔한 Outline을 위해 SceneNormal Texture는 Outline에서 사용하지 않았습니다. Pixelize는 전체 화면의 종횡비를 받아 설정한 크기의 Block으로 구획화하고, 각 블럭의 픽셀들의 색을 Gaussian/Box 블러링하는 방식으로 구현했습니다.
![Pixelizeb_a](https://github.com/user-attachments/assets/597aadc0-d09e-45ad-9b04-7b5737fd647a)


2.에셋 구조 제작
ScriptableObject를 통해 농작물

3.Logic 설계
