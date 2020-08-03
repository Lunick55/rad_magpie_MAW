﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoconutManager : BaseSingleton<CoconutManager>
{
    public List<CoconutPetBehavior> coconuts;

    public List<Transform> hidingSpots;

    void Init()
    {
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
}
