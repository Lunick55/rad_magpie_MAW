using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : BaseSingleton<AudioManager>
{

   public Sound[] sounds;

    void Awake()
    {
        foreach (Sound s in sounds)
      {
         //lists off each component of the audio manager for each sound or music
         s.source = gameObject.AddComponent<AudioSource>();
         s.source.clip = s.clip;

         s.source.volume = s.volume;
         s.source.pitch = s.pitch;
         s.source.loop = s.loop;
        
      }
    }

   void Start()
   {
      //play on start of game or level
      Play("MenuMusic");
   }

   public void Play(string name)
   {
        //looks for the name of the song
        Sound s = Array.Find(sounds, sound => sound.name == name);

        //won't play sound if spelt wrong or not existing
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " does not exist");
            return;
        }
        s.source.Play();
   }
}
