using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Complete;

using static AIManager;

public class AttackAdvancedState : FSMState
{
	EnemyJob myJob = EnemyJob.NONE;

	Transform assignedTarget = null;
	SlotManager playerSlot;

	int availableSlotIndex;

	float elapsedTime;
	float intervalTime;

	/// <summary>
	/// The States Constructor
	/// </summary>
	/// <param name="wp">Waypoints if needed</param>
	/// <param name="npcTank">The tank controller  that is using this script</param>
	public AttackAdvancedState(Transform[] wp, NPCTankController npcTank)
	{
		InitializeEnemy(npcTank, FSMStateID.Attacking, wp);
		playerSlot = npcTank.GetPlayerSlot();
		availableSlotIndex = -1;

		curRotSpeed = 1.0f;
		curSpeed = 0.0f;
		elapsedTime = 0.0f;
		intervalTime = 3.5f;
	}

	/// <summary>
	/// EnterStateInit Works as OnEnable for states
	/// </summary>
	public override void EnterStateInit()
	{
        npcTankController.navAgent.velocity = Vector3.zero;
		elapsedTime = 0.0f;
	}
	
	/// <summary>
	/// [Reason] The Logic for this State. For example, when should we change states?
	/// </summary>
	public override void Reason()
	{
		// Get a target
		if (myJob == EnemyJob.NONE) {
			myJob = AIManager.Instance.GetJob();
		}

		if (myJob == EnemyJob.GUARD)
		{
			if (assignedTarget == null)
			{
				if (!AIManager.Instance.GetTarget(ref assignedTarget))
				{
					myJob = EnemyJob.ATTACKER;
				}
			}

			npcTankController.navAgent.SetDestination(assignedTarget.position);
		}

		if (myJob == EnemyJob.ATTACKER)
		{
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

        // Not sure if right
        elapsedTime += Time.deltaTime;
        if (elapsedTime > intervalTime)
        {
            elapsedTime = 0.0f;
            npcTankController.Shoot();
            //npcTankController.PerformTransition(Transition.SawPlayer);
        }
    }
	
	/// <summary>
	/// [Act] This works as Update for this state
	/// </summary>
	public override void Act()
	{
		Transform npc = npcTankController.gameObject.transform;
		Transform player = npcTankController.GetPlayerTransform();

		if (player == null || npc == null) {
			return;
		}

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
