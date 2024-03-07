using UnityEngine;

public static class EventSystem
{
    public delegate void DelegateOnFail();
    public static event DelegateOnFail OnFail;

    public static void RaiseOnFail()
    {
        OnFail?.Invoke();
    }
    
    public delegate void DelegateOnKill(Factions killedFaction);
    public static event DelegateOnKill OnKill;

    public static void RaiseOnKill(Factions killedFaction)
    {
        OnKill?.Invoke(killedFaction);
    }
    
    public delegate void DelegateOnFollow(Collider collision);
    public static event DelegateOnFollow OnFollow;

    public static void RaiseOnFollow(Collider collision)
    {
        OnFollow?.Invoke(collision);
    }
    
    public delegate void DelegateOnGather(Collider collision);
    public static event DelegateOnGather OnGather;

    public static void RaiseOnGather(Collider collision)
    {
        OnGather?.Invoke(collision);
    }

    public delegate void DelegateOnBuy(ScriptableObject BoughtObject, int Amount);
    public static event DelegateOnBuy OnBuy;

    public static void RaisOnBuy(ScriptableObject BoughtObject, int Amount)
    {
        OnBuy?.Invoke(BoughtObject, Amount);
    }

    public delegate void DelegateOnArrival();
    public static event DelegateOnArrival OnArrival;

    public static void RaisOnArrival()
    {
        OnArrival?.Invoke();
    }

    public delegate void DelegateOnTrigger();
    public static event DelegateOnTrigger OnTrigger;

    public static void RaisOnTrigger()
    {
        OnTrigger?.Invoke();
    }
}
