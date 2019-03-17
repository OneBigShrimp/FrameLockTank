using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class StateMachine : LogicComponentBase
{
    Dictionary<EntityState, StateBase> allStates = new Dictionary<EntityState, StateBase>();

    StateBase curState;
    public StateMachine(LogicEntity entity) : base(entity)
    {

    }

    public EntityState GetCurState()
    {
        return curState.State;
    }

    public bool TryChangeState(EntityState state, StateArgsBase args)
    {
        if (state == GetCurState())
        {
            return curState.SameStateEnter(this, args);
        }
        StateBase targetState = allStates[state];
        if (targetState.ChechChangeState(this, args))
        {
            if (curState != null)
            {
                curState.Exit(this);
            }
            curState = targetState;
            targetState.Enter(this, args);
            return true;
        }
        else
        {
            return false;
        }
    }

}

public abstract class StateBase
{
    public abstract EntityState State { get; }

    int _forbidCount = 0;
    public void ChangeForbidCount(bool isAdd)
    {
        if (isAdd)
        {
            this._forbidCount++;
        }
        else
        {
            this._forbidCount--;
        }
    }

    public virtual bool SameStateEnter(StateMachine machine, StateArgsBase args)
    {
        return false;
    }

    public bool ChechChangeState(StateMachine machine, StateArgsBase args)
    {
        if (this._forbidCount == 0)
        {
            return CheckChangeState_Prot(machine, args);
        }
        else
        {
            return false;
        }
    }

    public virtual bool CheckChangeState_Prot(StateMachine machine, StateArgsBase args)
    {
        return true;
    }

    public abstract void Enter(StateMachine machine, StateArgsBase args);

    public virtual bool LogicTick(StateMachine machine, int ms)
    {
        return false;
    }

    public virtual void Exit(StateMachine machine) { }


}

public enum EntityState
{
    Free = 1,
    Move = 2,
    Skill = 3,
    Die = 4,
}

public class StateArgsBase
{

}
