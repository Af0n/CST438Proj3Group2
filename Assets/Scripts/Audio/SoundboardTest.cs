using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundboardTest : MonoBehaviour
{
    void Update()
    {
        // Music testing
        if (Input.GetKeyDown(KeyCode.B))
            AudioManager.Instance.PlayMusic("buddy holly");
        if (Input.GetKeyDown(KeyCode.M))
            AudioManager.Instance.PlayMusic("meow");

        // sfx testing
        if (Input.GetKeyDown(KeyCode.C))
            AudioManager.Instance.PlaySFX("concrete scrape", 2);
        if (Input.GetKeyDown(KeyCode.G))
            AudioManager.Instance.PlaySFX("glorp", 1);
        if (Input.GetKeyDown(KeyCode.S))
            AudioManager.Instance.PlaySFX("meat slap", 0);
        if (Input.GetKeyDown(KeyCode.V))
            AudioManager.Instance.PlaySFX("vine boom", 3);
    }

}
