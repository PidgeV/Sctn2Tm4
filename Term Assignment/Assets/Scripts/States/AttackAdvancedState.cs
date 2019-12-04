using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Complete;

public class AttackAdvancedState : FSMState
{
	Transform assignedTarget = null;

	/// <summary>
	/// The States Constructor
	/// </summary>
	/// <param name="wp">Waypoints if needed</param>
	/// <param name="npcTank">The tank controller  that is using this script</param>
	public AttackAdvancedState(Transform[] wp, NPCTankController npcTank)
	{
		InitializeEnemy(npcTank, FSMStateID.Attacking, wp);
	}

	/// <summary>
	/// EnterStateInit Works as OnEnable for states
	/// </summary>
	public override void EnterStateInit()
	{
	}
	
	/// <summary>
	/// [Reason] The Logic for this State. For example, when should we change states?
	/// </summary>
	public override void Reason()
	{
		Transform npc = npcTankController.gameObject.transform;
		Transform player = npcTankController.GetPlayerTransform();

		// Get a target
		if (assignedTarget == null)
		{
			assignedTarget = AIManager.Instance.GetTarget();
			npcTankController.navAgent.SetDestination(assignedTarget.position);
		}
	}
	
	/// <summary>
	/// [Act] This works as Update for this state
	/// </summary>
	public override void Act()
	{
	}
}
