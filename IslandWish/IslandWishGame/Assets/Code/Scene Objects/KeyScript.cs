using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    [HideInInspector] public DoorScript door;
    public bool isCollected = false;
    public Sprite sprite;

	private void Start()
	{
		if(isCollected)
		{
            gameObject.SetActive(false);
        }
    }

	public void CollectKey(Player player)
	{
        //door.UnlockLock();
        isCollected = true;
        gameObject.SetActive(false);
        player.hud.AddKey(this);
        AudioManager.Instance.Play("Pickup");
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            CollectKey(collision.GetComponent<Player>());
        }
    }
}
