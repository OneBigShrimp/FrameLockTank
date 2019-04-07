using UnityEngine;
using System.Collections.Generic;
using System;
using System.Reflection;

public class LogicTransform : LogicComponentBase
{
    Transform _tran;

    private SafeV3 _position = new SafeV3();
    public SafeV3 position
    {
        get
        {
            return _position;
        }
        set
        {
            if (value == _position)
            {
                return;
            }
            _position = value;
            if (_tran)
            {
                _tran.position = value;
            }
        }
    }




    public SafeV3 forward
    {
        get
        {
            SafeFloat rad = _eulerAngles.y * (SafeFloat)(Mathf.PI / 180);
            return new SafeV3(Mathf.Cos(rad), 0, Mathf.Sin(rad));
        }
    }


    private SafeV3 _eulerAngles = new SafeV3();
    public SafeV3 eulerAngles
    {
        get
        {
            return _eulerAngles;
        }
        set
        {
            if (_eulerAngles == value)
            {
                return;
            }
            _eulerAngles = value;
            if (_tran)
            {
                _tran.eulerAngles = value;
            }
        }
    }


    public LogicTransform(LogicEntity entity, SafeV3 pos)
        : base(entity)
    {
        this.position = pos;
    }

    public override void OnGameObjectLoaded(GameObject go)
    {
        this._tran = go.transform;
        _tran.position = position;
        _tran.eulerAngles = eulerAngles;
    }

}

//class PhysicData
//{
//    public int Layer;
//    public Shape Shape;
//}

//class Shape
//{

//}