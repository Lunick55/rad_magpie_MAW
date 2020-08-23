using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoconutData
{
    public CoconutData(string newName) 
    {
        name = newName; 
    }

    public CoconutData(string newName, GameObject newAccessory)
	{
        name = newName;
        accessory = newAccessory;
	}

    public string name;
    public GameObject accessory;
}

public class CoconutManager : BaseSingleton<CoconutManager>
{
    public List<CoconutPetBehavior> coconuts;

    public List<CoconutPetBehavior> coconutsFreed;

    public List<Transform> hidingSpots;


    void Init()
    {
        EventManager.instance.AddListener(ScatterCoconuts, EventTag.FAILSTATE);

        coconuts = new List<CoconutPetBehavior>();
        if (hidingSpots == null)
        {
            hidingSpots = new List<Transform>();
        }
    }

    public void AddCoconut(CoconutPetBehavior newCoconut)
	{
        if (!coconuts.Contains(newCoconut))
        {
            coconuts.Add(newCoconut);
            Debug.Log(newCoconut.name);
        }
	}

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Transform GetClosestHidingSpot(Transform point)
	{
        Transform hidingSpot;

        if(hidingSpots.Count > 0)
		{
            hidingSpot = hidingSpots[0];

            foreach (Transform possibleHidingSpot in hidingSpots)
            {
                if((point.position - possibleHidingSpot.position).sqrMagnitude < (point.position - hidingSpot.position).sqrMagnitude)
				{
                    hidingSpot = possibleHidingSpot;
				}
            }
		}
        else
		{
            hidingSpot = point;
        }


        return hidingSpot;
	}

    public void CoconutFreed(CoconutPetBehavior newRecruit)
	{
        if (!coconutsFreed.Contains(newRecruit))
        {
            coconutsFreed.Add(newRecruit);
            GameManager.Instance.player.hud.GainCoconut();
            GameManager.Instance.player.hud.UpdateCoconut(coconutsFreed.Count);

            if(coconutsFreed.Count >= coconuts.Count)
			{
                //go to next level
                SceneLoader.Instance.AddSavedCoconuts(coconutsFreed);
                SceneLoader.Instance.LoadScene("Boat Scene");
			}
        }
	}

    public void ScatterCoconuts(Event failstateEvent)
	{
        foreach(CoconutPetBehavior coconut in coconutsFreed)
		{
            coconut.hide = true;
            int randNum = Random.Range(0, hidingSpots.Count);
            coconut.transform.position = hidingSpots[randNum].position;
            GameManager.Instance.player.hud.LoseCoconut();
        }

        coconutsFreed.Clear();
        coconutsFreed.Capacity = 0;
        GameManager.Instance.player.hud.UpdateCoconut(coconutsFreed.Count);
    }
}
