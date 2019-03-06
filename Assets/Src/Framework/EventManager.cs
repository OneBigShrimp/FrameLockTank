using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 事件管理类,这个类可以作为不同模块事件管理类的基类,
/// 子类可以不做任何额外处理
/// </summary>
public class EventManager
{
    public delegate void EventDelegate<T>(T t) where T : EventArgs;

    public static readonly EventManager Instance = new EventManager();

    Dictionary<Type, Delegate> allDelegate = new Dictionary<Type, Delegate>();
    public void AddListener<T>(EventDelegate<T> listener) where T : EventArgs
    {
        if (allDelegate.ContainsKey(typeof(T)))
        {
            allDelegate[typeof(T)] = Delegate.Combine(allDelegate[typeof(T)], listener);
        }
        else
        {
            allDelegate[typeof(T)] = listener;
        }
    }
    public void RemoveListener<T>(EventDelegate<T> listener) where T : EventArgs
    {
        if (allDelegate.ContainsKey(typeof(T)))
        {
            allDelegate[typeof(T)] = Delegate.Remove(allDelegate[typeof(T)], listener);
            if (allDelegate[typeof(T)] == null)
            {
                allDelegate.Remove(typeof(T));
            }
        }
        else
        {
            Debug.LogError("还没有添加过" + typeof(T) + "类型的监听或者监听已经移除");
        }
    }
    /// <summary>
    /// 此方法慎用,当你确定该种类型的监听只有你自己注册过,
    /// 并且触发了一次以后就打算移除该类型监听时可以使用
    /// </summary>
    /// <param name="t"></param>
    public void RaiseOnce<T>(T t) where T : EventArgs
    {
        if (allDelegate.ContainsKey(typeof(T)))
        {
            (allDelegate[typeof(T)] as EventDelegate<T>).Invoke(t);
            allDelegate.Remove(typeof(T));
        }
        else
        {
            Debug.LogError("还没有添加过" + typeof(T) + "类型的监听或者监听已经移除");
        }
    }
    public void Raise<T>(T t) where T : EventArgs
    {
        if (allDelegate.ContainsKey(typeof(T)))
        {
            (allDelegate[typeof(T)] as EventDelegate<T>).Invoke(t);
        }
        else
        {
            Debug.LogError("还没有添加过" + typeof(T) + "类型的监听或者监听已经移除");
        }
    }
    public void ClearAll()
    {
        allDelegate.Clear();
    }
}
public class EventArgs
{
}
