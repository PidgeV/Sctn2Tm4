using Complete;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingState : FSMState
{
    public PatrollingState(Transform[] wp, NPCTankController npcTank)
    {
        InitializeEnemy(npcTank, FSMStateID.Patrolling, wp);
       
        destPos = GetRndPosition();
        npcTankController.navAgent.SetDestination(destPos);
    }

    public override void EnterStateInit()
    {
        destPos = GetRndPosition();
        npcTankController.navAgent.SetDestination(destPos);
    }

    //Reason
    public override void Reason()
    {
		if (npcTankController.AdvancedAI && AIManager.Instance.playerFound == true) {
			npcTankController.PerformTransition(Transition.ReachPlayer);
			return;
		}

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

		if (IsInCurrentRange(npcTankController.transform, destPos, NPCTankController.SLOT_DIST))
        {
            destPos = GetRndPosition();
            npcTankController.navAgent.SetDestination(destPos);
        }

        if (CanSeePlayer(180))
		{
			npcTankController.navAgent.velocity = Vector3.zero;
			npcTankController.PerformTransition(Transition.SawPlayer);
        }

    }

    //Act
    public override void Act()
    {

    }

    private Vector3 GetRndPosition()
    {
        GameObject point = new GameObject();
        float dist = Random.Range(25, 90);
        if (dist > 52 && dist < 62)
        {
            dist += 10;
        }
        point.transform.Rotate(0, Random.Range(0, 360), 0);
        point.transform.position = point.transform.forward * dist;
        Object.Destroy(point);
        return point.transform.position;
    }

    public bool CanSeePlayer(float fov)
    {
        Transform npc = npcTankController.gameObject.transform;
        Transform player = npcTankController.GetPlayerTransform();

        Quaternion leftQuatMax = Quaternion.AngleAxis(-fov, new Vector3(0, 1, 0));
        Quaternion rightQuatMax = Quaternion.AngleAxis(fov, new Vector3(0, 1, 0));

        Transform turret = npcTankController.turret.transform;
        Vector3 targetDirection = player.position - npc.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        float angleBetween = Vector3.Angle(targetDirection, npc.forward);
        if (angleBetween < fov && IsInCurrentRange(npc, player.position, 15))
        {
            return true;
        }

        return false;
    }
}
