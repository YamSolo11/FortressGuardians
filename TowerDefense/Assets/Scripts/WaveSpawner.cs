using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public enum SpawnState { SPAWNING, WAITING, COUNTING};

    public Transform enemy1;
    public Transform enemy2;
    public Transform enemy3;
    public Transform enemy4;
    public Transform boss;

    public float timeBetweenWaves = 5f;
    private float countdown = 2f;
    private int nextWave = 0;

    private float searchCountdown = 1f;

    private SpawnState state = SpawnState.COUNTING;

    [System.Serializable]
    public class Wave
    {
        public int enemyCount;
        public float spawnRate;
        public int enemy1Spawn = 0;
        public int enemy2Spawn = 0;
        public int enemy3Spawn = 0;
        public int enemy4Spawn = 0;
        public int bossSpawn = 0;
    }

    public Wave[] waves;
    public Transform[] spawnPoints;

    void Update()
    {
        if(state == SpawnState.WAITING)
        {
            //Check if enemies are still alive
            if(!EnemyisAlive() )
            {
                //Begin a new round
                WaveCompleted();
            }
            else
            {
                return;
            }
        }

        if (countdown <= 0f)
        {
            if(state != SpawnState.SPAWNING)
            {
                StartCoroutine(SpawnWave(waves[nextWave]));
                countdown = timeBetweenWaves;
            }
        }
        else
        {
            countdown -= Time.deltaTime;
        }
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        Debug.Log("Spawning Wave");

        state = SpawnState.SPAWNING;

        //spawn
        for(int i = 0; i < _wave.enemyCount; i++)
        {
            if(_wave.enemy1Spawn != 0)
            {
                SpawnEnemy(enemy1);
                _wave.enemy1Spawn--;
                yield return new WaitForSeconds(1f / _wave.spawnRate);
            }
            else if (_wave.enemy2Spawn != 0)
            {
                SpawnEnemy(enemy2);
                _wave.enemy2Spawn--;
                yield return new WaitForSeconds(1f / _wave.spawnRate);
            }
            else if (_wave.enemy3Spawn != 0)
            {
                SpawnEnemy(enemy3);
                _wave.enemy3Spawn--;
                yield return new WaitForSeconds(1f / _wave.spawnRate);
            }
            else if (_wave.enemy4Spawn != 0)
            {
                SpawnEnemy(enemy4);
                _wave.enemy4Spawn--;
                yield return new WaitForSeconds(1f / _wave.spawnRate);
            }
            else if (_wave.bossSpawn != 0)
            {
                SpawnEnemy(boss);
                _wave.bossSpawn--;
                yield return new WaitForSeconds(1f / _wave.spawnRate);
            }
        }    

        state = SpawnState.WAITING;
        yield break;
    }

    void SpawnEnemy(Transform _enemy)
    {
        //Spawn Enemy
        Debug.Log("Spawn Enemy"); 

        Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(_enemy, _sp.position, _sp.rotation);
    }

    bool EnemyisAlive()
    {
        searchCountdown -= Time.deltaTime;

        if(searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
        }

        return true;
    }

    void WaveCompleted()
    {
        Debug.Log("Wave Completed");

        if(nextWave +1 > waves.Length -1)
        {
            nextWave = 0;
            Debug.Log("Finished waves. looping");
        }
        else
        {
            nextWave++;
        }

        state = SpawnState.COUNTING;
        countdown = timeBetweenWaves;
    }
}
