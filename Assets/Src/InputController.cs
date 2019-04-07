using System;
using System.Collections.Generic;
using UnityEngine;

public class InputController
{
    public static readonly InputController Instance = new InputController();

    public int CtrlEntityKey { get; set; }

    public InputController()
    {
        CtrlEntityKey = 0;
    }

    public void AddJoyInput(SafeV3 joyDir)
    {
        if (CtrlEntityKey == 0)
        {
            return;
        }
        LogicEntity entity = EntityManager.Instance.GetEntity(CtrlEntityKey);
        if (entity != null)
        {
            //entity.LogicTran.eulerAngles
        }
        else
        {
            Debug.LogError("AddJoyInput() no find entity");
        }

    }

    public void AddSkillInput()
    {
        if (CtrlEntityKey == 0)
        {
            return;
        }
    }
}
