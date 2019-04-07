using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase
{
    public readonly int Id;
    public readonly string Name;
    public readonly int AttrTemplateId;

    int _entityKey;

    public LogicEntity Entity
    {
        get
        {
            return EntityManager.Instance.GetEntity(_entityKey);
        }
    }

    public PlayerBase(PlayerInitInfo initInfo)
    {
        this.Id = initInfo.Id;
        this.Name = initInfo.Name;
        this.AttrTemplateId = initInfo.AttrTemplateId;
    }

    public void CreateEntity()
    {
        LogicEntity oldEntity = EntityManager.Instance.GetEntity(_entityKey);
        if (oldEntity != null)
        {
            Debug.LogError("oldEntity is exist!");
            return;
        }

        //根据模板id查询配置
        LogicEntity entity = EntityManager.Instance.CreateEntityByResPath("");
        _entityKey = entity.EntityKey;

    }

    public void RevCmd(ICommand cmd)
    {
        cmd.Run(this);
    }

}
