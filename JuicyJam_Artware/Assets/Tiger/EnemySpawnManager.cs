using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField] GameObject[] allSpawnPoints;

    Transform playerTransform;

    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        InvokeRepeating("DistanceCheck", 3f, 3f);
    }

    void Update()
    {
        
    }

    void DistanceCheck()
    {
        for(int i = 0; i < allSpawnPoints.Length; i++) 
        {
            if (Vector3.Distance(allSpawnPoints[i].transform.position, playerTransform.position) < 10) 
            {
                allSpawnPoints[i].GetComponent<EnemySpawner>().enabled = true;
                //allSpawnPoints[i].SetActive(true);
            }
            else
            {
                allSpawnPoints[i].GetComponent<EnemySpawner>().enabled = false;
                //allSpawnPoints[i].SetActive(false);
            }
        }
    }
}
