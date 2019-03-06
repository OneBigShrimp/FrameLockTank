using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class FreeState : StateBase
{
    public override EntityState State
    {
        get
        {
            return EntityState.Free;
        }
    }

    public override void Enter(StateMachine machine, StateArgsBase args)
    {
    }
}
