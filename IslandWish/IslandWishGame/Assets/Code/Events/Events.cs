using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventTag
{
    NONE = -1,
    FIRST,
    SECOND,
    THIRD
}

/// <summary>
/// An abstract class to hold data that will be passed in to a function 
/// </summary>
public abstract class Event
{
    public virtual EventTag tag { get { return EventTag.NONE; } }
}

public class TestEvent : Event
{
    public TestEvent(string newName, int newAge, float newHeight) 
    { 
        name = newName; 
        age = newAge; 
        height = newHeight; 
    }

    public override EventTag tag { get { return EventTag.FIRST; } }

    public string name { get; }
    public int age { get; }
    public float height { get; }
}

public class TestEvent1 : Event
{
    public int num { get { return 1; } }

    public override EventTag tag { get { return EventTag.FIRST; } }
}
public class TestEvent2 : Event
{
    public int num { get { return 2; } }

    public override EventTag tag { get { return EventTag.SECOND; } }
}
public class TestEvent3 : Event
{
    public int num { get { return 3; } }

    public override EventTag tag { get { return EventTag.THIRD; } }
}

public class TimeEvent : Event 
{
    public TimeEvent(int nHour, int nMin, int nSec)
	{
        hour = nHour;
        min = nMin;
        sec = nSec;
	}

    public override EventTag tag { get { return EventTag.SECOND; } }

    int hour { get; }
    int min { get; }
    int sec { get; }
}