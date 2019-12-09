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

    public class TurretController : AdvancedFSM
    {
        public static int ATTACK_DIST = 25;
        public static int WARNING_DIST = 40;


        public int m_CharNumber = 1;                        // Used to identify which tank belongs to which character.  This is set by this tank's manager.
      //  public SlotManager coverPositionsSlotManager;
      //  public NavMeshAgent navAgent;
        public Transform turret;
        public Light light;


        [SerializeField] private GameObject bullet;
       // [SerializeField] private Transform barrel;

        [HideInInspector]
        public Rigidbody rigBody;

        [HideInInspector]
        public bool receivedAttackCommand = false;


        private Transform playerTransform;
        private GameObject[] pointList;
      //  private SlotManager playerSlotManager;
        private bool debugDraw;


        public Transform GetPlayerTransform()
        {
            return playerTransform;
        }
      /*  public SlotManager GetPlayerSlot()
        {
            return playerSlotManager;
        }
        */
        private string GetStateString()
        {
            string state = "NONE";
            if (CurrentState != null)
            {
                if (CurrentState.ID == FSMStateID.Dead)
                {
                    state = "DEAD";
                }
                else if (CurrentState.ID == FSMStateID.Patrolling)
                {
                    state = "STANDBY";
                }
                
                else if (CurrentState.ID == FSMStateID.Attacking)
                {
                    state = "ATTACK";
                }

                else if (CurrentState.ID == FSMStateID.Warning)
                {
                    state = "WARNING";
                }
            }

            return state;
        }

        // Initialize the FSM for the NPC turret.
        protected override void Initialize()
        {
            debugDraw = true;


            // Find the Player and init appropriate data.
            GameObject objPlayer = GameObject.FindGameObjectWithTag("Player");
            playerTransform = objPlayer.transform;
          //  playerSlotManager = objPlayer.GetComponent<SlotManager>();

            rigBody = GetComponent<Rigidbody>();

            receivedAttackCommand = false;

            // Create the FSM for the turret.
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
            //Patrolling State
            StandByState standByState = new StandByState(null, this);
            standByState.AddTransition(Transition.NoHealth, FSMStateID.Dead);
            standByState.AddTransition(Transition.ReachPlayer, FSMStateID.Attacking);
            standByState.AddTransition(Transition.SawPlayer, FSMStateID.Warning);
            AddFSMState(standByState);


            // Warning State
            WarningState warningState = new WarningState(null, this);
            warningState.AddTransition(Transition.NoHealth, FSMStateID.Dead);
            warningState.AddTransition(Transition.ReachPlayer, FSMStateID.Attacking);
            warningState.AddTransition(Transition.LostPlayer, FSMStateID.Patrolling);
            AddFSMState(warningState);

            // Attack State
            AttackTurretState attackState = new AttackTurretState(null, this);
            attackState.AddTransition(Transition.NoHealth, FSMStateID.Dead);
            attackState.AddTransition(Transition.SawPlayer, FSMStateID.Warning);
            attackState.AddTransition(Transition.LostPlayer, FSMStateID.Patrolling);
            AddFSMState(attackState);



        }

        private void OnEnable()
        {
           /* if (navAgent)
                navAgent.isStopped = false; */
            if (CurrentState != null)
                PerformTransition(Transition.Enable);
        }
        private void OnDisable()
        {
          /*  if (navAgent && navAgent.isActiveAndEnabled)
                navAgent.isStopped = true;
                */
        }

        public void Shoot()
        {
            GameObject shot = GameObject.Instantiate(bullet);

            //adding ofset to compensate for the height of the turret base. Without this turret shoots in the sky.
            //even with offset it doesn't aim player properly.
            Vector3 offset = new Vector3(0, 2, 0);
            shot.transform.position = turret.position;

            if (shot.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
            {
                  rigidbody.velocity = playerTransform.position - shot.transform.position;
            }
        }
    }


}
