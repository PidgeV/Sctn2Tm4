using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Complete;


public class AttackState : FSMState
{
    float elapsedTime;
    float intervalTime;

    public AttackState(Transform[] wp, NPCTankController npcTank)
    {
        InitializeEnemy(npcTank, FSMStateID.Attacking, wp);

        curRotSpeed = 1.0f;
        curSpeed = 0.0f;
        elapsedTime = 0.0f;
        intervalTime = 5.0f;
    }

    public override void EnterStateInit()
    {
        npcTankController.navAgent.velocity = Vector3.zero;
        elapsedTime = 0.0f;
    }

    //Reason
    public override void Reason()
    {
        Transform npc = npcTankController.gameObject.transform;
        Transform player = npcTankController.GetPlayerTransform();

		//if (If my HP is 0)
		//{
		// npcTankController.PerformTransition(Transition.NoHealth);
		// Return;
		//}

		//if (If my HP is LOW)
		//{
		// npcTankController.PerformTransition(Transition.LowHP);
		// Return;
		//}

		if (IsInCurrentRange(npc, player.position, NPCTankController.ATTACK_DIST))
        {
            // Wait to shoot
            if (npcTankController.receivedAttackCommand)
            {
                //enemyShoot.Firing = true;
            }
            else
            {
                //enemyShoot.Firing = false;
            }
        }
        else if (!IsInCurrentRange(npc, player.position, NPCTankController.ATTACK_DIST))
        {
            //enemyShoot.Firing = false;
            npcTankController.PerformTransition(Transition.SawPlayer);
        }
        else
        {
            //enemyShoot.Firing = false;
            npcTankController.PerformTransition(Transition.LostPlayer);
        }

        // Not sure if right
        elapsedTime += Time.deltaTime;
        if (elapsedTime > intervalTime)
        {
            elapsedTime = 0.0f;
            npcTankController.Shoot();
            npcTankController.PerformTransition(Transition.SawPlayer);
        }
    }

    //Act
    public override void Act()
    {
        Transform npc = npcTankController.gameObject.transform;
        Transform player = npcTankController.GetPlayerTransform();

        Quaternion leftQuatMax = Quaternion.AngleAxis(-45, new Vector3(0, 1, 0));
        Quaternion rightQuatMax = Quaternion.AngleAxis(45, new Vector3(0, 1, 0));

        UsefullFunctions.DebugRay(npc.position, leftQuatMax * npc.forward * 5.0f, Color.red);
        UsefullFunctions.DebugRay(npc.position, rightQuatMax * npc.forward * 5.0f, Color.red);

        Transform turret = npcTankController.turret.transform;
        Vector3 targetDirection = player.position - npc.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        float angleBetween = Vector3.Angle(targetDirection, npc.forward);
        if (angleBetween < 45)
        {
            // Rotate the turret
            npcTankController.turret.rotation = Quaternion.Slerp(npcTankController.turret.rotation,
               targetRotation, Time.deltaTime * curRotSpeed);
        }
        else
        {
            // Rotate the body
            npc.rotation = Quaternion.Slerp(npc.rotation,
                targetRotation, Time.deltaTime * curRotSpeed);
        }
    }
}
