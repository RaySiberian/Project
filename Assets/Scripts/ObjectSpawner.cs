using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ObjectSpawner : MonoBehaviour
{
    // Регулирование количество зон спавна
    // Число меньше - зон больше
    public int objectNoiseScale;
    public int grassNoiseScale;
    public int enemyNoiseScale;
    public int seed;
    private int raycastDistance = 100;
    
    public Text FPSText;
    public ObjectSpawnArea[] objectSpawnAreas = new ObjectSpawnArea[] { };
    public ObjectSpawnArea[] grassSpawnAreas = new ObjectSpawnArea[] { };
    public ObjectSpawnArea[] enemySpawnAreas = new ObjectSpawnArea[] { };
    
    public static event Action ObjectSpawned;
    
    private void OnEnable()
    {
        TerrainSpawn.TerrainSpawned += SpawnObjects;
    }

    private void OnDisable()
    {
        TerrainSpawn.TerrainSpawned -= SpawnObjects;
    }

    private void Start()
    {
        seed = SharedData.Seed;
    }

    private void SpawnObjects()
    {
        GameObject obsticalsParentObject = new GameObject();
        obsticalsParentObject.name = "Obsticals";
        GameObject grassParentObject = new GameObject();
        grassParentObject.name = "Grass";
        MeshCombiner meshCombiner = grassParentObject.AddComponent<MeshCombiner>();
        NavMeshObstacle navMeshObstacle = grassParentObject.AddComponent<NavMeshObstacle>();
        Spawn(objectSpawnAreas,objectNoiseScale,0.9f, new Vector3(3,3,3), obsticalsParentObject);
        Spawn(grassSpawnAreas, grassNoiseScale,0.8f, new Vector3(2,2,2), grassParentObject);
       
        meshCombiner.DestroyCombinedChildren = true;
        //meshCombiner.CombineMeshes(true);
        
        ObjectSpawned?.Invoke();
        Spawn(enemySpawnAreas,enemyNoiseScale,0.99f, new Vector3(1,1,1));
    }
    
    private void Spawn(ObjectSpawnArea[] objectSpawnArea, float noiseScale, float intensity, Vector3 localScale, GameObject parent)
    {
        for (int x = 0; x < 1200; x++)
        {
            for (int z = 0; z < 1200; z++)
            {
                float xCoord = (float)(x + seed) / 1200 * noiseScale;
                float yCoord = (float)(z + seed) / 1200 * noiseScale;
                float noise = Mathf.PerlinNoise(xCoord, yCoord);
                if (noise >= intensity)
                {
                    Ray ray = new Ray(new Vector3(x, 50, z),Vector3.down * raycastDistance);
                    
                    if (Physics.Raycast(ray, out RaycastHit hit))
                    {
                        foreach (var area in objectSpawnArea)
                        {
                            if (!hit.collider.gameObject.CompareTag("Ground"))
                            {
                                continue;
                            }
                            
                            if (hit.point.y >= area.minY && hit.point.y <= area.maxY)
                            {
                                int prefabID = Random.Range(0, area.prefabs.Length);
                                
                                GameObject go = area.prefabs[prefabID];
                                go.transform.localScale = localScale;
                                
                                Instantiate(go, new Vector3(x, hit.point.y, z),
                                    Quaternion.FromToRotation (transform.up, hit.normal) * transform.rotation, parent.transform);
                            }
                        }
                    }
                }
            }
        }
    }
    
    private void Spawn(ObjectSpawnArea[] objectSpawnArea, float noiseScale, float intensity, Vector3 localScale)
    {
        for (int x = 0; x < 1200; x++)
        {
            for (int z = 0; z < 1200; z++)
            {
                float xCoord = (float)(x + seed) / 1200 * noiseScale;
                float yCoord = (float)(z + seed) / 1200 * noiseScale;
                float noise = Mathf.PerlinNoise(xCoord, yCoord);
                if (noise >= intensity)
                {
                    Ray ray = new Ray(new Vector3(x, 50, z),Vector3.down * raycastDistance);
                    
                    if (Physics.Raycast(ray, out RaycastHit hit))
                    {
                        foreach (var area in objectSpawnArea)
                        {
                            if (!hit.collider.gameObject.CompareTag("Ground"))
                            {
                                continue;
                            }
                            
                            if (hit.point.y >= area.minY && hit.point.y <= area.maxY)
                            {
                                int prefabID = Random.Range(0, area.prefabs.Length);
                                
                                GameObject go = area.prefabs[prefabID];
                                go.transform.localScale = localScale;
                                
                                Instantiate(go, new Vector3(x, hit.point.y, z),
                                    Quaternion.FromToRotation (transform.up, hit.normal) * transform.rotation);
                            }
                        }
                    }
                }
            }
        }
    }
    
}

[System.Serializable]
public class ObjectSpawnArea
{
    //TODO сюда можно перенести настройку шума
    public string name;
    public float minY;
    public float maxY;
    public GameObject[] prefabs;
}