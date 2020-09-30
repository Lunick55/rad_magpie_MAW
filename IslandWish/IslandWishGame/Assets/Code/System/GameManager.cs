using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : BaseSingleton<GameManager>
{
    [Header("Player Info")]
    [SerializeField] List<Player> players;
    [HideInInspector] List<Movement> playersMove;
    [HideInInspector] List<Transform> playersTrans;

    [Header("UI Info")]
    [SerializeField] GameObject singlePlayerUI;
    [SerializeField] GameObject coopUI;
    [SerializeField] List<Camera> cameras;
    [SerializeField] List<HUDScript> huds;

    [Header("Enemy Info")]
    [SerializeField] public List<EnemyBehavior> enemies;
    int numEnemiesAggroed = 0;

    //do i even like this?
    [SerializeField] MenuManager menuManager;
    [SerializeField] NarrationCompletion narrateComplete;
 
    [SerializeField] PlayersCount debugPlayersCount;
	public enum PlayersCount
	{
        SINGLE = 1,
        COOP = 2
    }

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
            Debug.Log("Fill out the Player field");
        }
        else
        {
            playersTrans = new List<Transform>(players.Count);
            playersMove = new List<Movement>(players.Count);
            for (int i = 0; i < players.Count; i++)
            {
                playersTrans.Add(players[i].GetComponent<Transform>());
                playersMove.Add(players[i].GetComponent<Movement>());
            }

            int count = SceneLoader.Instance.playerCount;
            if (count == 0)
			{
                SceneLoader.Instance.playerCount = count = (int)debugPlayersCount;
			}

            if (count == 1)
            {
                //setup player 1 UI
                cameras[0].gameObject.SetActive(true);                
                cameras[1].gameObject.SetActive(false);

                cameras[0].rect = new Rect(0.0f, 0.0f, 1f, 1f);

                players[0].hud = huds[0];
                players[0].gameObject.SetActive(true);
                players[1].gameObject.SetActive(false);

                singlePlayerUI.SetActive(true);
                coopUI.SetActive(false);
            }
            else
			{
                //setup player 2 UI
                cameras[0].gameObject.SetActive(true);
                cameras[1].gameObject.SetActive(true);

                cameras[0].rect = new Rect(0.0f, 0.0f, 0.5f, 1f);
                cameras[1].rect = new Rect(0.5f, 0.0f, 0.5f, 1f);

                players[0].hud = huds[1];
                players[1].hud = huds[2];
                players[0].gameObject.SetActive(true);
                players[1].gameObject.SetActive(true);

                singlePlayerUI.SetActive(false);
                coopUI.SetActive(true);
            }

            LoadGame();

            if (count == 1)
            {
                cameras[0].transform.position = new Vector3(playersTrans[0].position.x + 4f, playersTrans[0].position.y + 7f, playersTrans[0].position.z + -4f);
                cameras[0].transform.eulerAngles = new Vector3(45, -45, 0);
            }
            else
            {
                cameras[0].transform.position = new Vector3(playersTrans[0].position.x + 2.4f, playersTrans[0].position.y + 7f, playersTrans[0].position.z + -2.4f);
                cameras[0].transform.eulerAngles = new Vector3(56, -45, 0);
                cameras[1].transform.position = new Vector3(playersTrans[1].position.x + 2.4f, playersTrans[1].position.y + 7f, playersTrans[1].position.z + -2.4f);
                cameras[1].transform.eulerAngles = new Vector3(56, -45, 0);
            }

        }

    }

    public void CoconutCelebration()
	{
        foreach(HUDScript hud in huds)
		{
            hud.hooray.Play();
		}

        if(narrateComplete)
		{
            narrateComplete.CocosFound();
        }

        AudioManager.Instance.Play("AllCocoRescue");
        huds[0].boatCommand.SetActive(true);
	}

    public void PauseEnemies()
	{
        for(int i = 0; i < enemies.Count; i++)
		{
            enemies[i].Pause();
		}
	}
    public void ResumeEnemies()
	{
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].Resume();
        }
    }

    public void EnableHud()
	{
        int count = SceneLoader.Instance.playerCount;
        if (count == 1)
        {
            singlePlayerUI.SetActive(true);
            coopUI.SetActive(false);
        }
        else
        {
            singlePlayerUI.SetActive(false);
            coopUI.SetActive(true);
        }
    }
    public void DisableHud()
	{
        int count = SceneLoader.Instance.playerCount;
        if (count == 1)
        {
            singlePlayerUI.SetActive(false);
            coopUI.SetActive(false);
        }
        else
        {
            singlePlayerUI.SetActive(false);
            coopUI.SetActive(false);
        }
    }

    public void LoadGame()
	{
        if (SceneLoader.Instance.loadSingleData || SceneLoader.Instance.loadMultiData)
        {
            SceneLoader.Instance.loadSingleData = false;

            //TODO: load all the data calls needed here
            foreach (Player player in players)
            {
                player.LoadPlayer();
            }
            LevelManager.Instance.LoadLevel();
            SceneLoader.Instance.LoadPersistentCoconuts();
        }
        else
        {
            for (int i = 0; i < GetPlayerCount(); i++)
            {
                players[i].currentHealth = players[i].stats.health;
            }

            foreach(CoconutPetBehavior coco in CoconutManager.Instance.coconuts)
			{
                coco.CreateCoconutLook();
			}            
        }
    }

    public void SaveGame()
	{
        foreach (Player player in players)
        {
            SaveSystem.SavePlayer(player);
        }
        LevelManager.Instance.SaveLevel();
        SaveSystem.SaveCoconuts(SceneLoader.Instance.GetSavedCoconuts());
        SceneLoader.Instance.SaveProgress();

        StartCoroutine(PlaySaveAnim());
    }

    IEnumerator PlaySaveAnim()
	{
        huds[0].saveUI.SetActive(true);
        yield return new WaitForSeconds(2);
        huds[0].saveUI.SetActive(false);
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
    public int GetPlayerCount()
	{
        return SceneLoader.Instance.playerCount;
	}

    public int GetClosestPlayer(Vector3 point, out Transform closestTrans)
	{
        int closestIndex = 0;
        float closestDistance = (point - playersTrans[0].position).sqrMagnitude;
        for (int i = 0; i < playersTrans.Count; i++)
		{
            if ((point - playersTrans[i].position).sqrMagnitude < closestDistance)
			{
                closestIndex = i;
			}
        }
        closestTrans = playersTrans[closestIndex];
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
   

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Pause"))
        {            
            menuManager?.Pause();
        }
        else if(menuManager.isPause() && Input.GetButtonDown("Cancel"))
		{
            menuManager.Resume();
		}
    }
}