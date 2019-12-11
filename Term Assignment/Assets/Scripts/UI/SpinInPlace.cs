using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinInPlace : MonoBehaviour
{
    public float rotSpeed = 1.0f;

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(Vector3.back, rotSpeed * Time.deltaTime);
    }
}
