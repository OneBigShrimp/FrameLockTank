using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LogicEntity
{
    static int AutoKey = 0;

    /// <summary>
    /// 逻辑实体对应的场景gameobject,可空
    /// </summary>
    GameObject _sceneGo;

    List<EntityCmdBase> allCmds = new List<EntityCmdBase>();

    List<LogicComponentBase> allComponents;

    int lifeTime = -1;

    bool hasDestroy = false;

    public int EntityKey { get; private set; }
    public bool IsDestroy { get; private set; }

    LogicTransform _logicTran;

    public LogicTransform LogicTran
    {
        get
        {
            if (_logicTran == null)
            {
                _logicTran = GetComponent<LogicTransform>();
            }
            return _logicTran;
        }
    }



    public LogicEntity(GameObject go, int key = 0)
    {
        this.EntityKey = AutoKey++;
        this.IsDestroy = false;
        this._sceneGo = go;
        allComponents = new List<LogicComponentBase>();
    }

    public LogicEntity(string resPath, int key = 0)
        : this((GameObject)null)
    {
        //todo: 异步加载资源
    }

    //客户端实体自增Key从100开始,前面的用于服务器开始游戏时指定
    public void OnGameStart()
    {
        AutoKey = 100;
    }

    public void SetLifeTime(int value)
    {
        this.lifeTime = value;
    }

    public void Tick(int ms)
    {
        for (int i = 0; i < allComponents.Count; i++)
        {
            allComponents[i].LogicTick(ms);
        }
        if (lifeTime > 0)
        {
            lifeTime = Mathf.Min(lifeTime - ms, 0);
        }
        if (lifeTime == 0)
        {

        }
    }

    public void Destroy()
    {
        if (this.hasDestroy)
        {
            return;
        }
        this.hasDestroy = true;
        EventManager.Instance.Raise(new EntityDestroyEventArgs(this.EntityKey));
    }

    public T AddComponent<T>(params object[] args) where T : LogicComponentBase
    {
        for (int i = 0; i < allComponents.Count; i++)
        {
            if (allComponents[i] is T)
            {
                Debug.LogError("One type of component can only add onece");
                return (T)allComponents[i];
            }
        }

        Type t = typeof(T);
        Type[] tps = new Type[args.Length + 1];
        tps[0] = typeof(LogicEntity);
        object[] realArgs = new object[args.Length + 1];
        realArgs[0] = this;
        for (int i = 0; i < args.Length; i++)
        {
            tps[i + 1] = args[i].GetType();
            realArgs[i + 1] = args[i];
        }
        T result = (T)t.GetConstructor(tps).Invoke(realArgs);

        allComponents.Add(result);
        if (this._sceneGo)
        {
            result.OnGameObjectLoaded(this._sceneGo);
        }
        return result;
    }

    public T GetComponent<T>() where T : LogicComponentBase
    {
        for (int i = 0; i < allComponents.Count; i++)
        {
            if (allComponents[i] is T)
            {
                return (T)allComponents[i];
            }
        }
        return null;
    }

    public Transform GetTransform()
    {
        return this._sceneGo == null ? null : this._sceneGo.transform;
    }


    void OnLoadEnd(GameObject go)
    {
        this._sceneGo = go;
        for (int i = 0; i < allComponents.Count; i++)
        {
            allComponents[i].OnGameObjectLoaded(go);
        }
    }

}

public class EntityCmdBase
{

}


public class SFrameCtrl
{
    /// <summary>
    /// 实体id
    /// </summary>
    public byte Id;
    public CtrlPair[] Ctrls;
}

public class CtrlPair
{
    /// <summary>
    /// 操控类型
    /// 1.移动
    /// 2.点击技能
    /// </summary>
    public byte CtrlType;
    /// <summary>
    /// 操控类型参数
    /// 1.移动方向0-359度
    /// 2.点击的技能索引
    /// </summary>
    public int CtrlArgs;
}
