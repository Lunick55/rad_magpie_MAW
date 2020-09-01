using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : BaseSingleton<GameManager>
{
    public Player player;
    public Movement playerMove;
    [HideInInspector] public Transform playerTrans;

    [SerializeField] List<EnemyBehavior> enemies;
    int numEnemiesAggroed = 0;

    //[SerializeField] LevelManager

	// Start is called before the first frame update
	void Awake()
    {
        Init();
    }

    public void Init()
	{
        SceneLoader.Instance.Init();

        if (!player)
        {
            Debug.LogError("Fill out the Player field");
        }
        playerTrans = player.GetComponent<Transform>();
        playerMove = player.GetComponent<Movement>();

        if (SceneLoader.Instance.loadData)
        {
            SceneLoader.Instance.loadData = false;

            //load all the data calls needed here
            player.LoadPlayer();
            LoadEnemies();
            LevelManager.Instance.LoadLevel();
            SceneLoader.Instance.LoadCoconuts();
        }
        else
        {
            player.currentHealth = player.stats.health;
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
            SaveSystem.SavePlayer(player);
            SaveSystem.SaveCoconuts(SceneLoader.Instance.GetSavedCoconuts());
            SaveSystem.SaveEnemies(enemies, SceneLoader.Instance.GetCurrentLevelName());
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