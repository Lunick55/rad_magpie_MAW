using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : BaseSingleton<GameManager>
{
    [SerializeField] List<Player> players;
    [HideInInspector] List<Movement> playersMove;
    [HideInInspector] List<Transform> playersTrans;

    [SerializeField] List<EnemyBehavior> enemies;
    int numEnemiesAggroed = 0;

    //do i even like this?
    [SerializeField] MenuManager menuManager;

	// Start is called before the first frame update
	void Awake()
    {
        Init();
    }

    public void Init()
	{
        SceneLoader.Instance.Init();

        if (players.Count <= 0)
        {
            Debug.LogError("Fill out the Player field");
        }
        playersTrans = new List<Transform>(players.Count);
        playersMove = new List<Movement>(players.Count);
        for(int i = 0; i < players.Count; i++)
		{
            playersTrans[i] = players[i].GetComponent<Transform>();
            playersMove[i] = players[i].GetComponent<Movement>();
        }

        LoadGame();
    }

    public void LoadGame()
	{
        if (SceneLoader.Instance.loadData)
        {
            SceneLoader.Instance.loadData = false;

            //TODO: load all the data calls needed here
            players[0].LoadPlayer();
            LoadEnemies();
            LevelManager.Instance.LoadLevel();
            SceneLoader.Instance.LoadCoconuts();
        }
        else
        {
            players[0].currentHealth = players[0].stats.health;
        }
    }

    public void SaveGame()
	{
        SaveSystem.SavePlayer(players[0]);
        SaveSystem.SaveCoconuts(SceneLoader.Instance.GetSavedCoconuts());
        SaveSystem.SaveEnemies(enemies, SceneLoader.Instance.GetCurrentLevelName());
    }

    public Player GetPlayer(int index)
	{
        return players[index];
	}    
    public Movement GetMovement(int index)
	{
        return playersMove[index];
	}    
    public Transform GetPlayerTrans(int index)
	{
        return playersTrans[index];
	}

    public int GetClosestPlayer(Vector3 point)
	{
        int closestIndex = 0;
        float closestDistance = (point - playersTrans[0].position).sqrMagnitude;
        for (int i = 0; i < playersTrans.Count; i++)
		{
            if ((point - playersTrans[0].position).sqrMagnitude < closestDistance)
			{
                closestIndex = i;
			}
        }

        return closestIndex;
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

    public void AddEnemy(EnemyBehavior enemy)
	{
        enemies.Add(enemy);
	}

    private void LoadEnemies()
	{
        EnemyData data = SaveSystem.LoadEnemies(SceneLoader.Instance.GetCurrentLevelName());

        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].isDead = data.enemiesDead[i];
        }
    }   

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //SaveGame();
            menuManager?.Pause();
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            LoadEnemies();
            SceneLoader.Instance.LoadCoconuts();
        }
    }
}

[Serializable]
public class EnemyData
{
    public EnemyData(List<EnemyBehavior> enemies)
	{
        enemiesDead = new bool[enemies.Count];

        for(int i = 0; i < enemies.Count; i++)
		{
            enemiesDead[i] = enemies[i].isDead;
		}
	}

   public bool[] enemiesDead;
}