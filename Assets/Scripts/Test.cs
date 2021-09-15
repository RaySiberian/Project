using UnityEngine;
using UnityEngine.AI;

public class Test : MonoBehaviour
{
    private NavMeshSurface navMeshSurface;
    private void Start()
    {
        navMeshSurface = GetComponent<NavMeshSurface>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            navMeshSurface.BuildNavMesh();
        }   
    }
}
