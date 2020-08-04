using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StillManager : MonoBehaviour
{
    public Sprite still1, still2, still3, still4, still5;
    int clicks;

    // Update is called once per frame
    void Update()
    {
        clicks = PrologueManager.prologueClick;
        UpdateStill();
    }

    void UpdateStill()
    {
        if(clicks == 0)
        {
            gameObject.GetComponent<Image>().sprite = still1;
        }
        else if(clicks == 1)
        {
            gameObject.GetComponent<Image>().sprite = still2;
        }
        else if(clicks == 2)
        {
            gameObject.GetComponent<Image>().sprite = still3;
        }
        else if(clicks == 4)
        {
            gameObject.GetComponent<Image>().sprite = still4;
        }
        else if(clicks == 6)
        {
            gameObject.GetComponent<Image>().sprite = still5;
        }

    }
}
