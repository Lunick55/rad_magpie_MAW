using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingItem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
            HealEvent heal = new HealEvent(1, transform.position);
            EventManager.instance.FireEvent(heal);
            Destroy(gameObject);
		}
	}
}
