using UnityEngine;
using System.Collections;
using MyNetManager;

public class CPlayerControl : IProtocol
{
    public int PlayerId;

    public byte IsControl;

    public Pos ClickPos = new Pos();


    public void Clear()
    {
        this.IsControl = 0;
    }


    public void Process(Linker linker)
    {
        throw new System.NotImplementedException();
    }
}

public class SPlayerControl : IProtocol
{
    public int PlayerId;

    public byte IsControl;

    public Pos ClickPos = new Pos();


    public void Clear()
    {
        this.IsControl = 0;
    }


    public void Process(Linker linker)
    {
        GameMain.Instance.ReceiveServerResult(this);
    }
}



public class Pos : ISerObj
{
    public float x;
    public float y;
    public float z;

    public Pos()
    {

    }

    public void Parse(Vector3 vPos)
    {
        this.x = vPos.x;
        this.y = vPos.y;
        this.z = vPos.z;
    }

    public Vector3 ToVec()
    {
        return new Vector3(x, y, z);
    }
}