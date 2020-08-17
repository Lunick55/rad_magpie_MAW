using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BoatScreenManager : MonoBehaviour
{
    //[SerializeField] TextMeshProUGUI text;
    [SerializeField] List<Transform> spawnPoints;
    private List<Transform> usedSpawnPoints;
    [SerializeField] GameObject coconutReference;

    // Start is called before the first frame update
    void Start()
    {
        SceneLoader.Instance.Init();
        usedSpawnPoints = new List<Transform>();

        LoadCoconuts();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadCoconuts()
	{
        //List<string> cocoNames = SceneLoader.Instance.GetSavedCoconutNames();
        List<CoconutData> coconuts = SceneLoader.Instance.GetSavedCoconuts();

        foreach(CoconutData coconut in coconuts)
		{
            //text.text += (coconut.name + "\n");
            PickSpawnPosition(coconut);
		}
	}

    private void PickSpawnPosition(CoconutData cocoData)
	{
        if (spawnPoints.Count > 0)
        {
            int randomSpawn = Random.Range(0, spawnPoints.Count);

            GameObject coco = Instantiate(coconutReference, spawnPoints[randomSpawn].position, Quaternion.identity);
            CoconutPetBehavior cocoBe = coco.GetComponent<CoconutPetBehavior>();
            cocoBe.accessory = cocoData.accessory;
            cocoBe.displayMode = true;
            usedSpawnPoints.Add(spawnPoints[randomSpawn]);
            spawnPoints.RemoveAt(randomSpawn);
        }
        else if (usedSpawnPoints.Count > 0)
		{
            int randomSpawn = Random.Range(0, usedSpawnPoints.Count);
            Vector3 spawnPoint = usedSpawnPoints[randomSpawn].position + new Vector3(0, 0.5f, 0);

            GameObject coco = Instantiate(coconutReference, spawnPoint, Quaternion.identity);
            CoconutPetBehavior cocoBe = coco.GetComponent<CoconutPetBehavior>();
            cocoBe.accessory = cocoData.accessory;
            cocoBe.displayMode = true;
        }
    }
}
