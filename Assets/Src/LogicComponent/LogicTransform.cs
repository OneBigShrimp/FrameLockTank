using UnityEngine;
using System.Collections.Generic;
using System;
using System.Reflection;

public class LogicTransform : LogicComponentBase
{
    Transform _tran;

    Dictionary<string, SafeV3> vecPropCache;

    public SafeV3 position
    {
        get
        {
            return GetVecProp("position");
        }
        set
        {
            SetVecProp("position", value);
        }
    }

    public SafeV3 forward
    {
        get
        {
            return GetVecProp("forward");
        }
        set
        {
            SetVecProp("forward", value);
        }
    }

    public SafeV3 eulerAngles
    {
        get
        {
            return GetVecProp("eulerAngles");
        }
        set
        {
            SetVecProp("eulerAngles", value);
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
        foreach (var item in vecPropCache)
        {
            SetVecProp(item.Key, item.Value);
        }
    }

    public SafeV3 GetVecProp(string propName)
    {
        SafeV3 result;
        if (vecPropCache.TryGetValue(propName, out result))
        {
            return result;
        }
        else
        {
            Debug.LogError("GetVecProp error , propName : " + propName);
            return new SafeV3();
        }
    }

    public void SetVecProp(string propName, SafeV3 value)
    {
        vecPropCache[propName] = value;
        if (_tran)
        {
            Type tranTp = typeof(Transform);
            PropertyInfo propInfo = tranTp.GetProperty(propName, BindingFlags.Public | BindingFlags.Instance);
            if (propInfo != null)
            {
                propInfo.SetValue(_tran, (Vector3)value, null);
            }
            else
            {
                Debug.LogError("SetVecProp error , propName : " + propName);
            }
        }
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