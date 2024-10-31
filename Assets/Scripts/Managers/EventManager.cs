using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;

    public Dictionary<string,IEventInfo> eventDic=new Dictionary<string,IEventInfo>();
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

    }


    void Start()
    {
        
    }


    void Update()
    {
        
    }

    //无参添加
    public void AddEventListener(string eventName, UnityAction action)
    {
        if (eventDic.ContainsKey(eventName))
        {
            (eventDic[eventName] as EventInfo).actions+= action;
        }
        else
        {
            eventDic.Add(eventName, new EventInfo(action));
        }
    }

    //有参添加
    public void AddEventListener<T>(string eventName, UnityAction<T> action)
    {
        if (eventDic.ContainsKey(eventName))
        {
            (eventDic[eventName] as EventInfo<T>).actions += action;
        }
        else
        {
            eventDic.Add(eventName, new EventInfo<T>(action));
        }
    }

    public void RemoveEventListener(string eventName,UnityAction action)
    {
        if(eventDic.ContainsKey(eventName))
        {
            (eventDic[eventName] as EventInfo).actions -= action;
        }
    }

    public void RemoveEventListener<T>(string eventName, UnityAction<T> action)
    {
        if (eventDic.ContainsKey(eventName))
        {
            (eventDic[eventName] as EventInfo<T>).actions -= action;
        }
    }

    //无参触发
    public void EventTrigger(string eventName)
    {
        if (eventDic.ContainsKey(eventName) && (eventDic[eventName] as EventInfo).actions != null)
        {
            (eventDic[eventName] as EventInfo).actions.Invoke();
        }
    }

    //有参触发
    public void EventTrigger<T>(string eventName,T info)
    {
        if (eventDic.ContainsKey(eventName) && (eventDic[eventName] as EventInfo<T>).actions != null)
        {
            (eventDic[eventName] as EventInfo<T>).actions.Invoke(info);
        }
    }



}

public interface IEventInfo
{ }

//无参事件响应
public class EventInfo:IEventInfo
{
    public UnityAction actions;

    public EventInfo(UnityAction action)
    {
        actions += action;
    }
}

public class EventInfo<T>:IEventInfo
{
    public UnityAction<T> actions;

    public EventInfo(UnityAction<T> action)
    {
        actions = action;
    }
}
