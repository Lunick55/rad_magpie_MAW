using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Abstracted function that will be invoked on triggered event
/// </summary>
[System.Serializable] public class EventFunction : UnityEvent<Event> //T : Event
{

} 

public class EventManager : MonoBehaviour
{
    //Unity Implementation
    /// <summary>
    /// Container of all my Unity Event Functions
    /// </summary>
    private Dictionary<EventTag, EventFunction> eventDictionary; 

    //Singleton implementation
    private static EventManager eventManager;
    public static EventManager instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (!eventManager)
                {
                    Debug.LogError("There needs to be one active EventManager script on a GameObject in your scene.");
                }
                else
                {
                    eventManager.Init();
                }
            }

            return eventManager;
        }
    }

    void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<EventTag, EventFunction>();
        }
    }

    //UnityEvent AddListener
    public void AddUnityListener(UnityAction<Event> nAction, EventTag nEvent)
	{
        if(eventDictionary.ContainsKey(nEvent))
		{
            //create our out parameter
            EventFunction eventFunc = new EventFunction();

            //copy the UnityEvent to our temp variable
            eventDictionary.TryGetValue(nEvent, out eventFunc);

            //TODO: find a way to check if it's already listening. Maybe a second map that looks like, ~ Dictionary unityActionCheck <EventTag, UnityAction<Event>>?
            //Add the listener!
            eventFunc.AddListener(nAction);
		}
        else
		{
            //create the new EventFunction and add a listener right away
            EventFunction eventFunc = new EventFunction();
            eventFunc.AddListener(nAction);

            //throw 'em in the dictionary
            eventDictionary.Add(nEvent, eventFunc);
		}
	}

    /// <summary>
    /// Removes a listener from an existing event
    /// </summary>
    /// <param name="nAction"></param>
    /// <param name="nEvent"></param>
    public void RemoveUnityListener(UnityAction<Event> nAction, EventTag nEvent)
	{
        if (eventDictionary.ContainsKey(nEvent))
        {
            //create our out parameter
            EventFunction eventFunc = new EventFunction();

            //copy the UnityEvent to our temp variable
            eventDictionary.TryGetValue(nEvent, out eventFunc);

            //TODO: find a way to check if it's already listening. Maybe a second map that looks like, ~ Dictionary unityActionCheck <EventTag, UnityAction<Event>>?
            //Remove the listener!
            eventFunc.RemoveListener(nAction);
        }
        else
        {
            Debug.LogWarning(string.Format("Trying to remove listener: {0}, from event: {1} that doesn't exist!", nAction, nEvent));
        }
    }

    /// <summary>
    /// Removes all listeners to a given event and removes the event from the dictionary
    /// </summary>
    /// <param name="nEvent"></param>
    public void ClearEventListeners(EventTag nEvent)
	{
        if (eventDictionary.ContainsKey(nEvent))
        {
            //iterate through dictionary, find key, clear its listeners, then remove it
            EventFunction eventFunc = new EventFunction();
            if (eventDictionary.TryGetValue(nEvent, out eventFunc))
            {
                eventFunc.RemoveAllListeners();
            }
            eventDictionary.Remove(nEvent);
        }
        else
        {
            Debug.LogWarning(string.Format("Trying to clear event: {0} that doesn't exist!", nEvent));
        }
    }

    /// <summary>
    /// Clears all events out, effectivly reseting the eventmanager
    /// </summary>
    public void ClearAllListeners()
	{
        //iterate through each key, and clear everything in the value, then remove all pairs from the dictionary
        foreach (KeyValuePair<EventTag, EventFunction> eventPair in eventDictionary)
		{
            eventPair.Value.RemoveAllListeners();
		}
        eventDictionary.Clear();
	}

    public void FireUnityEvent(Event myEvent)
	{
        EventFunction eventFunc;
        if (eventDictionary.TryGetValue(myEvent.tag, out eventFunc))
        {
            eventFunc.Invoke(myEvent);
        }
    }
}
