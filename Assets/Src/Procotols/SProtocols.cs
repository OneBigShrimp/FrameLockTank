using MyNetManager;
using System;
using System.Collections.Generic;


public class PlayerInitInfo : ISerObj
{
    public int Id;
    public string Name;
    public int AttrTemplateId;
}

public class SGameStart : IProtocol
{
    public int RandomSeed;
    public int YourPlayerId;
    public PlayerInitInfo[] AllInfos;

    public void Process(ILinker linker, object args)
    {
        throw new NotImplementedException();
    }
}

public class SPlayerMove : IProtocol
{
    public int EulerY;


    public void Process(ILinker linker, object args)
    {
        throw new NotImplementedException();
    }
}

