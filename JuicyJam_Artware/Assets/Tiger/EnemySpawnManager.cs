using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    //Array of all the spawnpoints.
    [SerializeField] GameObject[] allSpawnPoints;

    //list of enemy types.
    //Spot 0 is for sniper.
    //Spot 1 for charger.
    [SerializeField] public GameObject sniperPrefab;

    Transform playerTransform;

    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        InvokeRepeating("DistanceCheck", 3f, 3f);
    }

    //Checks distance from player every now and then.
    //Sets them to enabled or disabled based on whether they are too far.
    void DistanceCheck()
    {
        for(int i = 0; i < allSpawnPoints.Length; i++) 
        {
            Vector3 spawnPos = allSpawnPoints[i].transform.position;
            Vector3 playerPos = playerTransform.position;

            if (Vector3.Distance(spawnPos, playerPos) < 100 && Vector3.Distance(spawnPos, playerPos) > 30)
            {
                allSpawnPoints[i].GetComponent<EnemySpawner>().enabled = true;
            }
            else
            {
                allSpawnPoints[i].GetComponent<EnemySpawner>().enabled = false;
            }
        }
    }
}
