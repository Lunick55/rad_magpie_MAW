using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

public class SceneLoader : BasePersistentSingleton<SceneLoader>
{
	List<CoconutData> coconuts;
	bool isInit = false;
	public bool loadData = false;
	private string nextLevel;
	private PostProcessVolume postProcess;

	public void Init()
	{
		if(!isInit)
		{
			coconuts = new List<CoconutData>();
			//anim.runtimeAnimatorController = ;

			isInit = true;
		}
	}

	public void LoadScene(string scene)
	{
		//TODO: turn off player movement n'stuff
		StartCoroutine(TransitionManager.Instance.TransitionToScene(scene));
	}
	public void FinishLevel(string nextLevelName, PostProcessVolume newPost)
	{
		nextLevel = nextLevelName;
		postProcess = newPost;
	}
	public void NextLevel()
	{
		LoadScene(nextLevel);
	}
	public PostProcessVolume GetPost()
	{
		return postProcess;
	}


	public void AddSavedCoconuts(List<CoconutPetBehavior> savedCoconuts)
	{
		foreach (CoconutPetBehavior coconut in savedCoconuts)
		{
			CoconutData cocoData = new CoconutData(coconut.name, coconut.accessory);
			coconuts.Add(cocoData);
		}
	}

	public List<CoconutData> GetSavedCoconuts()
	{
		return coconuts;
	}

	public string GetCurrentLevelName()
	{
		return SceneManager.GetActiveScene().name;
	}

	public void Reset()
	{
		coconuts.Clear();
		coconuts.Capacity = 0;

	}

	public void LoadCoconuts()
	{
		Reset();

		CoconutSaveData data = SaveSystem.LoadCoconuts();

		for (int i = 0; i < data.name.Length; i++)
		{
			CoconutData coconut = new CoconutData(data.name[i], CoconutManager.Instance.cocoAttach.GetAccessoryFromID(data.accessoryID[i]));
			coconut.isSaved = data.isSaved[i];

			coconuts.Add(coconut);
		}
	}
}
