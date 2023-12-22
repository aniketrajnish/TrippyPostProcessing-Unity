using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// renderer feature to add a trippy post-processing effect over the camera
public class TrippyRendererFeature : ScriptableRendererFeature
{
    // settings to hold the trippy settings :]
    [System.Serializable]
    public class TrippySettings
    {
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
        public Shader trippyShader = null;
        [HideInInspector] public Material trippyMat = null;
        [HideInInspector] public Color edgeColor = Color.green;
        [HideInInspector] public Color baseColor = Color.black;
        [HideInInspector] public float edgeWidth = 1;
        [HideInInspector] public float intensity = .5f;        
        [HideInInspector] public bool useGradient = true;
        [HideInInspector] public Texture gradTex;
    }

    public TrippySettings settings = new TrippySettings();

    // custom render pass
    class TrippyRenderPass : ScriptableRenderPass
    {
        private Material trippyMat;
        private RenderTargetHandle tempTexture;
        private ScriptableRenderer renderer;
        private TrippySettings settings =  new TrippySettings();
         
        public TrippyRenderPass(Material trippyMat, TrippySettings settings)
        {
            this.trippyMat = trippyMat; 
            this.settings = settings;
        }

        
        public void Setup(ScriptableRenderer renderer)
        {
            this.renderer = renderer;
        }

        // override execute method to apply the effects
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (trippyMat == null || renderer == null)
            {
                Debug.LogError("Trippy Material or Renderer not set.");
                return;
            }

            trippyMat.SetColor("_EdgeColor", this.settings.edgeColor);
            trippyMat.SetColor("_BaseColor", this.settings.baseColor);
            trippyMat.SetFloat("_EdgeWidth", this.settings.edgeWidth);
            trippyMat.SetFloat("_Intensity", this.settings.intensity);
            trippyMat.SetTexture("_GradientTex", this.settings.gradTex);

            if (!this.settings.useGradient)
                trippyMat.SetFloat("_UseGradient", 0);
            else
                trippyMat.SetFloat("_UseGradient", 1);

            var source = renderer.cameraColorTarget;
            CommandBuffer cmd = CommandBufferPool.Get("TrippyPostProcess");
            RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;
            cmd.GetTemporaryRT(tempTexture.id, descriptor, FilterMode.Bilinear);
            Blit(cmd, source, tempTexture.Identifier(), trippyMat, 0);
            Blit(cmd, tempTexture.Identifier(), source);
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }

    TrippyRenderPass trippyPass;

    // create amd initialize render pass and material
    public override void Create()
    {
        settings.trippyShader = Shader.Find("Makra/Trippy");

        settings.trippyMat = new Material(settings.trippyShader);

        trippyPass = new TrippyRenderPass(settings.trippyMat, settings)
        {
            renderPassEvent = settings.renderPassEvent
        };
    }

    // add custom render pass to the renderer
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (!renderingData.cameraData.postProcessEnabled) return;

        var stack = VolumeManager.instance.stack;
        var trippyVolume = stack.GetComponent<TrippyVolume>();
        if (trippyVolume == null || !trippyVolume.IsActive()) return;

        // Update settings from volume component
        settings.edgeColor = trippyVolume.edgeColor.value;
        settings.baseColor = trippyVolume.baseColor.value;
        settings.edgeWidth = trippyVolume.edgeWidth.value;
        settings.intensity = trippyVolume.intensity.value;
        settings.gradTex = trippyVolume.gradientTex.value;
        settings.useGradient = trippyVolume.useGradient.value;

        trippyPass = new TrippyRenderPass(settings.trippyMat, settings)
        {
            renderPassEvent = settings.renderPassEvent
        };

        trippyPass.Setup(renderer);
        renderer.EnqueuePass(trippyPass);
    }
}
