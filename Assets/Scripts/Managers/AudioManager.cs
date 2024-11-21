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
    public float sfxGeneral = 1f;

    public void Awake()
    {

        // Makes sure there's only on instance of the manager
        if (Instance == null)
        {
            Instance = this;

        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        
        getAudioModifiers();
    }

    // gets all the audio modifiers from the player prefs
    public void getAudioModifiers()
    {
        // Passes a ref, and string to get what we need from player prefs. 
        getModifiers("musicVol", ref musicVol);
        MusicVolume(musicVol);
        getModifiers("sfxVol", ref sfxVol);
        SFXVolume(sfxVol);
        getModifiers("sfxEnt", ref sfxEnt);
        SFXEntVolume(sfxEnt);
        getModifiers("sfxObj", ref sfxObj);
        SFXObjVolume(sfxObj);
        getModifiers("sfxSpell", ref sfxSpell);
        SFXSpellVolume(sfxSpell);
        getModifiers("sfxGen", ref sfxGeneral);
        SFXSGeneralVolume(sfxGeneral);
    }

    private void getModifiers(string sfxName, ref float sfxVol)
    {
        if (PlayerPrefs.HasKey(sfxName))
        {
            sfxVol = PlayerPrefs.GetFloat(sfxName);
        } else {
            PlayerPrefs.SetFloat(sfxName, 1f);
            sfxVol = PlayerPrefs.GetFloat(sfxName);
            PlayerPrefs.Save();
        }
    }

    // Call this bit if you want to play a singular bg track!!!!
    public void PlayMusic(string name)
    {
        getAudioModifiers();

        // Attempts to find the music source with the rioght sound name in the array
        Sound s = Array.Find(musicSounds, x => x.name == name);

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
    public void PlaySFX(string name, AudioSourceTypes someCase)
    {
        getAudioModifiers();
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
    public float getModifier(AudioSourceTypes someCase)
    {
        // depending on the scenarion, the code will have different return values depending the type of source
        switch (someCase)
        {
            case AudioSourceTypes.ENTITY:
                return sfxSource.volume * sfxEnt;
            case AudioSourceTypes.OBJECT:
                return sfxSource.volume * sfxObj;
            case AudioSourceTypes.SPELL:
                return sfxSource.volume * sfxSpell;
            case AudioSourceTypes.GENERAL:
                return sfxSource.volume * sfxGeneral;
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
    public void SFXSGeneralVolume(float volume)
    {
        sfxGeneral = volume;

        PlayerPrefs.SetFloat("sfxGen", sfxGeneral);
        PlayerPrefs.Save();
    }

}
