using System;
using UnityEngine;


class GameMain
{
    public const int LogicFrameDelta = 10;


    float _lastTickRemain;


    void Update()
    {
        float delta = Time.deltaTime * 1000 + _lastTickRemain;
        while (delta > LogicFrameDelta)
        {
            delta -= LogicFrameDelta;
            LogicTick();
        }
        _lastTickRemain = delta;
    }

    private void LogicTick()
    {

        if (cacheCmds.Count == 0)
        {
            Debug.Log("cache   空了");
            waitCacheFull = true;
        }
        if (waitCacheFull)
        {
            return;
        }
        if (--controlFrameRemain == 0)
        {
            lastCmd = cacheCmds.Dequeue();
            controlFrameRemain = controlframeStep;
        }
        if (lastCmd.Dir == 1)
        {
            mt.position += new SafeV3(0, 0, Speed * LogicFrameDelta);
        }
        else if (lastCmd.Dir == 2)
        {
            mt.position += new SafeV3(0, 0, -Speed * LogicFrameDelta);
        }
        else
        {
            //Debug.Log("不移动");
        }
        logicTime += LogicFrameDelta;
    }
}
