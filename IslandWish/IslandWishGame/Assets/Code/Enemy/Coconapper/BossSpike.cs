using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpike : MonoBehaviour
{
    [SerializeField] float destroyTimer = 1.0f;
    private float timer = 0;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= destroyTimer)
		{
            Destroy(gameObject);
		}
    }
}
