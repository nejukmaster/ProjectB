ProjectB
===========
3D Farm Management Simulation Game Portfolio
-----------------------------

https://youtu.be/cdJTnnvfRNw

_<Gameplay>_

## 1.Rendering

I have converted the 3D visuals into pixel graphics.

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

The Pixelize.shader is applied as a post-processing effect using Unity's ScriptableRendererFeature functionality.

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

The Pixelize shader divides the full screen into blocks of a specified size based on the screen’s aspect ratio. Each pixel within a block is then blurred using either a Gaussian or Box blur to create a pixel art-style visual effect.

![Pixelizeb_a](https://github.com/user-attachments/assets/597aadc0-d09e-45ad-9b04-7b5737fd647a)

## 2.Asset Structure Design

Objects such as crops and recipes have been modularized using ScriptableObjects.

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

The GrindAsset is a ScriptableObject that stores information for each type of grain, including the Mesh and Material used for its visual representation, the ItemType obtained upon harvesting, and the time required for the grain to grow.

    namespace ProjectB
    {
        [CreateAssetMenu(fileName = "NewGrindManager", menuName = "ProjectB/GrindManagerAsset")]
        public class GrindManagerAsset : ScriptableObject
        {
            [SerializeField] public List<GrindAsset> GrindAssetList;
        }
    }
    
_<Assets/Scripts/LevelObject/FarmingObject/GrindManager.cs>_

The GrindManager is a ScriptableObject that contains all the GrindAsset objects used in the game.

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

The Recipe is a ScriptableObject used in the restaurant management system to define dishes. It contains the required ingredients, resulting product, cooking time, necessary kitchen tools, and whether the recipe is currently active.

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

The RecipeTree represents the hierarchical structure of all recipes. To unlock a high-tier recipe, its corresponding parent recipe must be activated. The RecipeTree contains a RecipeTreeNode objects as its elements. It also includes the basicRecipes, which are the default recipes provided at the start of the game, and the _node_Serialize_set list used to serialize all RecipeTreeNode instances.
By default, the RecipeTree is searched using the BFS algorithm. During searching, an elementFindCallback can be provided to define actions to be executed at each node.

The RecipeTree also provides a GraphView interface to allow for easy editing and visualization of the recipe hierarchy.

![image](https://github.com/user-attachments/assets/ab534cf0-2c47-46f0-8c62-3beed4a13d1b)

_<Assets/Scripts/Editor/Graph/ReceipeTreeGraph>_

This allows the RecipeTree object to be designed in a convenient and intuitive manner.

![image](https://github.com/user-attachments/assets/59d74371-d494-42d5-9058-04408ec18c86)

_<Assets/Scripts/Editor/Graph/ReceipeTreeGraph/ReceipeTreeGraphWindow.cs>_

## 3.Logic 설계

Due to the nature of simulation games, complex logic often needs to be implemented. Therefore, I aimed to manage the project systematically while maintaining independence between code modules.

### 3.1 DayCycleSystem

One cycle of the game represents a single day, and this is managed by the DayCycleSystem singleton object.

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

The DayCycleSystem is activated first and receives DayTimeEvents registered from other systems. A DayTimeEvent is an object that contains the scheduled time of occurrence, probability, maximum number of occurrences per day, and the delegate to be executed when triggered.

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

InteractableObject is the base class for all objects that the player can interact with. It uses an InteractionParams object to determine whether a progress bar should be displayed during interaction. Additionally, it has two abstract methods: InteractPreprocess, which is called immediately when the interaction key is pressed, and InteractCallback, which is invoked after the interaction is completed.

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

The ItemStack class represents an object that holds both the quantity and type of items generated within the game. The ItemType is an enum class that defines the various types of items and includes extensions for additional functionality.

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

The Inventory class inherits from a List of ItemStack objects. It allows functions to be registered that will be called whenever the inventory is updated.
