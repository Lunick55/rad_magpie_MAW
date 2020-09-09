using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//this clas only exists because I can't assign an animation controller at runtime without Resources.Load
public class TransitionManager : BaseSingleton<TransitionManager>
{
    [SerializeField] Animator anim;
    [SerializeField] float transitionTime = 1f;

    public IEnumerator TransitionToScene(string sceneName)
    {
        anim.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneName);
    }  
    
    //these 2 should be used in tandem
    public IEnumerator TransitionStart(Event evnt)
    {
        anim.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        //i dunno, fire CutSceneEvent?
        StartCoroutine(TransitionEnd(evnt));
    }
    public IEnumerator TransitionEnd(Event evnt)
    {
        anim.SetTrigger("End");

        yield return new WaitForSeconds(transitionTime);

        //i dunno, fire event?
    }

}
