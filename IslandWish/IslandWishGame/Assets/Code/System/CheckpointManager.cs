using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public Vector3 checkpoint;

    // Start is called before the first frame update
    void Start()
    {
        SetCheckpoint(GameObject.Find("CocoPlayer").transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCheckpoint(Vector3 _newCheckpoint)
    {
        checkpoint = _newCheckpoint;
    }

    public Vector3 GetCheckpoint()
    {
        return checkpoint;
    }


}
