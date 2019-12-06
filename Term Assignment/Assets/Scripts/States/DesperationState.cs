using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Complete;

public class DesperationState : FSMState
{
    public DesperationState(Transform[] wp, NPCTankController npcTank)
    {
       // InitializeEnemy(npcTank, FSMStateID.None, wp);
    }

    public override void EnterStateInit()
    {
        npcTankController.navAgent.velocity = Vector3.zero;
        Transform player = npcTankController.GetPlayerTransform();
		npcTankController.navAgent.SetDestination(player.position);
	}

    //Reason
    public override void Reason()
    {    
        //if (If my HP is 0)
        //{
        // npcTankController.PerformTransition(Transition.NoHealth);
        // Return;
        //}
    }

    //Act
    public override void Act()
    {
    }
}
