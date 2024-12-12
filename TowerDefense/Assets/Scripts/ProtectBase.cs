using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectBase : MonoBehaviour
{

    public float range = 4f;
    public string enemyTag = "Enemy";

    [SerializeField]
    public GameMaster MainHub;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameObject.FindWithTag(enemyTag))
        {
            return;
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < range)
            {
                MainHub.lives = MainHub.lives - 1;
                Destroy(enemy);
            }
        }

        
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
