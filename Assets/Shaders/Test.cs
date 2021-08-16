using UnityEngine;

public class Test : MonoBehaviour
{
    public TerrainData terrainData;
    public TextureData textureData;
    public Material terrainMaterial;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            textureData.ApplyToMaterial(terrainMaterial);
        }
    }
}
