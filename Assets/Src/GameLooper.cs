using UnityEngine;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class GameLooper : MonoBehaviour
{

    public const int LogicFrameDelta = 10;

    const int controlframeStep = 2;

    public const int ControlFrameDelta = LogicFrameDelta * controlframeStep;

    public static GameLooper Instance;

    public Transform Target;

    public float Speed = 4; //单位 米每秒
    public int CacheCmdCount = 1;

    public bool TestMode = false;


    bool waitCacheFull;

    LogicGameObject mt;


    Queue<Cmd> cacheCmds = new Queue<Cmd>(8);

    int logicTime = 0;
    int controlFrameRemain = 0;

    float lastTickRemain = 0;

    float logicTickRemain;
    float controlTickRemain;

    Cmd lastCmd;

    Cmd sendCmd;

    // Use this for initialization
    void Start()
    {
        Instance = this;
        controlFrameRemain = controlframeStep;
        mt = new LogicGameObject(Target.gameObject, new SafeV3());
        TestServer.Instance.OpenServer();
        lastCmd = new Cmd();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            TestServer.Instance.AddCmd(new Cmd() { Dir = 1 });
            if (TestMode)
            {
                Target.position += Speed * Time.deltaTime * 0.001f * Vector3.forward;
            }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            TestServer.Instance.AddCmd(new Cmd() { Dir = 2 });
            if (TestMode)
            {
                Target.position += Speed * Time.deltaTime * 0.001f * Vector3.back;
            }
        }

        if (TestMode)
        {
            cacheCmds.Clear();
            return;
        }


        if (waitCacheFull)
        {
            lastTickRemain = 0;
            return;
        }

        float delta = Time.deltaTime * 1000 + lastTickRemain;
        while (delta > LogicFrameDelta)
        {
            delta -= LogicFrameDelta;
            LogicTick();
        }
        lastTickRemain = delta;

    }

    void OnDestroy()
    {
        TestServer.Instance.CloseServer();
    }

    public void ReceiveCmd(Cmd cmd)
    {
        //if (cmd.Dir != 0)
        //{
        //    Debug.Log("收到   " + cmd.Dir);
        //}
        cacheCmds.Enqueue(cmd);
        if (cacheCmds.Count > CacheCmdCount)
        {
            if (waitCacheFull)
            {
                waitCacheFull = false;
                Debug.Log("放满cache   ");
            }
        }
    }

    void LogicTick()
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


class TestServer
{
    public static readonly TestServer Instance = new TestServer();

    bool _isOpening;

    Stopwatch _watch;

    Stack<Cmd> _receiveCmds;

    uint cmdId = 0;

    Cmd _emptyCmd;

    Thread _thd;

    TestServer()
    {

    }

    public void OpenServer()
    {
        _isOpening = true;

        _watch = new Stopwatch();
        _receiveCmds = new Stack<Cmd>(8);
        _emptyCmd = new Cmd();
        _thd = new Thread(ServerLoop);
        _thd.IsBackground = true;
        _thd.Start();
    }

    public void AddCmd(Cmd cmd)
    {
        lock (_receiveCmds)
        {
            _receiveCmds.Push(cmd);
        }
    }

    public void CloseServer()
    {
        this._isOpening = false;
    }

    void ServerLoop()
    {
        _watch.Start();
        while (this._isOpening)
        {
            _watch.Reset();
            Cmd cmd;
            lock (_receiveCmds)
            {
                cmd = _receiveCmds.Count > 0 ? _receiveCmds.Pop() : new Cmd();
                cmd.Id = ++cmdId;
                _receiveCmds.Clear();
            }
            GameLooper.Instance.ReceiveCmd(cmd);
            Thread.Sleep(GameLooper.ControlFrameDelta - (int)_watch.ElapsedMilliseconds);
        }
    }



}




public class Cmd
{
    public uint Id;
    public byte Dir; //0:无 1:前 2:后
}
