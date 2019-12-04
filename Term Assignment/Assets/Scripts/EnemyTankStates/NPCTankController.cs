//NPCTankController
//This class is derived from AdvancedFSM and holds the FSM for the NPC tank
//each tank must have this attached to it in order to have "tank" behaviour
//
namespace Complete
{
	using UnityEngine;
	using UnityEngine.AI;
	using UnityEngine.UI;
	using System.Collections;
	using System.Collections.Generic;

	public class NPCTankController : AdvancedFSM
	{
		public static int CHASE_DIST = 50;
		public static int SLOT_DIST = 1;
		public static int WAYPOINT_DIST = 1;
		public static int ATTACK_DIST = 35;

		public int m_CharNumber = 1;                        // Used to identify which tank belongs to which character.  This is set by this tank's manager.
		public SlotManager coverPositionsSlotManager;
		public NavMeshAgent navAgent;
		public Transform turret;

		[HideInInspector]
		public Rigidbody rigBody;

		[HideInInspector]
		public bool receivedAttackCommand = false;

		private Transform playerTransform;
		private GameObject[] pointList;
		private SlotManager playerSlotManager;
		private bool debugDraw;

		public bool AdvancedAI = true;

		public Transform GetPlayerTransform()
		{
			return playerTransform;
		}
		public SlotManager GetPlayerSlot()
		{
			return playerSlotManager;
		}

		private string GetStateString()
		{
			string state = "NONE";
			if (CurrentState != null)
			{
				if (CurrentState.ID == FSMStateID.Dead)
				{
					state = "DEAD";
				}
				else if (CurrentState.ID == FSMStateID.Ambushing)
				{
					state = "PATROL";
				}
				else if (CurrentState.ID == FSMStateID.Chasing)
				{
					state = "CHASE";
				}
				else if (CurrentState.ID == FSMStateID.Attacking)
				{
					state = "ATTACK";
				}
			}

			return state;
		}

		// Initialize the FSM for the NPC tank.
		protected override void Initialize()
		{
			debugDraw = true;

			// Find the Player and init appropriate data.
			GameObject objPlayer = GameObject.FindGameObjectWithTag("Player");
			playerTransform = objPlayer.transform;
			playerSlotManager = objPlayer.GetComponent<SlotManager>();

			rigBody = GetComponent<Rigidbody>();

			receivedAttackCommand = false;

			// Create the FSM for the tank.
			ConstructFSM();

		}

		// Update each frame.
		protected override void FSMUpdate()
		{
			if (CurrentState != null)
			{
				CurrentState.Reason();
				CurrentState.Act();
			}
			if (debugDraw)
			{
				UsefullFunctions.DebugRay(transform.position, transform.forward * 5.0f, Color.red);
			}
		}
		protected override void FSMFixedUpdate()
		{
		}

		/// <summary>
		/// Where we add our states
		/// </summary>
		private void ConstructFSM()
		{
			// ________________
			// Make Chase State
			ChaseState chaseState = new ChaseState(null, this);
			chaseState.AddTransition(Transition.NoHealth, FSMStateID.Dead);
			chaseState.AddTransition(Transition.ReachPlayer, FSMStateID.Attacking);
			chaseState.AddTransition(Transition.LostPlayer, FSMStateID.Patrolling);
			chaseState.AddTransition(Transition.LowHP, FSMStateID.Desperation);
			AddFSMState(chaseState);

			// ______________________
			// Make Desperation State
			DesperationState desperationState = new DesperationState(null, this);
			desperationState.AddTransition(Transition.NoHealth, FSMStateID.Dead);
			AddFSMState(desperationState);

			// _________________
			// Make Attack State 
			if (AdvancedAI)
			{
				AttackAdvancedState attackAdvancedState = new AttackAdvancedState(null, this);
				attackAdvancedState.AddTransition(Transition.NoHealth, FSMStateID.Dead);
				AddFSMState(attackAdvancedState);
			}
			else
			{
				AttackState attackState = new AttackState(null, this);
				attackState.AddTransition(Transition.NoHealth, FSMStateID.Dead);
				attackState.AddTransition(Transition.LowHP, FSMStateID.Desperation);
				attackState.AddTransition(Transition.LostPlayer, FSMStateID.Patrolling);
				attackState.AddTransition(Transition.SawPlayer, FSMStateID.Chasing);
				AddFSMState(attackState);
			}

			// _____________________
			// Make Patrolling State
			PatrollingState patrollingState = new PatrollingState(null, this);
			patrollingState.AddTransition(Transition.NoHealth, FSMStateID.Dead);
			patrollingState.AddTransition(Transition.SawPlayer, FSMStateID.Chasing);
			patrollingState.AddTransition(Transition.ReachPlayer, FSMStateID.Attacking);
			patrollingState.AddTransition(Transition.LowHP, FSMStateID.Desperation);
			AddFSMState(patrollingState);

		}

		private void OnEnable()
		{
			if (navAgent)
				navAgent.isStopped = false;
			if (CurrentState != null)
				PerformTransition(Transition.Enable);
		}
		private void OnDisable()
		{
			if (navAgent && navAgent.isActiveAndEnabled)
				navAgent.isStopped = true;
		}
	}
}
