using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class NetworkWin : NetworkBehaviour
{
    private string winnerName;
    public Text winText;

    public void DisplayWinScreen(string name)
    {
        winnerName = name;
        winText.text = " " + winnerName + "!";
        gameObject.SetActive(true);
    }

    public void HideWinScreen()
    {
        gameObject.SetActive(false);
    }
}
