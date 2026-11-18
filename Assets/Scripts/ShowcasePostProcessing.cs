using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Volume))]
public class ShowcasePostProcessing : MonoBehaviour
{
    [Header("Living colour")]
    public bool animateAtRuntime = true;
    public float pulseSpeed = 0.35f;
    public float bloomPulse = 0.08f;
    public float exposurePulse = 0.04f;

    Volume volume;
    Bloom bloom;
    ColorAdjustments colour;
    float baseBloom;
    float baseExposure;

    public static ShowcasePostProcessing Create(Transform parent, Camera viewCamera, Vector3 focusPoint,
                                                bool depthOfField)
    {
        var go = new GameObject("Showcase_PostProcessing");
        go.transform.SetParent(parent, false);

        var volume = go.AddComponent<Volume>();
        volume.isGlobal = true;
        volume.priority = 10f;
        volume.profile = BuildProfile(viewCamera, focusPoint, depthOfField);

        return go.AddComponent<ShowcasePostProcessing>();
    }

    static VolumeProfile BuildProfile(Camera viewCamera, Vector3 focusPoint, bool depthOfField)
    {
        var profile = ScriptableObject.CreateInstance<VolumeProfile>();

        profile.Add<Tonemapping>(true).mode.Override(TonemappingMode.ACES);

        var bloom = profile.Add<Bloom>(true);
        bloom.threshold.Override(1.0f);
        bloom.intensity.Override(0.9f);
        bloom.scatter.Override(0.68f);
        bloom.tint.Override(new Color(0.85f, 0.92f, 1f));
        bloom.highQualityFiltering.Override(true);

        var colour = profile.Add<ColorAdjustments>(true);
        colour.postExposure.Override(0.1f);
        colour.contrast.Override(12f);
        colour.saturation.Override(7f);
        colour.colorFilter.Override(new Color(0.94f, 0.97f, 1f));

        var shadows = profile.Add<ShadowsMidtonesHighlights>(true);
        shadows.shadows.Override(new Vector4(0.82f, 1.00f, 1.10f, -0.12f));
        shadows.midtones.Override(new Vector4(0.96f, 1.00f, 1.06f, 0f));
        shadows.highlights.Override(new Vector4(1.08f, 1.00f, 0.90f, 0.12f));

        var vignette = profile.Add<Vignette>(true);
        vignette.color.Override(new Color(0.015f, 0.025f, 0.045f));
        vignette.intensity.Override(0.34f);
        vignette.smoothness.Override(0.58f);

        var grain = profile.Add<FilmGrain>(true);
        grain.type.Override(FilmGrainLookup.Thin1);
        grain.intensity.Override(0.13f);
        grain.response.Override(0.78f);

        profile.Add<ChromaticAberration>(true).intensity.Override(0.07f);

        if (depthOfField)
        {
            var dof = profile.Add<DepthOfField>(true);
            dof.mode.Override(DepthOfFieldMode.Bokeh);
            dof.focusDistance.Override(Vector3.Distance(viewCamera.transform.position, focusPoint));
            dof.focalLength.Override(70f);
            dof.aperture.Override(8f);
            dof.bladeCount.Override(7);
        }

        return profile;
    }

    void Awake()
    {
        volume = GetComponent<Volume>();
        if (volume.profile == null) return;

        volume.profile = Instantiate(volume.profile);
        volume.profile.TryGet(out bloom);
        volume.profile.TryGet(out colour);

        if (bloom != null) baseBloom = bloom.intensity.value;
        if (colour != null) baseExposure = colour.postExposure.value;
    }

    void Update()
    {
        if (!animateAtRuntime || volume == null || volume.profile == null) return;

        float wave = Mathf.Sin(Time.unscaledTime * pulseSpeed);
        if (bloom != null) bloom.intensity.value = baseBloom + wave * bloomPulse;
        if (colour != null) colour.postExposure.value = baseExposure + wave * exposurePulse;
    }

    void OnDestroy()
    {
        if (!Application.isPlaying && volume != null && volume.profile != null
            && volume.profile.hideFlags == HideFlags.DontSave)
        {
            DestroyImmediate(volume.profile);
        }
    }
}