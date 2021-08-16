using UnityEngine;
[CreateAssetMenu]
public class TerrainData : UpdateableData
{
    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;
    
    public bool useFalloff;
    
    public float minHeight => meshHeightMultiplier * meshHeightCurve.Evaluate(0);

    public float maxHeight => meshHeightMultiplier * meshHeightCurve.Evaluate(1);
}
