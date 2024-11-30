using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject swamerPrefab;

    [SerializeField]
    private float swarmerInterval = 3.5f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnEnemy(swarmerInterval, swamerPrefab));
    }

    // Spawns enemy
    private IEnumerator spawnEnemy(float interval, GameObject enemy)
    {
        // Time interval between enemy spawns
        yield return new WaitForSeconds(interval);

        // Spawns enemy at random location
        GameObject newEnemy = Instantiate(enemy, new Vector3(Random.Range(-5f, 5), Random.Range(-6f, 6), 0), Quaternion.identity);
        
        // Guarentees that enemy spawning is endless (for testing purposes)
        // Code to be replaced with a counter to spawn a finite amount of enemies
        StartCoroutine(spawnEnemy(interval, enemy));
    }
}
