using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    EnemySpawnManager enemySpawnManager;
    void Start()
    {
    }

    private void OnEnable()
    {
        Invoke("SpawnEnemies", Random.Range(1, 5));
        enemySpawnManager = GameObject.FindWithTag("GameManager").GetComponent<EnemySpawnManager>();
    }

    //Spawns a random number of enemies, and a random type.
    //Strip down and remove the chance thing if only 1 enemy.
    //Rudimentary system, lots of space for optimisation, but fills its purpose.
    void SpawnEnemies()
    {
        int numberOfEnemies = Random.Range(2, 4);

        for(int index = 0; index < numberOfEnemies; index++)
        {
            int randomChance = Random.Range(0, 10);

            if (randomChance < 7)
            {
                Instantiate(enemySpawnManager.enemyTypes[0], gameObject.transform);
            }
            else
            {
                Instantiate(enemySpawnManager.enemyTypes[1], gameObject.transform);
            }
        }

        Invoke("SpawnEnemies", Random.Range(1, 5));
    }
}
