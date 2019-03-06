//using System;
//using System.Collections.Generic;

//class SkillInst
//{
//    private int _key;
//    public int Key
//    {
//        get
//        {
//            return _key;
//        }
//    }
//    private List<int> _allBullets = new List<int>();

//    public SkillInst()
//    {
//        this._key = AutoKey.Next;
//        this._cfg = cfg;
//        this._bulletCfg = ConfigManager.Intance.GetConfig<BulletCfg>(cfg.bulletId);
//    }

//    public void CreateBullet(LogicEntity hostEntity)
//    {
//        EntityManager.Instance.CreateEntityByResPath(this._bulletCfg.resPath);
//    }

//    public bool CanDestroy()
//    {
//        return _allBullets.Count == 0;
//    }
//}

//class SkillComponent : LogicComponentBase
//{
//    Dictionary<int, SkillInst> _aliveSkillInsts = new Dictionary<int, SkillInst>();

//    public SkillComponent(LogicEntity entity) : base(entity)
//    {
//    }

//    public int CreateNewSkillInst(SkillCfg cfg)
//    {
//        SkillInst inst = new SkillInst(cfg);
//        _aliveSkillInsts.Add(inst.Key, inst);
//        return inst.Key;
//    }

//    public void CreateBullet(int skillInstKey)
//    {
//        SkillInst inst = _aliveSkillInsts[skillInstKey];
//        inst.CreateBullet(this.hostEntity);
//    }

//    public bool TryDestroySkillInst(int instKey)
//    {
//        SkillInst inst;
//        if (_aliveSkillInsts.TryGetValue(instKey, out inst))
//        {
//            if (inst.CanDestroy())
//            {
//                _aliveSkillInsts.Remove(instKey);
//                return true;
//            }
//        }
//        return false;
//    }

//}

