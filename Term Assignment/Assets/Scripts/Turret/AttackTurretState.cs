using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Complete;


public class AttackTurretState : FSMState
{
    float elapsedTime;
    float intervalTime;

	private TurretController turretController;

	public AttackTurretState(Transform[] wp, TurretController turret)
    {
        turretController = turret;
        stateID = FSMStateID.Attacking;
        waypoints = wp;

        curRotSpeed = 1.0f;
        elapsedTime = 0.0f;
        intervalTime = 5.0f;
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
            elapsedTime += Time.deltaTime;
            if (elapsedTime > intervalTime)
            {
                elapsedTime = 0.0f;
                turretController.Shoot();
                // npcTankController.PerformTransition(Transition.SawPlayer);
            }
        }
        else if (IsInCurrentRange(npc, player.position, TurretController.WARNING_DIST))
        {
            //enemyShoot.Firing = false;
            turretController.PerformTransition(Transition.SawPlayer);
        }
        else
        {
            //enemyShoot.Firing = false;
            turretController.PerformTransition(Transition.LostPlayer);
        }

        // Not sure if right

    }

    //Act
    public override void Act()
    {
        Transform npc = turretController.gameObject.transform;
        Transform player = turretController.GetPlayerTransform();


        UsefullFunctions.DebugRay(npc.position,  npc.forward, Color.red);

        Transform turret = turretController.turret.transform;
        Vector3 targetDirection = player.position - npc.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        // Rotate the turret
        turretController.turret.rotation = Quaternion.Slerp(turretController.turret.rotation,
        targetRotation, Time.deltaTime * curRotSpeed);


    }
}
