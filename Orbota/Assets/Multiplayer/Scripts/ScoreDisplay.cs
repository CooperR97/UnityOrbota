using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    private Text displayText;
    private int currentPlayer;
    private NetworkedCorgiCharacter player1;
    private NetworkedCorgiCharacter player2;
    private string defaultText;

    // Start is called before the first frame update
    void Start()
    {
        displayText = GetComponent<Text>();
        if(displayText.text.Contains("1"))
        {
            defaultText = displayText.text;
            currentPlayer = 1;
        }
        else if(displayText.text.Contains("2"))
        {
            defaultText = displayText.text;
            currentPlayer = 2;
        }
        UpdateText();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateText();
    }

    void FixedUpdate()
    {
        if (player1 == null && GameObject.Find("Player 1") != null)
        {
            player1 = GameObject.Find("Player 1").GetComponent<NetworkedCorgiCharacter>();
        }
        if (player2 == null && GameObject.Find("Player 2") != null)
        {
            player2 = GameObject.Find("Player 2").GetComponent<NetworkedCorgiCharacter>();
        }
    }

    private void UpdateText()
    {
        if (player1 != null && currentPlayer == 1)
        {
            displayText.text = defaultText + player1.score;
        }
        else if (player2 != null && currentPlayer == 2)
        {
            displayText.text = defaultText + player2.score;
        }
    }
}
