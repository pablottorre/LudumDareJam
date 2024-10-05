using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager
{

    public delegate void EventReceiver(params object[] ParameterContainer);
    private static Dictionary<string, EventReceiver> _Events = new Dictionary<string, EventReceiver>();

    public static void SubscribeToEvent(string eventType, EventReceiver Listener)
    {
        if (_Events.ContainsKey(eventType))
        {
            if (_Events.ContainsValue(Listener))
                return;
        }
        if (!_Events.ContainsKey(eventType))
            _Events.Add(eventType, null);
        _Events[eventType] += Listener;
    }

    public static void UnsuscribeToEvent(string eventType, EventReceiver Listener)
    {
        if (_Events.ContainsKey(eventType)) 
            _Events[eventType] -= Listener;
    }

    public static void TriggerEvent(string eventType, params object[] parametersWrapper)
    {
        if (_Events.ContainsKey(eventType))
            _Events[eventType](parametersWrapper);
    }

    public static void TriggerEvent(string eventType)
    {
        TriggerEvent(eventType, null);
    }

    public static void CleanSubscriptions()
    {
        _Events.Clear();
    }
}
