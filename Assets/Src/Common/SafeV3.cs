#define OpenSafeDebug
using UnityEngine;
using System.Collections;
using System;


public struct SafeV3
{
    SafeFloat _x;
    SafeFloat _y;
    SafeFloat _z;

    public float x
    {
        get
        {
            return this._x.Value;
        }
        set
        {
            this._x = value;
        }
    }

    public float y
    {
        get
        {
            return this._y.Value;
        }
        set
        {
            this._y = value;
        }
    }

    public float z
    {
        get
        {
            return this._z.Value;
        }
        set
        {
            this._z = value;
        }
    }

    public SafeFloat sqrMagnitude
    {
        get
        {
            return _sqrMagnitude;
        }
    }

    public SafeFloat magnitude
    {
        get
        {
            return Mathf.Pow(_sqrMagnitude, 0.5f);
        }
    }


    private float _sqrMagnitude
    {
        get
        {
            return this.x * this.x + this.y * this.y + this.z * this.z;
        }
    }

    private float _magnitude
    {
        get
        {
            return Mathf.Pow(_sqrMagnitude, 0.5f);
        }
    }

    public SafeV3(SafeFloat x, SafeFloat y, SafeFloat z)
    {
        this._x = x;
        this._y = y;
        this._z = z;
    }

    public SafeV3(float x, float y, float z)
    {
        this._x = x;
        this._y = y;
        this._z = z;
    }

    public void SetNormalize()
    {
        float len = this._magnitude;
        this.x /= len;
        this.y /= len;
        this.z /= len;
    }

    public SafeV3 Normalize()
    {
        float len = this._magnitude;
        return new SafeV3(this.x / len, this.y / len, this.z / len);
    }

    public override string ToString()
    {
        return string.Format("[{0},{1},{2}]", _x, _y, _z);
    }

    public static SafeFloat Distance(SafeV3 a, SafeV3 b)
    {
        return (a - b).magnitude;
    }

    public static SafeFloat SqrDistance(SafeV3 a, SafeV3 b)
    {
        return (a - b).sqrMagnitude;
    }

    public static implicit operator Vector3(SafeV3 v)
    {
        return new Vector3(v.x, v.y, v.z);
    }

    public static SafeV3 operator +(SafeV3 a, SafeV3 b)
    {
        return new SafeV3() { _x = a._x + b._x, _y = a._y + b._y, _z = a._z + b._z };
    }

    public static SafeV3 operator -(SafeV3 a, SafeV3 b)
    {
        return new SafeV3() { _x = a._x - b._x, _y = a._y - b._y, _z = a._z - b._z };
    }

    public static SafeV3 operator *(SafeFloat f, SafeV3 v3)
    {
        return new SafeV3() { _x = v3._x * f, _y = v3._y * f, _z = v3._z * f };
    }

    public static bool operator ==(SafeV3 a, SafeV3 b)
    {
        return a.x == b.x && a.y == b.y && a.z == b.z;
    }

    public static bool operator !=(SafeV3 a, SafeV3 b)
    {
        return !(a == b);
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

/// <summary>
/// 擦除浮点数一定位数的精度,保证不同cpu计算结果是相同的
/// </summary>
public struct SafeFloat
{
    /// <summary>
    /// 擦除的精度位数
    /// </summary>
    const int eraseBit = 4;

    float _v;

    public float Value
    {
        get
        {
            return _v;
        }
    }


    unsafe public SafeFloat(float v)
    {
        *((int*)&v) = (*((int*)&v) >> eraseBit) << eraseBit;
        this._v = v;
    }

    public override bool Equals(object obj)
    {
        if (obj.GetType() != typeof(SafeFloat))
        {
            return false;
        }
        return ((SafeFloat)obj)._v == this._v;
    }

    public override int GetHashCode()
    {
        return _v.GetHashCode();
    }

    public static implicit operator SafeFloat(float v)
    {
        return new SafeFloat(v);
    }

    public static implicit operator float(SafeFloat v)
    {
        return v.Value;
    }

    public static implicit operator int(SafeFloat v)
    {
        float value = v.Value;
        if (value != (int)value)
        {
            Debug.LogErrorFormat("SafeFloat 转换为 int时发生精度丢失");
        }
        return (int)value;
    }


    public static SafeFloat operator +(SafeFloat a, SafeFloat b)
    {
        return a._v + b._v;
    }

    public static SafeFloat operator -(SafeFloat a, SafeFloat b)
    {
        return a._v - b._v;
    }

    public static SafeFloat operator *(SafeFloat a, SafeFloat b)
    {
        return a._v * b._v;
    }

    public static SafeFloat operator /(SafeFloat a, SafeFloat b)
    {
        return a._v / b._v;
    }

    public static bool operator >(SafeFloat a, SafeFloat b)
    {
        return a._v > b._v;
    }

    public static bool operator <(SafeFloat a, SafeFloat b)
    {
        return a._v < b._v;
    }

    public static bool operator ==(SafeFloat a, SafeFloat b)
    {
        return a._v == b._v;
    }

    public static bool operator !=(SafeFloat a, SafeFloat b)
    {
        return a._v != b._v;
    }
}