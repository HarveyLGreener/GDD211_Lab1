using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public List<GameObject> obstacles;
    [SerializeField] private Transform spawnLocation;
    public List<GameObject> currentObst;
    public GameObject prefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Spawn()
    {
        GameObject newObst = Instantiate(prefab);
        Debug.Log(newObst);
        currentObst.Add(newObst);
        Debug.Log(currentObst);
        if (currentObst.Count >= 2)
        {
            
        }
    }
}
