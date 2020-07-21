using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //didn't know this was a thing but made this stupid easy and made it harder than I thought

public class SceneChangeScriptFinalScene : MonoBehaviour //look for script SceneChangeLVL1, this will direct to the boat scene to change to level 1
{

   void OnTriggerEnter(Collider other) //this is the current win condition, can be kept or changed for what it best suited
   {
      SceneManager.LoadScene(4);
      //Time to fight some bois!!!!
   }

}