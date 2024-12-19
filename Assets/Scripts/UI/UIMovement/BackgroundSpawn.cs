using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSpawn : MonoBehaviour
{
    public GameObject background;
    public float backgroundSize;
    
    private float spawnTime;
    private float spawnPos;

    // Start is called before the first frame update
    void Start()
    {
        spawnPos = backgroundSize;
        spawnTime = ((backgroundSize * 30) - 0.1f);

        InvokeRepeating("SpawnBackground", spawnTime/2, spawnTime);
    }

    void SpawnBackground()
    {
        Instantiate(background, new Vector3(0f, spawnPos, 0f), background.transform.rotation);
        Instantiate(background, new Vector3(spawnPos, spawnPos, 0f), background.transform.rotation);
        Instantiate(background, new Vector3(spawnPos, 0f, 0f), background.transform.rotation);
    }
}
