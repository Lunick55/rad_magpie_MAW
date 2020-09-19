using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    private GameObject checkpointManager;
    public GameObject[] activateOnCheckpoint;

    private void Start()
    {
        checkpointManager = GameObject.Find("CheckpointManager");
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            checkpointManager.GetComponent<CheckpointManager>().SetCheckpoint(gameObject.transform.position) ;
            ActivateObjects();
            //gameObject.SetActive(false);
            GameManager.Instance.SaveGame();
        }
    }

    private void ActivateObjects()
    { 
        foreach(GameObject obj in activateOnCheckpoint)
        {
            obj.SetActive(true);
        }
    }
}
