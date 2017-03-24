using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MyNetManager;
using System.IO;

public class GameMain : MonoBehaviour
{
    public static GameMain Instance;

    public Vector3 TestPos;

    float lastTickRemainTime = 0;

    float frameTime = 0.02f;

    int lockStepFrame = 5;

    int stepFrameCount = 6;

    int lastControlUnitFrame;

    CPlayerControl sendPc;

    Vector3 dir;

    Vector3 targetPos;

    int unityFrame = 0;

    int curFrame = 1;


    // Use this for initialization
    void Start()
    {
        Instance = this;
        ClientNetManager.Instance.Regist(typeof(CPlayerControl), 1);
        ClientNetManager.Instance.Regist(typeof(SPlayerControl), 2);
        ClientNetManager.Instance.Connect("127.0.0.1", 60001, 128, 128);
        //ClientNetManager.Instance.SendMsg(new CPlayerControl());

        sendPc = new CPlayerControl();
    }

    public void ReceiveServerResult(SPlayerControl pc)
    {
        if (pc.IsControl > 0)
        {
            targetPos = pc.ClickPos.ToVec();
            dir = (targetPos - transform.position).normalized;
        }
        //else
        //{
        //    dir = Vector3.zero;
        //}
        for (int i = curFrame; i <= stepFrameCount; i++)
        {
            GameUpdate();
        }

        curFrame = 1;
    }



    // Update is called once per frame
    void Update()
    {
        ClientNetManager.Instance.Tick();

        unityFrame++;
        if (CheckWaitServer())
        {//等待服务器回复逻辑帧操作信息
            return;
        }


        float curDelta = Time.deltaTime + lastTickRemainTime;

        while (curDelta > frameTime)
        {
            UpdatePlayerControl();

            CheckSendMsg();

            GameUpdate();

            if (CheckWaitServer())
            {
                curDelta = 0;
                break;
            }
            else
            {
                curDelta -= frameTime;
            }
        }
        lastTickRemainTime = curDelta;
    }

    private void CheckSendMsg()
    {
        if (curFrame == lockStepFrame)
        {
            //发送本次逻辑帧之前收集的操作
            ClientNetManager.Instance.SendMsg(sendPc);
            sendPc.Clear();
        }
    }

    private void UpdatePlayerControl()
    {
        if (this.lastControlUnitFrame != this.unityFrame && Input.GetMouseButtonDown(0))
        {
            //sendPc.ClickPos = Utils.GetFloorPosFromMousePos();
            sendPc.ClickPos.Parse(TestPos);
            sendPc.IsControl = 1;
            this.lastControlUnitFrame = unityFrame;
        }
    }

    bool CheckWaitServer()
    {
        if (curFrame == stepFrameCount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    List<string> allLines = new List<string>();

    void GameUpdate()
    {
        if (dir != Vector3.zero)
        {
            Vector3 curMove = dir * frameTime;
            if (Vector3.SqrMagnitude(targetPos - transform.position) <= Vector3.SqrMagnitude(curMove))
            {
                transform.position = targetPos;
                dir = Vector3.zero;
                File.WriteAllLines("Test.txt",allLines.ToArray());
            }
            else
            {
                transform.position += curMove;
                allLines.Add(transform.position.x + "___" + transform.position.y + "____" + transform.position.z);
            }
        }
        this.curFrame++;
    }


}



public class Control
{
    public int PlayerKey;

    //public Pos ClickPos;

}

//public class Pos : ISerialize
//{
//    public float x;
//    public float y;
//    public float z;
//}