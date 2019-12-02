using Complete;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingState : FSMState
{
    NPCTankController NPCTankController;

    public PatrollingState(Transform[] wp, NPCTankController npcTank)
    {
        NPCTankController = npcTank;
        waypoints = wp;

        destPos = waypoints[Random.Range(0, waypoints.Length)].position;
        NPCTankController.navAgent.SetDestination(destPos);
    }

    public override void EnterStateInit()
    {
        destPos = waypoints[Random.Range(0, waypoints.Length)].position;
        NPCTankController.navAgent.SetDestination(destPos);
    }

    //Reason
    public override void Reason()
    {
        if (IsInCurrentRange(NPCTankController.transform, destPos, 2))
        {
            destPos = waypoints[Random.Range(0, waypoints.Length)].position;
            NPCTankController.navAgent.SetDestination(destPos);
        }
    }

    //Act
    public override void Act()
    {

    }
}
