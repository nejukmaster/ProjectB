ProjectB
3D 농장 경영 시뮬레이션 게임 포트폴리오

[작동영상]


1.Rendering

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

<Assets/Shader/Pixelize/Pixelize.shader>

Outline은 Sobel필터를 통한 가장자리 검출 알고리즘을 SceneDepth Texture에 사용하여 구현했으며, Scene의 도트 느낌을 최대한 주기 위하여 Orthonormal 투영 카메라를 사용하므로 보다 깔끔한 Outline을 위해 SceneNormal Texture는 Outline에서 사용하지 않았습니다. Pixelize는 전체 화면의 종횡비를 받아 설정한 크기의 Block으로 구획화하고, 각 블럭의 픽셀들의 색을 Gaussian/Box 블러링하는 방식으로 구현했습니다.
![Pixelizeb_a](https://github.com/user-attachments/assets/597aadc0-d09e-45ad-9b04-7b5737fd647a)

2.에셋 구조 제작

ScriptableObject를 통해 농작물, 레시피, 손님 오브젝트들을 모듈화하였습니다.

2.1 GrindAsset & GrindManager

    namespace ProjectB
    {
        [CreateAssetMenu(fileName = "NewGrindAsset", menuName = "ProjectB/GrindAsset")]
    #if UNITY_EDITOR
        [CanEditMultipleObjects]
    #endif
        public class GrindAsset : ScriptableObject
        {
            public int GrindLevelsNum
            {
                get
                {
                    return meshes.Length;
                }
            }
            [SerializeField] Mesh[] meshes;
            [SerializeField] Material material;
            [SerializeField] ItemType crop;
            [SerializeField] float GrowSec;
        ...
        }
    }
    
<Assets/Scripts/LevelObject/FarmingObject/GrindAsset.cs>

GrindAsset은 각 곡물의 화면에 표시될 Mesh, Material과 곡물을 수확했을 때 들어올 ItemType, 곡물이 자라는데 걸리는 시간을 저장하고 있는 ScriptableObject입니다.

    namespace ProjectB
    {
        [CreateAssetMenu(fileName = "NewGrindManager", menuName = "ProjectB/GrindManagerAsset")]
        public class GrindManagerAsset : ScriptableObject
        {
            [SerializeField] public List<GrindAsset> GrindAssetList;
        }
    }
    
<Assets/Scripts/LevelObject/FarmingObject/GrindManager.cs>

GrindManager는 게임에 사용되는 모든 곡물의 GrindAsset 객체를 담고있는 ScriptableObject입니다.

2-2.Receipe & ReceipeTree

    namespace ProjectB
    {
        [System.Serializable]
        [CreateAssetMenu(fileName = "NewReceipe", menuName = "ProjectB/Receipe Asset")]
        public class Receipe : ScriptableObject
        {
            [SerializeField] public List<ItemStack> Ingredients;
            [SerializeField] public ItemStack Result;
            [SerializeField] public float CookingSec;
            [SerializeField] public CookwareType type;
    
            [NonSerialized] public bool bIsEnabled = false;
        }
    }
    
<Assets/Scripts/System/Cooking/Receipe.cs>

Receipe는 레스토랑 경영 시스템에서 음식을 만들기 위해 필요하며, 재료, 결과물, 요리시간, 필요한 조리도구, 활성화 여부를 담고있는 ScriptableObject입니다.

    namespace ProjectB
    {
        [Serializable]
        public class ReceipeTreeNode
        {
            [SerializeField] public Receipe data;
            [SerializeField] public List<ReceipeTreeNode> children;
            ...
        }
    
        [CreateAssetMenu(fileName = "New ReceipeTree", menuName = "ProjectB/Receipe Tree Asset")]
        public class ReceipeTree : ScriptableObject
        {
            [SerializeField] public List<ReceipeTreeNode> basicReceipes;
            [SerializeField] List<ReceipeTreeNode> _node_serialize_set = new List<ReceipeTreeNode>();
    
            public void BFS(Action<ReceipeTreeNode, ReceipeTreeNode> elementFindCallback)
            {
                ...
            }
            ...
        }
    }

<Assets/Scripts/System/ReceipeTree.cs>

ReceipeTree는 각 레시피들의 계층구조를 나타냅니다. 하위 레시피를 해금하고 싶으면, 그에 맞는 상위 레시피가 활성화되어 있어야합니다. ReceipeTree는 ReceipeTreeNode객체를 요소로 가지며, 게임 시작시 기본제공될 Receipe인 basicReceipes와 모든 ReceipeTreeNode들을 직렬화 하기위한 _node_Serialize_set 리스트를 가집니다. 기본적으로 ReceipeTree의 탐색은 BFS 알고리즘을 통하며, 각 탐색마다 호출될 수 있는 elementFindCallback을 파라미터로 제공합니다.

또한 ReceipeTree는 용이한 수정을 위한 GraphView를 제공합니다.
![image](https://github.com/user-attachments/assets/ab534cf0-2c47-46f0-8c62-3beed4a13d1b)
<Assets/Scripts/Editor/Graph/ReceipeTreeGraph>

이를 통해 ReceipeTree 객체를 편하고 직관적으로 디자인할 수 있습니다.
![image](https://github.com/user-attachments/assets/59d74371-d494-42d5-9058-04408ec18c86)
<Assets/Scripts/Editor/Graph/ReceipeTreeGraph/ReceipeTreeGraphWindow.cs>


    

3.Logic 설계
