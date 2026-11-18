using UnityEngine;

public class FlameFlicker : MonoBehaviour
{
    public float baseIntensity = 3f;
    public float amount = 0.35f;
    public float speed = 2.2f;

    Light targetLight;
    float seed;

    void Awake()
    {
        targetLight = GetComponent<Light>();
        seed = Random.value * 100f;
    }

    void Update()
    {
        if (targetLight == null) return;

        float noise = Mathf.PerlinNoise(seed, Time.time * speed) * 0.7f
                    + Mathf.PerlinNoise(seed + 10f, Time.time * speed * 3.1f) * 0.3f;
        targetLight.intensity = baseIntensity * (1f + (noise - 0.5f) * 2f * amount);
    }
}

public class RunePulse : MonoBehaviour
{
    public Renderer target;
    public Color baseEmission = Color.white;
    public float amount = 0.25f;
    public float speed = 1.3f;

    Material material;

    void Start()
    {
        if (target != null) material = target.material;
    }

    void Update()
    {
        if (material == null) return;
        float pulse = 1f + amount * Mathf.Sin(Time.time * speed);
        material.SetColor("_EmissionColor", baseEmission * pulse);
    }
}