using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private GameObject cube;
    [SerializeField] private Health health;

    [SerializeField] private GameObject target;

    private void Start()
    {
        target = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target.transform.position);
    }

    private void FixedUpdate()
    {
        cube.transform.localScale = new Vector3(0.00001f, 0.5f, health.currentLife);
    }
}