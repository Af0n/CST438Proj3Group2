using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioAdjustmentScript : MonoBehaviour
{

    public Slider _musicSlider, _sfxSlider,
                    _sfxEntitySlider, _sfxObjectSlider,
                    _sfxSpellSlider, _genSider;

    // Sets up the value of the sound sl;iders when the game starts
    public void Start()
    {

        _musicSlider.value = AudioManager.Instance.getMusicVolume();
        _sfxSlider.value = AudioManager.Instance.getSFXVolume();

        _sfxEntitySlider.value = AudioManager.Instance.sfxEnt;
        _sfxObjectSlider.value = AudioManager.Instance.sfxObj;
        _sfxSpellSlider.value = AudioManager.Instance.sfxSpell;
        _genSider.value = AudioManager.Instance.sfxGeneral;
    }

    public void ToggleMusic()
    {
        AudioManager.Instance.ToggleMusic();
    }

    public void ToggleSFX()
    {
        AudioManager.Instance.ToggleSFX();
    }

    public void MusicVolume()
    {
        AudioManager.Instance.MusicVolume(_musicSlider.value);
    }

    public void SFXVolume()
    {
        AudioManager.Instance.SFXVolume(_sfxSlider.value);
    }

    public void SFXEntVolume()
    {
        AudioManager.Instance.SFXEntVolume(_sfxEntitySlider.value);
    }

    public void SFXObjVolume()
    {
        AudioManager.Instance.SFXObjVolume(_sfxObjectSlider.value);
    }

    public void SFXSpellVolume()
    {
        AudioManager.Instance.SFXSpellVolume(_sfxSpellSlider.value);
    }

    public void SFXGenVolume() {
        AudioManager.Instance.SFXSGeneralVolume(_genSider.value);
    }
}
