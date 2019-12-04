using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Complete;


public class ChaseState : FSMState
{
	SlotManager playerSlot;

	int availableSlotIndex;

	float elapsedTime;
    float intervalTime;

    public ChaseState(Transform[] wp, NPCTankController npcTank)
    {
        InitializeEnemy(npcTank, FSMStateID.Chasing, wp);

        curRotSpeed = 2.0f;
        curSpeed = 3.0f;
        elapsedTime = 0.0f;
        intervalTime = 1.0f;

        playerSlot = npcTank.GetPlayerSlot();
        availableSlotIndex = -1;
        npcTankController.navAgent.SetDestination(playerSlot.GetSlotPosition(availableSlotIndex));
    }

    public override void EnterStateInit()
    {
        destPos = npcTankController.GetPlayerTransform().position;
        playerSlot.ClearSlots(npcTankController.gameObject);
        availableSlotIndex = playerSlot.ReserveSlotAroundObject(npcTankController.gameObject);
        if (availableSlotIndex != -1)
        {
            destPos = playerSlot.GetSlotPosition(availableSlotIndex);
        }
        elapsedTime = 0.0f;
    }

    public override void Act()
    {
        Quaternion targetRotation = Quaternion.LookRotation(destPos - npcTankController.transform.position);
        npcTankController.transform.rotation = Quaternion.Slerp(npcTankController.transform.rotation, targetRotation, Time.deltaTime * curRotSpeed);
        npcTankController.turret.rotation = Quaternion.Slerp(npcTankController.turret.transform.rotation, npcTankController.transform.rotation, Time.deltaTime * curRotSpeed);
    }

    public override void Reason()
    {
        Transform npc = npcTankController.gameObject.transform;
        Transform player = npcTankController.GetPlayerTransform();

		if (IsInCurrentRange(npcTankController.transform, destPos, 5)) {
			if (AIManager.Instance.AdvancedAI == true)
			{
				AIManager.Instance.StartAdvancedStates();
			}
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

		if (npcTankController.AdvancedAI && AIManager.Instance.playerFound == true) {
			npcTankController.PerformTransition(Transition.ReachPlayer);
			return;
		}

        if (!IsInCurrentRange(npcTankController.transform, player.transform.position, NPCTankController.CHASE_DIST)) {
            npcTankController.PerformTransition(Transition.LostPlayer);
        }

        if (IsInCurrentRange(npcTankController.transform, destPos, NPCTankController.SLOT_DIST)) {
			npcTankController.PerformTransition(Transition.ReachPlayer);
		}

		elapsedTime += Time.deltaTime;
		if (elapsedTime > intervalTime)
		{
			elapsedTime = 0.0f;
			destPos = npcTankController.GetPlayerTransform().position;
			playerSlot.ReleaseSlot(availableSlotIndex);
			availableSlotIndex = playerSlot.ReserveSlotAroundObject(npcTankController.gameObject);
			if (availableSlotIndex != -1)
			{
				destPos = playerSlot.GetSlotPosition(availableSlotIndex);
				npcTankController.navAgent.SetDestination(playerSlot.GetSlotPosition(availableSlotIndex));
			}
		}
	}
}
