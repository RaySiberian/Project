using UnityEngine;

public class TerrainSpawn : MonoBehaviour
{
    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;
    public Material material;

    [Range(0, 6)] public int levelOfDetail;
    public float noiseScale;

    public int octaves;
    [Range(0, 1)] public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public bool useFalloff;

    public TerrainType[] regions;

    private int mapChunkSize = 241;
    private GameObject meshGameObject;
    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;
    private MeshCollider meshCollider;
    private float[,] falloffMap;

    private void Awake()
    {
        falloffMap = FalloffGenerator.GenerateFalloffMap(mapChunkSize);
    }

    private void Start()
    {
        meshGameObject = new GameObject("Terrain");
        Instantiate(meshGameObject);
        meshRenderer = meshGameObject.AddComponent<MeshRenderer>();
        meshFilter = meshGameObject.AddComponent<MeshFilter>();
        MapData mapData = GenerateMapData();
        DrawMesh(
            MeshGenerator.GenerateTerrainMesh(mapData.heightMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail));
        meshCollider = meshGameObject.AddComponent<MeshCollider>();
    }

    private void DrawMesh(MeshData meshData)
    {
        meshRenderer.material = material;
        meshFilter.sharedMesh = meshData.CreateMesh();
    }

    public MapData GenerateMapData()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance,
            lacunarity, offset);
        
        for (int y = 0; y < mapChunkSize; y++)
        {
            for (int x = 0; x < mapChunkSize; x++)
            {
                if (useFalloff)
                {
                    noiseMap[x, y] = Mathf.Clamp01(noiseMap[x, y] - falloffMap[x, y]);
                }
            }
        }

        return new MapData(noiseMap);
    }

    [System.Serializable]
    public struct TerrainType
    {
        public string name;
        public float height;
        public Color colour;
    }
}