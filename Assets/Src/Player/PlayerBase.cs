using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase
{
    public readonly int Id;
    public readonly string Name;
    public readonly int AttrTemplateId;
    public PlayerBase(PlayerInitInfo initInfo)
    {
        this.Id = initInfo.Id;
        this.Name = initInfo.Name;
        this.AttrTemplateId = initInfo.AttrTemplateId;
    }

    public void ReceiveJoyCmd()
    {

    }


}
