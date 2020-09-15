using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StillManagerEpi : MonoBehaviour
{
    public Sprite still1, still2, still3, still4;
    int clicks;

    // Update is called once per frame
    void Update()
    {
        clicks = EpilogueManager.epilogueClick;
        UpdateStill();
    }

    void UpdateStill()
    {
        if (clicks == 0)
        {
            gameObject.GetComponent<Image>().sprite = still1;
        }
        else if (clicks == 3)
        {
            gameObject.GetComponent<Image>().sprite = still2;
        }
        else if (clicks == 6)
        {
            gameObject.GetComponent<Image>().sprite = still3;
        }
        else if (clicks == 8)
        {
            gameObject.GetComponent<Image>().sprite = still4;
        }

    }
}
