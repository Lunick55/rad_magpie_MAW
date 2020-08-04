using UnityEngine.Audio;
using UnityEngine;


[System.Serializable]
public class Sound
{
   //simple name for audio taking the longer version and just writing player death instead of it reading player death sound final
   public string name;

   //choose your clip (FIGHT!)
   public AudioClip clip;

   //close your ears, open your ears (volume range)
   [Range(0f, 1f)]
   public float volume;

   //dogwhistle, bass drum (pitch range)
   [Range(.1f, 3f)]
   public float pitch;

   public bool loop;

   [HideInInspector]
   public AudioSource source;

}
