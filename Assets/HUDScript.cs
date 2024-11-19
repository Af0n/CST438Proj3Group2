using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDScript : MonoBehaviour
{
    public TextMeshProUGUI moneyText;
    // Updates the TMP text with the formatted money value
    public void UpdateMoneyText(long playerMoney)
    {
        moneyText.text = $"${playerMoney:N0}"; // Formats with commas (e.g., 1,000)
    }
}
