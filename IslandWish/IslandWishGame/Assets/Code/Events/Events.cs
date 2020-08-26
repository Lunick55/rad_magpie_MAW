using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventTag
{
    NONE = -1,
    DAMAGE,
    HEAL,
    FAILSTATE,
    CHECKPOINT,
    BEACH_LOG
}

/// <summary>
/// An abstract class to hold data that will be passed in to a function 
/// </summary>
public abstract class Event
{
    public virtual EventTag tag { get { return EventTag.NONE; } }
}

public class BeachEvent : Event
{
    public override EventTag tag { get { return EventTag.BEACH_LOG; } }
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

//public class DamageEvent : Event
//{
//    public DamageEvent(int newDamage, Vector3 newPosition)
//	{
//        damage = newDamage;
//        position = newPosition;
//	}

//    public override EventTag tag { get { return EventTag.DAMAGE; } }

//    public int damage { get; }
//    public Vector3 position { get; }
//}

//public class HealEvent : Event
//{
//    public HealEvent(int newHeal, Vector3 newPosition)
//    {
//        heal = newHeal;
//        position = newPosition;
//    }

//    public override EventTag tag { get { return EventTag.HEAL; } }

//    public int heal { get; }
//    public Vector3 position { get; }
//}

public class FailstateEvent : Event
{
    public override EventTag tag { get { return EventTag.FAILSTATE;  } }
}