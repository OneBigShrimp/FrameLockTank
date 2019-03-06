using UnityEngine;
using System.Collections;

public abstract class LogicComponentBase : ILogicTick
{
    public LogicEntity HostEntity { get; private set; }

    

    public LogicComponentBase(LogicEntity entity)
    {
        this.HostEntity = entity;
    }

    public virtual void Init()
    {

    }

    public virtual void LogicTick(int ms)
    {

    }

    public virtual void OnGameObjectLoaded(GameObject go)
    {

    }

    public virtual void OnComponentDestroy()
    {

    }
}

