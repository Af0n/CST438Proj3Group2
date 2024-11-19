using UnityEngine;

public class HighlightAction : MonoBehaviour
{
    public void OnHighlight()
    {
        if(AudioManager.Instance != null) {
            AudioManager.Instance.PlaySFX("Select", 1);
        }
    }
}
