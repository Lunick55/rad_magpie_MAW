using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NarrationObject : MonoBehaviour
{
	private int narrationIndex = 0;

    [SerializeField] List<string> narrationText;
    [SerializeField] List<AudioClip> narrationAudio;
	private string audioChannelName = "Narration";

	private bool canNarrate = false;

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Space) && canNarrate)
		{
			AudioManager.Instance.Stop(audioChannelName);
			if (++narrationIndex >= narrationAudio.Count)
			{
				LevelManager.Instance.narrationUI.gameObject.SetActive(false);

				for (int i = 0; i < GameManager.Instance.GetPlayerCount(); i++)
				{
					GameManager.Instance.GetPlayer(i).canMove = true;
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

	public void StartMovie()
	{
		for (int i = 0; i < GameManager.Instance.GetPlayerCount(); i++)
		{
			GameManager.Instance.GetPlayer(i).canMove = false;
		}

		LevelManager.Instance.playerUI.gameObject.SetActive(false);

		StartCoroutine(LevelManager.Instance.MovieTransitionStart(StartNarration));
	}

	public void StartNarration()
	{
		LevelManager.Instance.narrationUI.gameObject.SetActive(true);
		LevelManager.Instance.text.text = narrationText[narrationIndex];
		AudioManager.Instance.SetClip(audioChannelName, narrationAudio[narrationIndex]);
		AudioManager.Instance.Play(audioChannelName);
		canNarrate = true;
	}
	public void EndNarration()
	{
		LevelManager.Instance.playerUI.gameObject.SetActive(true);
	}

	//TODO: make this whole class a base, and just override this function for conditions needed
	private void OnTriggerEnter(Collider other)
	{
		StartMovie();
	}
}
