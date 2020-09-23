using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering.PostProcessing;

public class BoatScreenManager : MonoBehaviour
{
    //[SerializeField] TextMeshProUGUI text;
    [SerializeField] List<Transform> spawnPoints;
    private List<Transform> usedSpawnPoints;
    [SerializeField] GameObject coconutReference;
    [SerializeField] private PostProcessVolume post;

    // Start is called before the first frame update
    void Start()
    {
        SceneLoader.Instance.Init();
        usedSpawnPoints = new List<Transform>();

        LoadCoconuts();

        post = SceneLoader.Instance.GetPost();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadCoconuts()
	{
        List<CoconutData> coconuts = SceneLoader.Instance.GetSavedCoconuts();

        float height = 0f;
        foreach(CoconutData coconut in coconuts)
		{
            PickSpawnPosition(coconut, ref height);
		}
	}

    private void PickSpawnPosition(CoconutData cocoData, ref float height)
	{
        int randomSpawn = 0;
        Vector3 spawnPoint = Vector3.zero;

        if (spawnPoints.Count > 0)
        {
            //pick spawn point
            randomSpawn = Random.Range(0, spawnPoints.Count);
            spawnPoint = spawnPoints[randomSpawn].position + new Vector3(0, height, 0);

        }
        else if (usedSpawnPoints.Count > 0)
		{
            for(int i = 0; i < usedSpawnPoints.Count; i++)
			{
                spawnPoints.Add(usedSpawnPoints[i]);
            }
            usedSpawnPoints.Clear();
            usedSpawnPoints.Capacity = 0;
            height += 0.5f;

            randomSpawn = Random.Range(0, spawnPoints.Count);
            spawnPoint = spawnPoints[randomSpawn].position + new Vector3(0, height, 0);
        }

        GameObject coco = Instantiate(coconutReference, spawnPoint, Quaternion.identity);
        Vector3 parentRot = Vector3.zero;
        parentRot.y = spawnPoints[randomSpawn].eulerAngles.y;
        coco.transform.eulerAngles = parentRot;
        CoconutPetBehavior cocoBe = coco.GetComponent<CoconutPetBehavior>();

        cocoBe.LoadCoconutLook(cocoData);

        cocoBe.displayMode = true;
        usedSpawnPoints.Add(spawnPoints[randomSpawn]);
        spawnPoints.RemoveAt(randomSpawn);
    }
}
