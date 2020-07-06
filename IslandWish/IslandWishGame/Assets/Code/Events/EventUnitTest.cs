using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventUnitTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.AddUnityListener(TestFunc1, EventTag.FIRST);
        EventManager.instance.AddUnityListener(TestFunc2, EventTag.SECOND);
        EventManager.instance.AddUnityListener(TestFunc3, EventTag.THIRD);

        TestEvent1 ev1 = new TestEvent1();
        TestEvent2 ev2 = new TestEvent2();
        TestEvent3 ev3 = new TestEvent3();
        EventManager.instance.FireUnityEvent(ev1);
        EventManager.instance.FireUnityEvent(ev2);
        EventManager.instance.FireUnityEvent(ev3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TestFunc1(Event ev)
	{
        TestEvent1 nEvent = (TestEvent1)ev;

        Debug.Log(nEvent.num);
	}

    public void TestFunc2(Event ev)
	{
        TestEvent2 nEvent = (TestEvent2)ev;

        Debug.Log(nEvent.num + 1);
    }

    public void TestFunc3(Event ev)
	{
        TestEvent3 nEvent = (TestEvent3)ev;

        Debug.Log(nEvent.num + 2);
    }

    public void TestFunc4(Event ev)
	{
        Debug.Log("4");
    }
}
