using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Complete;


public class WarningState : FSMState
{
    float elapsedTime;
    float intervalTime;

	private TurretController turretController;

	public WarningState(Transform[] wp, TurretController turret)
    {
        turretController = turret;
        stateID = FSMStateID.Warning;
        waypoints = wp;

        curRotSpeed = 1.0f;
        elapsedTime = 0.0f;
        intervalTime = 5.0f;
       // turretController.light = turretController.GetComponent<Light>();


    }

    public override void EnterStateInit()
    {
        // npcTankController.navAgent.velocity = Vector3.zero;
        elapsedTime = 0.0f;
    }

    //Reason
    public override void Reason()
    {
        Transform npc = turretController.gameObject.transform;
        Transform player = turretController.GetPlayerTransform();

        //if (If my HP is 0)
        //{
        // turretController.PerformTransition(Transition.NoHealth);
        // Return;
        //}

        //if (If my HP is LOW)
        //{
        // npcTankController.PerformTransition(Transition.LowHP);
        // Return;
        //}

        if (IsInCurrentRange(npc, player.position, TurretController.ATTACK_DIST))
        {
            turretController.PerformTransition(Transition.ReachPlayer);

        }
        else if (!IsInCurrentRange(npc, player.position, TurretController.WARNING_DIST))
        {
            //enemyShoot.Firing = false;
            turretController.PerformTransition(Transition.LostPlayer);
        }



    }

    //Act
    public override void Act()
    {

        Transform npc = turretController.gameObject.transform;
        Transform player = turretController.GetPlayerTransform();


        UsefullFunctions.DebugRay(npc.position, npc.forward, Color.red);

        Transform turret = turretController.turret.transform;

        Vector3 targetDirection = player.position - npc.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        // Rotate the turret
        turret.rotation = Quaternion.Slerp(turret.rotation,
        targetRotation, Time.deltaTime * curRotSpeed);

        turretController.light.color = Color.yellow;

    }
}
