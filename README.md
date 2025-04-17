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


2.에셋 구조 제작
ScriptableObject를 통해 농작물

3.Logic 설계
