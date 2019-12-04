using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Complete;
public class DeadState : FSMState
{
    public DeadState(Transform[] wp, NPCTankController npcTank)
    {
        //InitializeEnemy(npcTank, FSMStateID.Dead, wp);
    }

    public override void EnterStateInit()
    {
    }

    //Reason
    public override void Reason()
    {
    }

    //Act
    public override void Act()
    {
    }
}
