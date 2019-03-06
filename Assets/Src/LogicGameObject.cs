using UnityEngine;
using System.Collections;



public class LogicGameObject
{
    bool _isLoadedByMe;

    SafeV3 _pos;
    public SafeV3 position
    {
        get
        {
            return _pos;
        }
        set
        {
            _transform.position = value;
            _pos = value;
        }
    }

    private GameObject _go;

    private Transform _transform;

    public LogicGameObject(string resPath, SafeV3 initPos, bool syncLoad = false)
    {
        _isLoadedByMe = true;
        LoadObj(syncLoad);
        position = initPos;
    }

    public LogicGameObject(GameObject target, SafeV3 initPos)
    {
        _go = target;
        _transform = target.transform;
        position = initPos;
        _isLoadedByMe = false;
    }

    public void Move(SafeV3 dir)
    {
        this.position += dir;
    }


    void LoadObj(bool syncLoad)
    {

    }




    void Destroy()
    {
        if (this._isLoadedByMe)
        {
            //回收资源
        }
    }

}
