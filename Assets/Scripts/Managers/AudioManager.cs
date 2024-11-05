using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Following this yt tutorial to set up a solid audio manager
// https://youtu.be/rdX7nhH6jdM?si=RxO-aM-_3iWXbqgB
public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance;

    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    public void Start()
    {
        PlayMusic("meow");
    }
    public void Awake()
    {

        // Makes sure there's only on instance of the manager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Call this bit if you want to play a singular bg track!!!!
    public void PlayMusic(string name)
    {

        // Attempts to find the music source with the rioght sound name in the array
        Sound s = Array.Find(musicSounds, x=> x.name == name);

        // if it isn't found, then error messages will be given
        if (s == null)
        {
            Debug.Log("The sound " + name + " has not been found!");
        }
        // if it is found, then the clip will be saved and played
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    // for sound effects that you're okay with having multiple of running at the same time, click this!!!
    public void PlaySFX(string name)
    {
        // Attempts to find the music source with the rioght sound name in the array
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        // if it isn't found, then error messages will be given
        if (s == null)
        {
            Debug.Log("The sound " + name + " has not been found!");
        }
        // if it is found, then the clip will be saved and played
        else
        {

            // Look into differentiating the value of the sounds dependent on the source
            // i.e. player, enemies, machinery, etc

            sfxSource.PlayOneShot(s.clip);
        }
    }

}
