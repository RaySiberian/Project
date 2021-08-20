using System;
using UnityEngine;

public class TerrainSpawn : MonoBehaviour
{
    public NoiseData noiseData;
    public TerrainData terrainData;

    public Material material;

    [Range(0, 6)] public int levelOfDetail;

    private int mapChunkSize = 241;
    private GameObject meshGameObject;
    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;
    private MeshCollider meshCollider;
    private float[,] falloffMap;
    public static event Action TerrainSpawned;
    
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
            MeshGenerator.GenerateTerrainMesh(mapData.heightMap, terrainData.meshHeightMultiplier,
                terrainData.meshHeightCurve, levelOfDetail));
        meshCollider = meshGameObject.AddComponent<MeshCollider>();
        meshGameObject.transform.position = new Vector3(600, 0, 600);
        meshGameObject.transform.localScale = new Vector3(5, 1, 5);
        TerrainSpawned?.Invoke();
    }

    private void DrawMesh(MeshData meshData)
    {
        meshRenderer.material = material;
        meshFilter.sharedMesh = meshData.CreateMesh();
    }

    public MapData GenerateMapData()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, noiseData.seed, noiseData.noiseScale,
            noiseData.octaves, noiseData.persistance,
            noiseData.lacunarity, noiseData.offset);

        for (int y = 0; y < mapChunkSize; y++)
        {
            for (int x = 0; x < mapChunkSize; x++)
            {
                if (terrainData.useFalloff)
                {
                    noiseMap[x, y] = Mathf.Clamp01(noiseMap[x, y] - falloffMap[x, y]);
                }
            }
        }

        return new MapData(noiseMap);
    }
}