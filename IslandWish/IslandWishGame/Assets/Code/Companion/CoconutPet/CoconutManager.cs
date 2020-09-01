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

    public CoconutData(string newName, GameObject newAccessory)
	{
        name = newName;
        accessory = newAccessory;
	}

    public string name;
    public bool isSaved = false;
    public GameObject accessory;
}

[System.Serializable]
//public class CoconutSaveData
//{
//    public CoconutSaveData(List<CoconutData> coconuts)
//    {
//        coconutsSaved = new bool[coconuts.Count];

//        for (int i = 0; i < coconuts.Count; i++)
//        {
//            coconutsSaved[i] = coconuts[i].isSaved;
//        }
//    }

//    public bool[] coconutsSaved;
//}
public class CoconutSaveData
{
	public CoconutSaveData(List<CoconutData> coconuts)
	{
		name = new string[coconuts.Count];
		isSaved = new bool[coconuts.Count];
        accessoryID = new int[coconuts.Count];

		for (int i = 0; i < coconuts.Count; i++)
		{
			name[i] = coconuts[i].name;
			isSaved[i] = coconuts[i].isSaved;
            accessoryID[i] = CoconutManager.Instance.cocoAttach.GetIDFromAccessory(coconuts[i].accessory);
		}
	}

    public string[] name;
    public bool[] isSaved;
    public int[] accessoryID;
}

public class CoconutManager : BaseSingleton<CoconutManager>
{
    public List<CoconutPetBehavior> coconuts;

    public List<CoconutPetBehavior> coconutsFreed;

    public List<Transform> hidingSpots;

    public CoconutAttachments cocoAttach;

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
            SceneLoader.Instance.AddSavedCoconuts(coconutsFreed);
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
            GameManager.Instance.player.hud.GainCoconut();
            GameManager.Instance.player.hud.UpdateCoconut(coconutsFreed.Count);
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
