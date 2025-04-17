ProjectB
===========
3D 농장 경영 시뮬레이션 게임 포트폴리오
-----------------------------

https://youtu.be/cdJTnnvfRNw

_<작동 영상>_

## 1.Rendering

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

_<Assets/Scripts/PixelizePass.cs>_

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

_<Assets/Scripts/PixelizeFeature.cs>_

Unity의 ScriptableRendererFeature 기능을 통해 Pixelize.shader를 Postprocessing으로 적용합니다

    Shader"Hidden/Pixelize"
    {
        ...
            Pass
            {
                Name "Pixelation"
    
                HLSLPROGRAM
    
                #define E 2.71828182846
    
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
                    return tex;
                }
                ENDHLSL
            }
        }
    }

_<Assets/Shader/Pixelize/Pixelize.shader>_

Pixelize 쉐이더는 화면의 종횡비를 통해 전체 화면을 설정한 크기의 블럭으로 나누고, 블럭안의 각 픽셀을 Gaussian/Box 블러링하여 도트 그래픽을 구현합니다.

![Pixelizeb_a](https://github.com/user-attachments/assets/597aadc0-d09e-45ad-9b04-7b5737fd647a)

## 2.에셋 구조 제작

ScriptableObject를 통해 농작물, 레시피등 오브젝트들을 모듈화하였습니다.

### 2.1 GrindAsset & GrindManager

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
    
_<Assets/Scripts/LevelObject/FarmingObject/GrindAsset.cs>_

GrindAsset은 각 곡물의 화면에 표시될 Mesh, Material과 곡물을 수확했을 때 들어올 ItemType, 곡물이 자라는데 걸리는 시간을 저장하고 있는 ScriptableObject입니다.

    namespace ProjectB
    {
        [CreateAssetMenu(fileName = "NewGrindManager", menuName = "ProjectB/GrindManagerAsset")]
        public class GrindManagerAsset : ScriptableObject
        {
            [SerializeField] public List<GrindAsset> GrindAssetList;
        }
    }
    
_<Assets/Scripts/LevelObject/FarmingObject/GrindManager.cs>_

GrindManager는 게임에 사용되는 모든 곡물의 GrindAsset 객체를 담고있는 ScriptableObject입니다.

### 2-2.Receipe & ReceipeTree

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
    
_<Assets/Scripts/System/Cooking/Receipe.cs>_

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

_<Assets/Scripts/System/ReceipeTree.cs>_

ReceipeTree는 각 레시피들의 계층구조를 나타냅니다. 하위 레시피를 해금하고 싶으면, 그에 맞는 상위 레시피가 활성화되어 있어야합니다. ReceipeTree는 ReceipeTreeNode객체를 요소로 가지며, 게임 시작시 기본제공될 Receipe인 basicReceipes와 모든 ReceipeTreeNode들을 직렬화 하기위한 _node_Serialize_set 리스트를 가집니다. 기본적으로 ReceipeTree의 탐색은 BFS 알고리즘을 통하며, 각 탐색마다 호출될 수 있는 elementFindCallback을 파라미터로 제공합니다.

또한 ReceipeTree는 용이한 수정을 위한 GraphView를 제공합니다.

![image](https://github.com/user-attachments/assets/ab534cf0-2c47-46f0-8c62-3beed4a13d1b)

_<Assets/Scripts/Editor/Graph/ReceipeTreeGraph>_

이를 통해 ReceipeTree 객체를 편하고 직관적으로 디자인할 수 있습니다.

![image](https://github.com/user-attachments/assets/59d74371-d494-42d5-9058-04408ec18c86)

_<Assets/Scripts/Editor/Graph/ReceipeTreeGraph/ReceipeTreeGraphWindow.cs>_

## 3.Logic 설계

시뮬레이션 장르 특성상 복잡한 로직을 표현해야 하는 경우가 많으므로, 최대한 코드간의 독립성을 유지하며, 체계적이게 프로젝트를 관리하려고 하였습니다.

### 3.1 DayCycleSystem

게임의 한 주기는 1일이며, 이를 DayCycleSystem Singletone 객체가 관리합니다. 

    namespace ProjectB
    {
        public enum DayTime
        {
            MORNING,
            AFTERNOON,
            EVENING,
            NIGHT
        }
    
        public class DayTimeEvent
        {
            public HashSet<DayTime> encounterDayTime;
            public float encounterProbability;
            public int maxEncounterOnDay;
            public Action action;
    
            public int encounterNum = 0;
            public bool enabled = true;
    
            public DayTimeEvent(HashSet<DayTime> encounterDayTime, float encounterProbability, int maxEncounterOnDay, Action action)
            {
                ...
            }
        }
        public class DayCycleSystem : MonoBehaviour
        {
            public static DayCycleSystem instance;
            public DayTime dayTime = DayTime.MORNING;
            
            ...
    
            float dayCycleSec = 0;
    
            //"하루" 동안 발생가능한 이벤트들을 저장합니다.
            List<DayTimeEvent> dayTimeEventManager = new List<DayTimeEvent>();
    
            void Start()
            {
                instance = this;
                InitializeDayTime();
            }
    
            // Update is called once per frame
            void Update()
            {
                ...
            }
    
            public void InitializeDayTime()
            {
                dayTime = DayTime.MORNING;
                dayCycleSec = oneDaySec;
                foreach(DayTimeEvent e in dayTimeEventManager)
                {
                    e.encounterNum = 0;
                }
            }
            public void RegisterDayTimeEvent(DayTimeEvent dayTimeEvent)
            {
                dayTimeEventManager.Add(dayTimeEvent);
            }
        }
    }
    
_<Assets/Scripts/System/DayCycleSystem.cs>_

DayCycleSystem은 가장 먼저 활성화되어 다른 System으로부터 DayTimeEvent를 등록받습니다. DayTimeEvent는 하루동안 일어나는 이벤트의 일어날 시기, 확률, 최대 발생 횟수, 작동할 Delegate를 담고있는 객체입니다.

### 3.2 InteractiveObject

    namespace ProjectB
    {
        [System.Serializable]
        public class InteractionParams
        {
            [SerializeField] public bool bUseProgression;
            [SerializeField] public float progressionTime;
            [SerializeField] public float waitTime;
        }
        public abstract class InteractableObject : MonoBehaviour
        {
            public InteractionParams interactionParams;
            public bool bNeedItem = false;
            public HashSet<ItemType> InteractItems = new HashSet<ItemType>();
    
            [SerializeField] List<ItemType> InteractItems_inspector;
    
            ...
    
            public void OnInteract(PlayerController playerController)
            {
                if (InteractItems.Contains(playerController.GetCurrentItem().GetType()) || !bNeedItem)
                {
                    MainUI.instance.ShowInteractionProgression(interactionParams.bUseProgression,
                                                                interactionParams.progressionTime,
                                                                interactionParams.waitTime,
                                                                () => { InteractCallback(playerController); },
                                                                () => { InteractPreprocess(playerController); });
                }
            }
            
            ...
            
            public abstract void InteractCallback(PlayerController playerController);
            public abstract void InteractPreprocess(PlayerController playerController);
        }
    }

_<Assets/Scripts/LevelObject/InteractableObject.cs>_

InteractableObject는 Player가 상호작용할 수 있는 오브젝트들의 최상위 클래스입니다. InteractionParams는 상호작용시 Progressbar를 사용할지 여부를 담습니다. 또한 상호작용이 끝난후 호출될 InteractCallBack과 상호작용 키를 눌렀을 때 즉시 호출되는 InteractPreprocess 추상메서드를 가집니다.

![image](https://github.com/user-attachments/assets/8ad9729e-2c67-4ece-8969-e8d338cc7f47)

_<Interaction Progress Bar>_

### 3.3 Inventory

    namespace ProjectB
    {
        public enum ItemType
        {
            NONE,
            STEAK,
            WATER_BOTTLE,
            HOE,
            WHEAT_SEED,
            WHEAT,
            CORN_SEED,
            CORN
        }
        public static class ItemTypeExtensions
        {
            ...
        }
    
        [System.Serializable]
        public class ItemStack
        {
            [SerializeField] ItemType type;
            [SerializeField] int Amount;
    
            public ItemStack(ItemType type, int amount)
            {
                ...
            }
            ...
        }
    }

_<Assets/Scripts/System/Inventory/ItemStack.cs>_

ItemStack 클래스는 게임 내에서 생성된 아이템의 개수와 종류를 담는 객체입니다. ItemType은 아이템의 종류를 표현하는 enum클래스로, Extension을 가집니다.

    namespace ProjectB
    {
        public class Inventory : List<ItemStack>
        {
            public Action onInventoryUpdate;
            int size;
    
            public Inventory(int size) : base()
            {
                this.size = size;
            }
    
            ...
        }
    }
    
_<Assets/Scripts/System/Inventory/Inventory.cs>_

Inventory 클래스는 ItemStack을 담는 List를 상속받습니다. Inventory 클래스에는 인벤토리가 업데이트 되었을때 호출될 함수를 등록할 수 있습니다.
