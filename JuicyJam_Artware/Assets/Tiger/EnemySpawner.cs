using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    EnemySpawnManager enemySpawnManager;

    [SerializeField] Vector3 spawnPosition;
    LayerMask rayMask = 1 << 8;
    RaycastHit hit;

    private void OnEnable()
    {
        Invoke("SpawnEnemies", Random.Range(5, 9));
        enemySpawnManager = GameObject.FindWithTag("GameManager").GetComponent<EnemySpawnManager>();

        if(Physics.Raycast(transform.position, Vector3.down, out hit, 20, rayMask))
            spawnPosition = new Vector3(hit.point.x, hit.point.y + 0.5f, hit.point.z);
    }

    //Spawns a random number of enemies, and a random type.
    //Strip down and remove the chance thing if only 1 enemy.
    //Rudimentary system, lots of space for optimisation, but fills its purpose.
    void SpawnEnemies()
    {
        int numberOfEnemies = Random.Range(1, 3);

        for(int index = 0; index < numberOfEnemies; index++)
        {
            Vector3 spawnOffset = new Vector3(spawnPosition.x + Random.Range(-7, 7), spawnPosition.y, spawnPosition.z + Random.Range(-7, 7));
            Instantiate(enemySpawnManager.sniperPrefab, spawnOffset, new Quaternion(0,0,0,0));
        }

        Invoke("SpawnEnemies", Random.Range(5, 9));
    }
}
