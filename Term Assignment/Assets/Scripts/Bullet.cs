using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletLife = 10;

    private float bulletDuration = 0;
    private float hitBoxSize = 1;
    private float maxHitBoxSize = 1;

    private bool resize = false;

    // Update is called once per frame
    void Update()
    {
        //
        bulletDuration += Time.deltaTime;

        //
        if (!resize && bulletDuration > 0.1f)
            resize = true;

        //
        if (bulletDuration > bulletLife)
            Destroy(gameObject);

        //
        if (resize && hitBoxSize < maxHitBoxSize)
        {
            hitBoxSize = Mathf.Lerp(hitBoxSize, maxHitBoxSize, 0.35f);
            GetComponentInChildren<SphereCollider>().radius = hitBoxSize;
        }
    }
}
