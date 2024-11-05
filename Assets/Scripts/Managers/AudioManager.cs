using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Following this yt tutorial to set up a solid audio manager
// https://youtu.be/rdX7nhH6jdM?si=RxO-aM-_3iWXbqgB
public class AudioManager : MonoBehaviour
{

    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

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

}
