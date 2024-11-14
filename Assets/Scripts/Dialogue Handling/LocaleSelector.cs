using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Localization.Settings;

public class LocaleSelector : MonoBehaviour
{

    // bool that keeps track of whether the locale is currently getting changed
    private bool active = false;

    // can be called by other systems to changde the locale
    public void ChangeLocale(int localeID)
    {

        if (active)
        {
            return;
        }

        StartCoroutine(SetLocale(localeID));
    }

    // Main function the switches the locale
    IEnumerator SetLocale (int _localeID)
    {

        // Sets the tracker to true
        active = true;

        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_localeID];

        // and once the locale has been set, it will turn to false
        active = false;

    }

}
