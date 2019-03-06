using UnityEngine;

class BulletComponent : LogicComponentBase
{
    int _releaserKey;
    SafeFloat _checkSqrRange;
    public BulletComponent(LogicEntity entity, LogicEntity releaserEntity) : base(entity)
    {
        this._releaserKey = releaserEntity.EntityKey;
        //hostEntity.LogicTran.
        SafeFloat range = releaserEntity.GetComponent<AttrComponent>().GetAttr(Attr.BulletRange);
        this._checkSqrRange = range * range;
    }

    public override void LogicTick(int ms)
    {
        SafeV3 selfPos = this.HostEntity.LogicTran.position;
        foreach (var item in EntityManager.Instance.GetAllEntitys())
        {
            if (item.EntityKey != this._releaserKey)
            {
                if (SafeV3.SqrDistance(item.LogicTran.position, selfPos) < _checkSqrRange)
                {
                    EntityManager.Instance.DestroyEntity(this.HostEntity.EntityKey);
                }
            }
        }
    }

    private bool TryHit(LogicEntity target)
    {
        EffectReceiver receiver = target.GetComponent<EffectReceiver>();
        if (receiver == null)
        {
            return false;
        }
        LogicEntity releaser = EntityManager.Instance.GetEntity(this._releaserKey);
        if (releaser == null)
        {
            return true;
        }
        AttrComponent attr = releaser.GetComponent<AttrComponent>();

        EffectData data = new EffectData();
        data.OpEntityKey = this._releaserKey;
        data.ChagneAttr = new AttrPair[] { new AttrPair(Attr.Hp, attr.GetAttr(Attr.Attack)) };
        receiver.ReciveEffect(data);
        return true;
    }
}
