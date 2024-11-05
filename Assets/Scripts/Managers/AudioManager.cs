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

    // Extra audio vals for smaller audio tweaking
    public float sfxEnt = 1f;
    public float sfxObj = 1f;
    public float sfxSpell = 1f;

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

    // Flips whether the music is playing or not
    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }

    // Flips whether the SFX is playing or not
    public void ToggleSFX()
    {
        sfxSource.mute = !sfxSource.mute;
    }

    // Adjust volume for music
    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    // Adjust volume for sfx
    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }

    // Adjusts volumes for entity sfxs
    public void SFXEntVolume(float volume)
    {
        sfxEnt = volume;
    }

    // Adjusts volumes for object sfxs
    public void SFXObjVolume(float volume)
    {
        sfxObj = volume;
    }

    // Adjusts volumes for spell sfxs
    public void SFXSpellVolume(float volume)
    {
        sfxSpell = volume;
    }

}
