using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PrologueManager : MonoBehaviour
{
    public static int prologueClick;
    public TextMeshProUGUI narr;
    public GameObject stills;
    public List<AudioClip> narrClips;
    private int narrationIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        prologueClick = 0;
        ChangeText();
    }

    private void Update()
    {
        Debug.Log(prologueClick);
        UpdateClicks();
    }

    void UpdateClicks()
    {
        if (Input.GetKeyDown("space") || Input.GetMouseButtonDown(0) || Input.GetButtonDown("Submit"))
        {
            ChangeText();
        }
    }

    void ChangeText()
    {
        if (prologueClick == 0)
        {
            narr.text = "Coconut Island. A beautiful place named for the curious creatures that inhabit the land.".ToString();
            PlayAudio();
        }
        else if (prologueClick == 1)
        {
            narr.text = "Friendly by nature, the coconuts prospered in a peaceful kingdom that cared for all who touched its sand.".ToString();
            PlayAudio();

        }
        else if (prologueClick == 2)
        {
            narr.text = "However, a gentle heart is easily seized, and the less-kind island dwellers eventually grew less than pleased.".ToString();
            PlayAudio();
        }
        else if (prologueClick == 3)
        {
            narr.text = "“Why should the island belong to those weaklings?” they roared, conspiring in the dead of night. “We are bigger and stronger than they, they would hardly put up a fight.”".ToString();
            PlayAudio();
        }
        else if (prologueClick == 4)
        {
            narr.text = "And so their wicked plan was hatched. They invaded the kingdom, they swiped, they snatched.".ToString();
            PlayAudio();
        }
        else if (prologueClick == 5)
        {
            narr.text = "The island fell dark, overcome with hate. But a new hero arrives, to bring a new fate.".ToString();
            PlayAudio();
        }
        else if (prologueClick == 6)
        {
            narr.text = "So heed these words and please make haste, lest this island paradise fall further to waste.".ToString();
            PlayAudio();
        }
        else if (prologueClick == 7)
        {
            narr.text = "Wait no longer, no ifs, ands, or buts. Venture forth and rescue these poor coconuts.".ToString();
            PlayAudio();
        }
        else if (prologueClick > 7)
        {
            narr.text = "".ToString();
            stills.SetActive(false);
            SceneLoader.Instance.LoadScene("Level 1");
        }
        prologueClick++;
    }

    private void PlayAudio()
	{
        AudioManager.Instance.Stop("Narration");
        AudioManager.Instance.SetClip("Narration", narrClips[narrationIndex++]);
        AudioManager.Instance.Play("Narration");
    }
}
