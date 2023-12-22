using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[VolumeComponentMenu("Makra/Trippy")]
// to give a scene-specific user friendly controls on the render feature's parameters
public class TrippyVolume : VolumeComponent, IPostProcessComponent
{
    public BoolParameter enableEffect = new BoolParameter(true);
    public ClampedFloatParameter intensity = new ClampedFloatParameter(.5f, 0f, 5f);
    public bool IsActive() => enableEffect.value;
    [Header("Edge")]
    public ColorParameter edgeColor = new ColorParameter(Color.green);    
    public ClampedFloatParameter edgeWidth = new ClampedFloatParameter(1f, 0f, 2f);
    [Header("Base")]
    public ColorParameter baseColor = new ColorParameter(Color.black);
    [Header("Gradient")]
    public BoolParameter useGradient = new BoolParameter(false);
    public TextureParameter gradientTex = new TextureParameter(null);    
    public bool IsTileCompatible() => false;  
}
