using UnityEngine;

public class Bullet : MonoBehaviour
{
    //Public changeable variables depending on the bullet type. 
    private Transform target;

    public float speed = 70f;
    public float explosionRadius = 0f; //If this is greater than 0, the bullet will explode in a radius
    public float damageAmount = 50f;
    public GameObject impactEffect; //effect of the bullet. 


    public void Seek(Transform _target)
    {
        target = _target;
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if(dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

       // transform.Translate(dir.normalized * distanceThisFrame);
       transform.Translate(dir.normalized*distanceThisFrame, Space.World);

    }

    void HitTarget()
    {
        GameObject effectIns = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effectIns, 2f);
        if(explosionRadius > 0f)
        {
            Explode();
        }
        else
        {
            Damage(target);
        }

        Destroy(gameObject);
    }

    void Explode()
    {
        
        Collider[] colliders= Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders)
        {
            if(collider.tag == "Enemy")
            {
                Damage(collider.transform);
            }
        }
    }

    void Damage(Transform enemy)
    {
        enemy.GetComponent<Enemy>().TakeDamage(damageAmount);
        //Destroy(enemy.gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

}
