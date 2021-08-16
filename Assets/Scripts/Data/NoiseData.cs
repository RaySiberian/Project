using UnityEngine;

[CreateAssetMenu()]
public class NoiseData : UpdateableData
{
    public int octaves;
    [Range(0, 1)] public float persistance;
    public float lacunarity;
    public float noiseScale;
    
    public int seed;
    public Vector2 offset;

    protected override void OnValidate()
    {
        if (lacunarity < 1)
        {
            lacunarity = 1;
        }

        if (octaves < 0)
        {
            octaves = 0;
        }
        
        base.OnValidate();
    }
}
