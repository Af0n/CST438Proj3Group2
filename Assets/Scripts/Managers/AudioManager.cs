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

    // Initial audio vals
    public float musicVol = 1f;
    public float sfxVol = 1f;

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

        getAudioModifiers();

    }

    // gets all the audio modifiers from the player prefs
    public void getAudioModifiers()
    {

        // Gets/makes the mmusic volume in the player prefs
        if (PlayerPrefs.HasKey("musicVol"))
        {
            musicVol = PlayerPrefs.GetFloat("musicVol");
        }
        else
        {
            PlayerPrefs.SetFloat("musicVol", 1f);
            musicVol = PlayerPrefs.GetFloat("musicVol");
            PlayerPrefs.Save();
        }

        // Gets/makes the sfx volume in the player prefs
        if (PlayerPrefs.HasKey("sfxVol"))
        {
            sfxVol = PlayerPrefs.GetFloat("sfxVol");
        }
        else
        {
            PlayerPrefs.SetFloat("sfxVol", 1f);
            sfxVol = PlayerPrefs.GetFloat("sfxVol");
            PlayerPrefs.Save();
        }

        // Gets/makes the sfxEnt volume in the player prefs
        if (PlayerPrefs.HasKey("sfxEnt"))
        {
            sfxEnt = PlayerPrefs.GetFloat("sfxEnt");
        }
        else
        {
            PlayerPrefs.SetFloat("sfxEnt", 1f);
            sfxEnt = PlayerPrefs.GetFloat("sfxEnt");
            PlayerPrefs.Save();
        }

        // Gets/makes the sfxObj volume in the player prefs
        if (PlayerPrefs.HasKey("sfxObj"))
        {
            sfxObj = PlayerPrefs.GetFloat("sfxObj");
        }
        else
        {
            PlayerPrefs.SetFloat("sfxObj", 1f);
            sfxObj = PlayerPrefs.GetFloat("sfxObj");
            PlayerPrefs.Save();
        }

        // Gets/makes the sfx volume in the player prefs
        if (PlayerPrefs.HasKey("sfxSpell"))
        {
            sfxSpell = PlayerPrefs.GetFloat("sfxSpell");
        }
        else
        {
            PlayerPrefs.SetFloat("sfxSpell", 1f);
            sfxSpell = PlayerPrefs.GetFloat("sfxSpell");
            PlayerPrefs.Save();
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
    public void PlaySFX(string name, int someCase)
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

            sfxSource.PlayOneShot(s.clip, getModifier(someCase));
        }
    }

    public float getModifier(int someCase)
    {

        // depending on the scenarion, the code will have different return values depending the type of source
        /*
         * 1 == entity
         * 2 == object
         * 3 == spell
         * 
         */
        switch (someCase)
        {
            case 1:
                return sfxSource.volume * sfxEnt;
            case 2:
                return sfxSource.volume * sfxObj;
            case 3:
                return sfxSource.volume * sfxSpell;
            default:
                return sfxSource.volume;
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
        musicVol = volume;
        musicSource.volume = musicVol;

        PlayerPrefs.SetFloat("musicVol", musicVol);
        PlayerPrefs.Save();
    }

    // Adjust volume for sfx
    public void SFXVolume(float volume)
    {
        sfxVol = volume;
        sfxSource.volume = sfxVol;

        PlayerPrefs.SetFloat("sfxVol", sfxVol);
        PlayerPrefs.Save();
    }

    // Getter for the music Source

    public float getMusicVolume()
    {
        return musicVol;
    }

    // Getter for the SFX source

    public float getSFXVolume()
    {
        return sfxVol;
    }

    // Adjusts volumes for entity sfxs
    public void SFXEntVolume(float volume)
    {
        sfxEnt = volume;

        PlayerPrefs.SetFloat("sfxEnt", sfxEnt);
        PlayerPrefs.Save();
    }

    // Adjusts volumes for object sfxs
    public void SFXObjVolume(float volume)
    {
        sfxObj = volume;

        PlayerPrefs.SetFloat("sfxObj", sfxObj);
        PlayerPrefs.Save();
    }

    // Adjusts volumes for spell sfxs
    public void SFXSpellVolume(float volume)
    {
        sfxSpell = volume;

        PlayerPrefs.SetFloat("sfxSpell", sfxSpell);
        PlayerPrefs.Save();
    }

}
