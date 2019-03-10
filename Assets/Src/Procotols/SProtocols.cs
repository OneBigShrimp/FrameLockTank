using MyNetManager;
using System;
using System.Collections.Generic;

class AttrInfo
{
    public int AttrType;
    public float AttrValue;
}

class EntityInitInfo : ISerObj
{
    public int EntityKey;
    public int AttrTemplateId;
    public string EntityName;
}

class SGameStart : IProtocol
{
    public int RandomSeed;
    public EntityInitInfo YourInfo;
    public EntityInitInfo[] OtherInfos;

    public void Process(ILinker linker, object args)
    {
        throw new NotImplementedException();
    }
}

