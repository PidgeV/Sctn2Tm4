using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float life = 1;
    public float currentLife;

    private void Start()
    {
        currentLife = life;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Damage dam = collision.gameObject.GetComponent<Damage>();

        if (dam != null)
        {
            currentLife -= dam.damage;
            if (currentLife <= 0)
            {
                Destroy(gameObject);
            }
            else
            {
                if (gameObject.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
                {
                    rigidbody.velocity = collision.gameObject.GetComponent<Rigidbody>().velocity * 0.1f;
                }
            }
        }
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
