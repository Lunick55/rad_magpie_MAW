using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CoconutData
{
    public CoconutData(string newName) 
    {
        name = newName; 
    }

    public CoconutData(string newName, int newAccessory, int newBody)
	{
        name = newName;
        accessoryIndex = newAccessory;
        bodyIndex = newBody;
	}

    public string name;
    public int accessoryIndex;
    public int bodyIndex;
}

[System.Serializable]
public class CoconutSaveData
{
	public CoconutSaveData(List<CoconutData> coconuts)
	{
		name = new string[coconuts.Count];
        bodyID = new int[coconuts.Count];
        accessoryID = new int[coconuts.Count];

		for (int i = 0; i < coconuts.Count; i++)
		{
			name[i] = coconuts[i].name;
            bodyID[i] = coconuts[i].bodyIndex;
            accessoryID[i] = coconuts[i].accessoryIndex;
		}
	}

    public string[] name;
    public int[] bodyID;
    public int[] accessoryID;
}

public class CoconutManager : BaseSingleton<CoconutManager>
{
    public List<CoconutPetBehavior> coconuts;

    public List<CoconutPetBehavior> coconutsFreed;

    public List<Transform> hidingSpots;

    void Init()
    {
        //EventManager.instance.AddListener(ScatterCoconuts, EventTag.FAILSTATE);

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
        if (Input.GetKeyDown(KeyCode.C))
		{
            //SceneLoader.Instance.AddSavedCoconuts(coconutsFreed);
        }
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
            GameManager.Instance.GetPlayer(0).hud.GainCoconut();
            GameManager.Instance.GetPlayer(0).hud.UpdateCoconut(coconutsFreed.Count);
            AudioManager.Instance.Play("CocoRescue");

            if(coconutsFreed.Count >= coconuts.Count)
			{
                Debug.Log("Level Complete!");
                GameManager.Instance.CoconutCelebration();
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
            GameManager.Instance.GetPlayer(0).hud.LoseCoconut();
        }

        coconutsFreed.Clear();
        coconutsFreed.Capacity = 0;
        GameManager.Instance.GetPlayer(0).hud.UpdateCoconut(coconutsFreed.Count);
    }

}
