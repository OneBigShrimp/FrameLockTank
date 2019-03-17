using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class MoveState : StateBase
{
    public override EntityState State
    {
        get
        {
            return EntityState.Move;
        }
    }

    public override bool CheckChangeState_Prot(StateMachine machine, StateArgsBase args)
    {
        if (machine.GetCurState() == EntityState.Skill)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public override void Enter(StateMachine machine, StateArgsBase args)
    {
        
    }
}
