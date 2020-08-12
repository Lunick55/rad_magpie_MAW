using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BoatScreenManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        SceneLoader.Instance.Init();
        LoadCoconuts();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadCoconuts()
	{
        List<string> cocoNames = SceneLoader.Instance.GetSavedCoconuts();

        foreach(string name in cocoNames)
		{
            text.text += (name + "\n");
		}
	}
}
