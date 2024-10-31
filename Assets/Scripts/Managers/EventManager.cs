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

    //�޲����
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

    //�в����
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

    //�޲δ���
    public void EventTrigger(string eventName)
    {
        if (eventDic.ContainsKey(eventName) && (eventDic[eventName] as EventInfo).actions != null)
        {
            (eventDic[eventName] as EventInfo).actions.Invoke();
        }
    }

    //�вδ���
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

//�޲��¼���Ӧ
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
