using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventTag
{
    NONE = -1,
    DAMAGE
}

/// <summary>
/// An abstract class to hold data that will be passed in to a function 
/// </summary>
public abstract class Event
{
    public virtual EventTag tag { get { return EventTag.NONE; } }
}

public class ExampleEvent : Event
{
    public ExampleEvent(string newName, int newAge, float newHeight) 
    { 
        name = newName; 
        age = newAge; 
        height = newHeight; 
    }

    public override EventTag tag { get { return EventTag.NONE; } }

    public string name { get; }
    public int age { get; }
    public float height { get; }
}

public class DamageEvent : Event
{
    public DamageEvent(int newDamage)
	{
        damage = newDamage;
	}

    public override EventTag tag { get { return EventTag.DAMAGE; } }

    public int damage { get; }
}