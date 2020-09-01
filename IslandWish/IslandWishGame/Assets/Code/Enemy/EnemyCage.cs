using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCage : MonoBehaviour
{
    [SerializeField] int maxHealth;
    private int currHealth;
    public bool isBroken = false;

    // Start is called before the first frame update
    void Start()
    {
        currHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "MeleeAttack")
		{
            currHealth-=1;
            AudioManager.Instance.Play("PCCageHit");
            if (currHealth <= 0)
			{
                isBroken = true;
                gameObject.SetActive(false);
			}
		}
	}
}
