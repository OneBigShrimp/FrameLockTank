using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EntityManager : ILogicTick
{
    public static readonly EntityManager Instance = new EntityManager();

    Dictionary<int, LogicEntity> allEntitys = new Dictionary<int, LogicEntity>();

    List<int> waitRemoveId = new List<int>();

    bool isTicking;

    public EntityManager()
    {

    }


    public void LogicTick(int ms)
    {
        isTicking = false;
        foreach (var item in allEntitys)
        {
            item.Value.Tick(ms);
        }
        isTicking = true;
        for (int i = 0; i < waitRemoveId.Count; i++)
        {
            allEntitys.Remove(waitRemoveId[i]);
        }
        allEntitys.Clear();
    }

    public LogicEntity CreateEntityByObj(GameObject obj = null, int key = 0)
    {
        LogicEntity entity = new LogicEntity(obj);
        allEntitys.Add(entity.EntityKey, entity);
        return entity;
    }

    public LogicEntity CreateEntityByResPath(string resPath)
    {
        LogicEntity entity = new LogicEntity(resPath);
        allEntitys.Add(entity.EntityKey, entity);
        return entity;
    }

    public IEnumerable<LogicEntity> GetAllEntitys()
    {
        return allEntitys.Values;
    }

    public LogicEntity GetEntity(int entityId)
    {
        if (waitRemoveId.Contains(entityId))
        {
            return null;
        }
        return allEntitys[entityId];
    }

    public void DestroyEntity(int entityId)
    {
        LogicEntity entity = GetEntity(entityId);
        entity.Destroy();
        if (isTicking)
        {
            waitRemoveId.Add(entityId);
        }
        else
        {
            allEntitys.Remove(entityId);
        }
    }

    public LogicEntity CreateBullet(LogicEntity releaserEntity)
    {
        AttrComponent attr = releaserEntity.GetComponent<AttrComponent>();
        LogicEntity result = CreateEntityByResPath("");
        result.SetLifeTime(attr.GetAttr(Attr.BulletLife));
        result.AddComponent<BulletComponent>(releaserEntity);
        LogicMove moveComp = result.AddComponent<LogicMove>();
        moveComp.AddMove(releaserEntity.LogicTran.forward, attr.GetAttr(Attr.BulletSpeed), attr.GetAttr(Attr.BulletLife), null);
        return result;
    }

}
