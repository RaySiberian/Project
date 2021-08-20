using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectSpawner : MonoBehaviour
{
    // Регулирование количество зон ставна
    // Число меньше - зон больше
    public int objectNoiseScale;
    public int grassNoiseScale;
    public int seed;
    private int raycastDistance = 100;

    public ObjectSpawnArea[] objectSpawnAreas = new ObjectSpawnArea[] { };
    public ObjectSpawnArea[] grassSpawnAreas = new ObjectSpawnArea[] { };
    private void OnEnable()
    {
        //TerrainSpawn.TerrainSpawned += SpawnObjects;
        //TerrainSpawn.TerrainSpawned += SpawnGrass;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SpawnObjects();
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            SpawnGrass();
        }
    }

    private void SpawnObjects()
    {
        for (int x = 0; x < 1200; x++)
        {
            for (int z = 0; z < 1200; z++)
            {
                float xCoord = (float)(x + seed) / 1200 * objectNoiseScale;
                float yCoord = (float)(z + seed) / 1200 * objectNoiseScale;
                float noise = Mathf.PerlinNoise(xCoord, yCoord);
                if (noise >= 0.9)
                {
                    Ray ray = new Ray();
                    ray.origin = new Vector3(x, 50, z);
                    ray.direction = Vector3.down * raycastDistance;
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit))
                    {
                        foreach (var area in objectSpawnAreas)
                        {
                            if (hit.point.y >= area.minY && hit.point.y <= area.maxY)
                            {
                                int prefabID = Random.Range(0, area.prefabs.Length);
                                GameObject go = area.prefabs[prefabID];
                                go.transform.localScale = new Vector3(3, 3, 3);
                                float rotate = Random.Range(0, 180);
                                Instantiate(go, new Vector3(x, hit.point.y, z),
                                    Quaternion.FromToRotation (transform.up, hit.normal) * transform.rotation);
                            }
                        }
                    }
                }
            }
        }

        TerrainSpawn.TerrainSpawned -= SpawnObjects;
    }
    
    private void SpawnGrass()
    {
        for (int x = 0; x < 1200; x++)
        {
            for (int z = 0; z < 1200; z++)
            {
                float xCoord = (float)(x + seed) / 1200 * grassNoiseScale;
                float yCoord = (float)(z + seed) / 1200 * grassNoiseScale;
                float noise = Mathf.PerlinNoise(xCoord, yCoord);
                if (noise >= 0.85)
                {
                    Ray ray = new Ray();
                    ray.origin = new Vector3(x, 50, z);
                    ray.direction = Vector3.down * raycastDistance;
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit))
                    {
                        foreach (var area in grassSpawnAreas)
                        {
                            if (hit.point.y >= area.minY && hit.point.y <= area.maxY)
                            {
                                int prefabID = Random.Range(0, area.prefabs.Length);
                                GameObject go = area.prefabs[prefabID];
                                go.transform.localScale = new Vector3(3, 3, 3);
                                float rotate = Random.Range(0, 180);
                                Instantiate(go, new Vector3(x, hit.point.y, z),
                                    Quaternion.FromToRotation (transform.up, hit.normal) * transform.rotation);
                            }
                        }
                    }
                }
            }
        }

        TerrainSpawn.TerrainSpawned -= SpawnGrass;
    }
}

[System.Serializable]
public class ObjectSpawnArea
{
    public string name;
    public float minY;
    public float maxY;
    public GameObject[] prefabs;
}