using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerStats stats;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.AddListener(TakeDamage, EventTag.DAMAGE);
    }

    // Update is called once per frame
    void Update()
    {
        if(stats.health <= 0)
		{
            Debug.Log("ur dead bruh");
		}
    }

    public void TakeDamage(Event newDamageEvent)
	{
        DamageEvent damageEvent = (DamageEvent)newDamageEvent;

        stats.health -= damageEvent.damage;
	}

}
