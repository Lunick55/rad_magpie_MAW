using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //didn't know this was a thing but made this stupid easy and made it harder than I thought

public class BoatToLevel3 : MonoBehaviour
{
   void OnTriggerEnter(Collider other) //this is the current win condition, can be kept or changed for what it best suited
   {
      SceneManager.LoadScene(3);
      //Time to fight some bois!!!!
   }

}
