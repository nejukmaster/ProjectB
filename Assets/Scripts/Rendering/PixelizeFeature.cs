using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

//인게임 화면을 Pixelize해주는 RendererFeature입니다.
public class PixelizeFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class CustomPassSettings
    {
        //이 랜더링 패스가 언제 실행될지를 담는 RenderPassEvent Enum
        public RenderPassEvent renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        public Material mat;
        //픽셀 랜더링시 화면의 Height
        public int screenHeight = 144;
    }

    [SerializeField] private CustomPassSettings settings;
    private PixelizePass customPass;

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