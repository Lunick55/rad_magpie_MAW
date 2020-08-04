using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : BaseSingleton<GameManager>
{
    public Player player;
    public Transform playerTrans;

    // Start is called before the first frame update
    void Start()
    {
        if(!player)
		{
            Debug.LogError("Fill out the Player field");
		}
        if(!playerTrans)
		{
            Debug.LogError("Fill out the Player Trans field");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
