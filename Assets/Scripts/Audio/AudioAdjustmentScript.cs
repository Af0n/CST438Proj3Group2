using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AudioAdjustmentScript : MonoBehaviour
{

    public Slider _musicSlider, _sfxSlider, 
                    _sfxEntityrSlider, _sfxObjectSlider, 
                    _sfxSpellSlider;

    // refers to the category of the slider

    /*
     * 
     * 1 == music
     * 2 == sfx
     * 3 == sfx entity
     * 4 == sfx object
     * 
     */

    public int type;

    public void Start()
    {
        


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
        AudioManager.Instance.SFXEntVolume(_sfxEntityrSlider.value);
    }

    public void SFXObjVolume()
    {
        AudioManager.Instance.SFXObjVolume(_sfxObjectSlider.value);
    }

    public void SFXSpellVolume()
    {
        AudioManager.Instance.SFXSpellVolume(_sfxSpellSlider.value);
    }


}
