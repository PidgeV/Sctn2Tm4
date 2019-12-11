using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float life = 1;
    public float currentLife;

    [SerializeField] private GameObject explosion;

    private void Start()
    {
        currentLife = life;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Dear Black ... Did you know TryGetComponent<>() is the same as GetComponent<>() But without the garbage collection :p
        if (collision.gameObject.TryGetComponent<Damage>(out Damage hit))
        {
            Instantiate(explosion, transform.position + new Vector3(0, 1, 0), transform.rotation);

            // Take damage
            currentLife -= hit.damage;

            // IF were dead
            if (currentLife <= 0)
            {
                if (gameObject.TryGetComponent<Lives>(out Lives life))
                {
                    life.lives -= 1;
                    if (life.lives == 0)
                    {
                        Destroy(gameObject);
                    }
                    else
                    {
                        Respawn();
                    }
                }
            }
            else
            {
                // Else add knockback
                if (collision.gameObject.TryGetComponent<Rigidbody>(out Rigidbody bullet) &&
                              gameObject.TryGetComponent<Rigidbody>(out Rigidbody thisObject))
                {
                    thisObject.velocity = bullet.velocity * 0.1f;
                    thisObject.angularVelocity = Vector3.zero;
                }
            }

            // Destroy the bullet that his this gameobject
            Destroy(collision.gameObject);
        }
    }

    void Respawn()
    {
        transform.position = new Vector3(0, 0, -65);
        transform.rotation = Quaternion.identity;
        currentLife = life;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    currentLife -= 1;
    //    if (currentLife <= 0)
    //    {
    //        Destroy(gameObject);
    //    }
    //    else
    //    {
    //        if (gameObject.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
    //        {
    //            rigidbody.velocity = other.GetComponent<Rigidbody>().velocity * 0.1f;
    //        }
    //    }
    //}
}
