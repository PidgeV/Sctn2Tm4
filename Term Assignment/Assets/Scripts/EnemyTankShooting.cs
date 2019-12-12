using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTankShooting
{
    public float m_rateOfFire = 3.0f;         // The rate at which shells will be fired (in secs).

    public bool Firing                          // Allows manager to set when this tank can fire.
    {
        get { return bFiring; }
        set { bFiring = value; }
    }


    private bool bFiring;                       // Controls whether take is allowed to fire.
    private float m_fireTimer;                  // Controls the fire rate;


    private void OnEnable()
    {
        // When the tank is turned on, reset the launch force and the UI
        //m_CurrentLaunchForce = m_MinLaunchForce;
        m_fireTimer = 0.0f;
        bFiring = false;
    }


    private void Start()
    {
        // When the tank is turned on, reset the launch force and the UI
        //m_CurrentLaunchForce = m_MinLaunchForce;
        m_fireTimer = 0.0f;
        bFiring = false;
    }


    private void Update()
    {
        //Checking if we are able to fire (was I told to fire?)
        if (bFiring)
        {
            // Update the fire time.
            m_fireTimer += Time.deltaTime;

            // Check if it is time to fire... 
            if (m_fireTimer > m_rateOfFire)
            {
                Fire();
                m_fireTimer = 0.0f;
            }
        }
    }


    private void Fire()
    {
        //// Create an instance of the shell and store a reference to it's rigidbody.
        //Rigidbody shellInstance =
        //    Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

        //// Set the shell's velocity to the launch force in the fire position's forward direction.
        //shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward;

        //// Change the clip to the firing clip and play it.
        //m_ShootingAudio.clip = m_FireClip;
        //m_ShootingAudio.Play();

        //// Reset the launch force.  This is a precaution in case of missing button events.
        //m_CurrentLaunchForce = m_MinLaunchForce;
    }
}