using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : BaseSingleton<GameManager>
{
    public Player player;
    public Transform playerTrans;

    public AudioManager audioManager;

    int numEnemiesAggroed = 0;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public void Init()
	{
        if (!player)
        {
            Debug.LogError("Fill out the Player field");
        }
        if (!playerTrans)
        {
            Debug.LogError("Fill out the Player Trans field");
        }
    }

    public void IncreaseAggro()
	{
        numEnemiesAggroed++;
	}

    public void DecreaseAggro()
	{
        if(numEnemiesAggroed > 0)
		{
            numEnemiesAggroed--;
        }
    }

    public int GetCurrentAggro()
	{
        return numEnemiesAggroed;
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
