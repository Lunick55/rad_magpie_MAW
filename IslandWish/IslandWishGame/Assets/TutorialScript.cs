using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    public GameObject tutorialUI;

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
        if (other.tag == "Player" && (CoconutManager.Instance.coconutsFreed.Count < CoconutManager.Instance.coconuts.Count))
        {
            tutorialUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            tutorialUI.SetActive(false);
            Destroy(gameObject);
        }
    }
}
