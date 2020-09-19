using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NarrationObject : MonoBehaviour
{
	private int narrationIndex = 0;

    [SerializeField] protected List<string> narrationText;
    [SerializeField] protected List<AudioClip> narrationAudio;
	private string audioChannelName = "Narration";
	protected bool canNarrate = false;
	public bool complete = false;

	// Update is called once per frame
	protected void Update()
    {
		if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || Input.GetButtonDown("Submit")) && canNarrate)
		{
			AudioManager.Instance.Stop(audioChannelName);
			if (++narrationIndex >= narrationAudio.Count)
			{
				LevelManager.Instance.narrationUI.gameObject.SetActive(false);

				for (int i = 0; i < GameManager.Instance.GetPlayerCount(); i++)
				{
					GameManager.Instance.GetPlayer(i).canMove = true;
					GameManager.Instance.GetPlayer(i).canAttack = true;
					GameManager.Instance.ResumeEnemies();
				}

				StartCoroutine(Level1Manager.Instance.MovieTransitionEnd(EndNarration));
				canNarrate = false;

				return;
			}

			LevelManager.Instance.text.text = narrationText[narrationIndex];
			AudioManager.Instance.SetClip(audioChannelName, narrationAudio[narrationIndex]);
			AudioManager.Instance.Play(audioChannelName);
		}
	}

	protected void StartMovie()
	{
		GameManager.Instance.PauseEnemies();

		for (int i = 0; i < GameManager.Instance.GetPlayerCount(); i++)
		{
			GameManager.Instance.GetPlayer(i).canMove = false;
			GameManager.Instance.GetPlayer(i).canAttack = false;
		}

		GameManager.Instance.DisableHud();

		StartCoroutine(LevelManager.Instance.MovieTransitionStart(StartNarration));
	}

	protected void StartNarration()
	{
		LevelManager.Instance.narrationUI.gameObject.SetActive(true);
		LevelManager.Instance.text.text = narrationText[narrationIndex];
		AudioManager.Instance.SetClip(audioChannelName, narrationAudio[narrationIndex]);
		AudioManager.Instance.Play(audioChannelName);
		canNarrate = true;
	}
	protected void EndNarration()
	{
		GameManager.Instance.EnableHud();
		complete = true;
	}

	//TODO: make this whole class a base, and just override this function for conditions needed
	protected virtual void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && !complete)
		{
			StartMovie();
		}
	}
}
