using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//this clas only exists because I can't assign an animation controller at runtime without Resources.Load
public class TransitionManager : BaseSingleton<TransitionManager>
{
    [SerializeField] Animator anim;
    [SerializeField] float transitionTime = 1f;

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Transition(string sceneName)
    {
        anim.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneName);
    }

}
