using UnityEngine;
using System.Collections;

public class LogicAnim : LogicComponentBase
{
    /// <summary>
    /// 0: 空闲
    /// 1: 移动
    /// 2; 攻击
    /// </summary>
    ActionStep[] asAry;

    public LogicAnim(LogicEntity entity) : base(entity)
    {

    }

    public void Play(int animIndex)
    {

    }

    public override void LogicTick(int ms)
    {

    }


    class ActionStep
    {
        int animLength;
        string resName;
    }



}
