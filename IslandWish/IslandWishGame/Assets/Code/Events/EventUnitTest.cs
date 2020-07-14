using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventUnitTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.AddListener(TestFunc1, EventTag.NONE);
        EventManager.instance.AddListener(TestFunc2, EventTag.NONE);
        EventManager.instance.AddListener(TestFunc3, EventTag.NONE);

        //TestEvent1 ev1 = new TestEvent1();
        //TestEvent2 ev2 = new TestEvent2();
        //TestEvent3 ev3 = new TestEvent3();
        //EventManager.instance.FireEvent(ev1);
        //EventManager.instance.FireEvent(ev2);
        //EventManager.instance.FireEvent(ev3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TestFunc1(Event ev)
	{

	}

    public void TestFunc2(Event ev)
	{

    }

    public void TestFunc3(Event ev)
	{

    }

    public void TestFunc4(Event ev)
	{
        Debug.Log("4");
    }
}
