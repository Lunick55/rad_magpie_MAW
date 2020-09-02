using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    [HideInInspector] public DoorScript door;
    public bool isCollected = false;

    public void CollectKey()
	{
        door.UnlockLock();
        isCollected = true;
        gameObject.SetActive(false);
        AudioManager.Instance.Play("Pickup");
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            CollectKey();
        }
    }
}
