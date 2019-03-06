using System;
using System.Collections.Generic;


public delegate void OnProcessEffect(ref EffectData data);

public delegate void OnBeforeDie();

public class EffectReceiver : LogicComponentBase
{
    LinkedList<OnProcessEffect> beforeProcesses = new LinkedList<OnProcessEffect>();

    LinkedList<OnProcessEffect> afterProcesses = new LinkedList<OnProcessEffect>();

    OnBeforeDie onDie;

    AttrComponent _attrAgent;

    public EffectReceiver(LogicEntity entity) : base(entity)
    {

    }

    public override void Init()
    {
        _attrAgent = HostEntity.GetComponent<AttrComponent>();
    }

    public void ReciveEffect(EffectData effectData)
    {
        ProcessEffect(beforeProcesses, ref effectData);


        CaculateRealHealNum(effectData);


        ProcessEffect(afterProcesses, ref effectData);

        if (effectData == null)
        {
            return;
        }
        if (effectData.HealNum != 0)
        {
            _attrAgent.ChangeAttr(Attr.Hp, effectData.HealNum);
        }

        for (int i = 0; i < effectData.ChagneAttr.Length; i++)
        {
            _attrAgent.ChangeAttr(effectData.ChagneAttr[i]);
        }
        if (_attrAgent.GetAttr(Attr.Hp) <= 0)
        {
            Die();
        }



    }

    void Die()
    {
        if (onDie != null)
        {
            onDie();
        }

        //死亡逻辑
    }


    void ProcessEffect(LinkedList<OnProcessEffect> onProcessEffects, ref EffectData effectData)
    {
        LinkedListNode<OnProcessEffect> cur = onProcessEffects.First;
        while (cur != null && effectData != null)
        {
            cur.Value(ref effectData);
            cur = cur.Next;
        }
    }

    void CaculateRealHealNum(EffectData effectData)
    {

    }

}


public class EffectData
{

    public int OpEntityKey;
    /// <summary>
    /// 正数表示治疗,负数表示伤害
    /// </summary>
    public SafeFloat HealNum;
    public AttrPair[] ChagneAttr;

}

public struct AttrPair
{
    public Attr AttrType;
    public SafeFloat Value;

    public AttrPair(Attr attr, SafeFloat value)
    {
        this.AttrType = attr;
        this.Value = value;
    }

}
