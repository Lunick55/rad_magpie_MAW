using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EpilogueManager : MonoBehaviour
{
    public static int epilogueClick;
    public TextMeshProUGUI narr;
    public GameObject stills;

    // Start is called before the first frame update
    void Start()
    {
        epilogueClick = 0;
    }

    private void Update()
    {
        Debug.Log(epilogueClick);
        UpdateClicks();
        ChangeText();
    }

    void UpdateClicks()
    {
        if (Input.GetKeyDown("space"))
        {
            epilogueClick++;
        }
    }

    void ChangeText()
    {
        if (epilogueClick == 0)
        {
            narr.text = "The gates swing open and your friends rejoice. The old Cocoknight had made the right choice.".ToString();
        }
        else if (epilogueClick == 1)
        {
            narr.text = "The kingdom is saved and they usher you through, offer you shells, gems, fish, and even fruit too.".ToString();
        }
        else if (epilogueClick == 2)
        {
            narr.text = "You are hailed as a hero, a savior, a knight. The coconuts marvel at your resolve and might.".ToString();
        }
        else if (epilogueClick == 3)
        {
            narr.text = "With no hesitation, they welcome you to their home. The kind people of the kingdom have named you one of their own.".ToString();
        }
        else if (epilogueClick == 4)
        {
            narr.text = "And so our tale ends with a marvelous feast, in honor of the hero who slayed the scheming beast.".ToString();
        }
        else if (epilogueClick == 5)
        {
            narr.text = "We praise you, young hero, for not giving up the fight. We praise you, dear child, the new Cocoknight.".ToString();
        }
    }
}
