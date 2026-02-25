using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum GameEventType
{
    Attacked,Defeated
}
public class GameEventBus : MonoBehaviour
{
   private static readonly IDictionary<GameEventType,UnityEvent>
        Events = new Dictionary<GameEventType, UnityEvent>();

    public static void Subscribe(GameEventType eventType,UnityAction Listender)
    {
        UnityEvent thisevent;
        if(Events.TryGetValue(eventType, out thisevent))
        {
            thisevent.AddListener(Listender);
        }
        else
        {
            thisevent = new UnityEvent();
            thisevent.AddListener(Listender);
            Events.Add(eventType, thisevent);
        }
    }
    public static void Unsubscribe(GameEventType eventType, UnityAction Listender)
    {
        UnityEvent thisevent;
        if (Events.TryGetValue(eventType, out thisevent))
        {
            thisevent.RemoveListener(Listender);
        }

    }
    public static void Publish(GameEventType eventType)
    {
        UnityEvent thisevent;
        if (Events.TryGetValue(eventType, out thisevent))
        {
            thisevent.Invoke();
        }

    }

}
