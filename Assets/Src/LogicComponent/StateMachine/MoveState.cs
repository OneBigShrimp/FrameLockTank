using UnityEngine;

public class MoveState : StateBase
{
    MoveStateArgs _args;
    bool _isAvaliable = false;

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

    public override bool SameStateEnter(StateMachine machine, StateArgsBase args)
    {
        Enter(machine, args);
        return true;
    }

    public override void Enter(StateMachine machine, StateArgsBase args)
    {
        _isAvaliable = true;
        _args = (args as MoveStateArgs);
    }

    public override bool LogicTick(StateMachine machine, int ms)
    {
        if (!_isAvaliable)
        {
            return true;
        }
        _isAvaliable = false;
        LogicMove lm = machine.HostEntity.GetComponent<LogicMove>();
        if (lm != null)
        {
            AttrComponent attr = machine.HostEntity.GetComponent<AttrComponent>();
            SafeFloat speed = attr.GetAttr(Attr.MoveSpeed);
            lm.MoveOneStep(speed * new SafeV3(_args.DirX, 0, _args.DirZ));
        }

        return false;
    }

    public override void Exit(StateMachine machine)
    {
        base.Exit(machine);
        _isAvaliable = false;
    }
}

public class MoveStateArgs : StateArgsBase
{
    public SafeFloat DirX;
    public SafeFloat DirZ;
}
