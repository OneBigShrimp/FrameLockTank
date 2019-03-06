using System;
using System.Collections.Generic;


public class SkillArgs : StateArgsBase
{
    public int SkillId;
}


public class SkillState : StateBase
{
    private bool _hasTriggerBullet;

    private int _flagTime;
    public override EntityState State
    {
        get
        {
            return EntityState.Skill;
        }
    }

    public SkillState()
    {

    }

    public override bool CheckChangeState_Prot(StateMachine machine, StateArgsBase args)
    {
        SkillArgs skillArgs = args as SkillArgs;
        //todo: 眩晕状态等等状态下禁止释放技能
        return true;
    }

    public override void Enter(StateMachine machine, StateArgsBase args)
    {
        _flagTime = 0;
        _hasTriggerBullet = false;
    }

    public override bool LogicTick(StateMachine machine, int ms)
    {
        AttrComponent attr = machine.HostEntity.GetComponent<AttrComponent>();
        _flagTime += ms;
        if (!_hasTriggerBullet && _flagTime > attr.GetAttr(Attr.BulletDelay))
        {
            EntityManager.Instance.CreateBullet(machine.HostEntity);
            _hasTriggerBullet = true;
            _flagTime = 0;
        }
        return _hasTriggerBullet && _flagTime > attr.GetAttr(Attr.AttackAfter);
    }
}
