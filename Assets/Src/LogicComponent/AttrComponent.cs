using System;
using System.Collections.Generic;



public class AttrComponent : LogicComponentBase
{
    /// <summary>
    /// 
    /// </summary>
    Dictionary<Attr, SafeFloat> _attrs = new Dictionary<Attr, SafeFloat>();

    public AttrComponent(LogicEntity entity) : base(entity)
    {
    }

    public SafeFloat GetAttr(Attr attr)
    {
        return _attrs[attr];
    }

    public void SetAttr(AttrPair ap)
    {
        SetAttr(ap.AttrType, ap.Value);
    }

    public void ChangeAttr(AttrPair ap)
    {
        SetAttr(ap.AttrType, _attrs[ap.AttrType] + ap.Value);
    }

    public void ChangeAttr(Attr attr,SafeFloat value)
    {
        SetAttr(attr, _attrs[attr] + value);
    }

    public void SetAttr(Attr attr, SafeFloat value)
    {
        switch (attr)
        {
            case Attr.Hp:
                if (value > _attrs[Attr.MaxHp])
                {
                    value = _attrs[Attr.MaxHp];
                }
                break;
            case Attr.Mp:
                if (value > _attrs[Attr.MaxMp])
                {
                    value = _attrs[Attr.MaxMp];
                }
                break;
            case Attr.MaxHp:
                if (value < 1)
                {
                    value = 1;
                }
                break;
            case Attr.MaxMp:
                if (value < 0)
                {
                    value = 0;
                }
                break;
        }
        _attrs[attr] = value;
    }
}


public enum Attr
{
    Hp = 1,
    Mp = 2,
    MoveSpeed = 3,
    Attack = 4,
    /// <summary>
    /// 防御,影响受到伤害的量
    /// </summary>
    Defence = 5,
    /// <summary>
    /// 治疗系数,影响受到的治疗量
    /// </summary>
    HealRatio = 6,
    /// <summary>
    /// 子弹发射延迟
    /// </summary>
    BulletDelay = 21,
    /// <summary>
    /// 子弹飞行速度
    /// </summary>
    BulletSpeed = 22,
    /// <summary>
    /// 子弹伤害范围
    /// </summary>
    BulletRange = 23,
    /// <summary>
    /// 子弹生命
    /// </summary>
    BulletLife = 24,
    /// <summary>
    /// 攻击后摇
    /// </summary>
    AttackAfter = 32,

    MaxHp = 101,
    MaxMp = 102,
}