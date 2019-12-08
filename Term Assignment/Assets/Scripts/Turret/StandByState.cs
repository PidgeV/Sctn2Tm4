using Complete;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandByState : FSMState
{
    TurretController turretController;

    public StandByState(Transform[] wp, TurretController turret)
    {
        turretController = turret;
        waypoints = wp;

        //destPos = waypoints[Random.Range(0, waypoints.Length)].position;
       // TurretController.navAgent.SetDestination(destPos);
    }

    public override void EnterStateInit()
    {
        //destPos = waypoints[Random.Range(0, waypoints.Length)].position;
     //   TurretController.navAgent.SetDestination(destPos);
    }

    //Reason
    public override void Reason()
    {
        Transform npcTurret = turretController.gameObject.transform;
        Transform player = turretController.GetPlayerTransform();

		// if (enemyHealth && enemyHealth.isDead)
		//  {
		//      turretController.PerformTransition(Transition.NoHealth);
		//     return;
		//  }

		if (IsInCurrentRange(turretController.transform, player.position, TurretController.WARNING_DIST))
        {
            turretController.PerformTransition(Transition.SawPlayer);

        }
    }
    //Act
    public override void Act()
    {

    }
}
