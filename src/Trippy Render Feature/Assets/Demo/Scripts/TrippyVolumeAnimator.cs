using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Volume))]
public class TrippyVolumeAnimator : MonoBehaviour
{
    private Volume volume;
    private TrippyVolume trippyEffect;

    public float edgeWidthFrequency = 1.0f;
    public float intensityFrequency = 1.0f;
    public float colorFrequency = 1.0f;
    public float flickerFrequency = 10f;

    private float timer = 0.0f;

    void Start()
    {
        volume = GetComponent<Volume>();
        volume.profile.TryGet(out trippyEffect);
    }

    void Update()
    {
        if (trippyEffect == null) return;

        float edgeWidthValue = (Mathf.Sin(Time.timeSinceLevelLoad * edgeWidthFrequency) + 1f) / 2f * (1.5f - 0.75f) + 0.5f;
        trippyEffect.edgeWidth.value = edgeWidthValue;

        float intensityValue = (Mathf.Cos(Time.timeSinceLevelLoad * intensityFrequency) + 1f) / 2f * (1.5f - 0.5f) + 0.5f;
        trippyEffect.intensity.value = intensityValue;

        float colorValue = (Mathf.Sin(Time.timeSinceLevelLoad * colorFrequency) + 1f) / 2f;
        Color animatedColor = Color.Lerp(Color.black, new Color(0f, 0f, 0.2f), colorValue);
        trippyEffect.baseColor.value = animatedColor;

        if (Time.time - timer > 1 / flickerFrequency)
        {
            timer = Time.time;
            trippyEffect.useGradient.value = Random.value > 0.5f;
        }
    }
}
