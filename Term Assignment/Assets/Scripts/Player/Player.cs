﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform cameraHelper;
    [SerializeField] private GameObject camera;

    [SerializeField] private Transform barrel;
    [SerializeField] private GameObject bullet;

    public float speed = 15f;
    public float rotation = 55f;

    private float currentSpeed;
    private float currentRotation;

    public float shootCooldown = 1.0f;
    public float shootInterval = 0.0f;

    public VisualEffect smokeEffect;

    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = speed;
        currentRotation = rotation;
        shootInterval = 0.0f;


    }

    // Update is called once per frame
    void Update()
    {
        shootInterval += Time.deltaTime;
        if (shootInterval > shootCooldown)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                smokeEffect.SendEvent("EmitSmoke");
                GameObject shot = GameObject.Instantiate(bullet);
                shot.transform.position = barrel.position;

                if (shot.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
                {
                    rigidbody.velocity = transform.forward * 100;
                }

                shootInterval = 0.0f;
            }
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = Mathf.Lerp(currentSpeed, speed * 3.0f, 0.01f);
            currentRotation = Mathf.Lerp(currentRotation, rotation * 0.2f, 0.01f);
        }
        else
        {
            currentSpeed = Mathf.Lerp(currentSpeed, speed, 0.01f);
            currentRotation = Mathf.Lerp(currentRotation, rotation, 0.01f);
        }

        Vector3 tragetPos = transform.position + (transform.forward * -5.0f) + new Vector3(0, 5, 0);

        float x = Input.GetAxis("Horizontal") * Time.deltaTime * currentRotation;
        float y = Input.GetAxis("Vertical") * Time.deltaTime * currentSpeed;

        y = y < 0 ? y *= 0.4f : y;

        transform.Translate(0, 0, y);
        transform.Rotate(0, x, 0);

        camera.transform.position = Vector3.Lerp(camera.transform.position, tragetPos, 0.1f);
        camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation, cameraHelper.rotation, 0.1f);
    }
}
