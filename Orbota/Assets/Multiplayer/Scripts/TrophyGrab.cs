using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TrophyGrab : NetworkBehaviour
{
    public bool isOver;
    public string winnerName;
    public NetworkWin winScreen;
    private bool resetting;
    private NetworkedCorgiCharacter player1;
    private NetworkedCorgiCharacter player2;

    void Start()
    {
        winScreen = Resources.FindObjectsOfTypeAll<NetworkWin>()[0];
    }

    void Update()
    {
        //Check if round is resetting
        if (resetting == false && winScreen.isActiveAndEnabled)
        {
            winScreen.HideWinScreen();
        }
    }

    void OnTriggerEnter2D(Collider2D collisionObject)
    {
        //On round end
        if(collisionObject.CompareTag("Player"))
        {
            winnerName = collisionObject.name;
            NewRound();
            winScreen.DisplayWinScreen(winnerName);
        }
    }

    private void NewRound()
    {
        isOver = false;
        resetting = true;

        //Start 3 second delay and at the end reset the scene
        StartCoroutine(Reset());
    }

    private IEnumerator Reset()
    {
        /**
         * During delay
         **/

        //Reset move box position
        MoveBox moveBox = GameObject.Find("MoveBox").GetComponent<MoveBox>();
        moveBox.ResetPosition();

        //Find player 1 and reset position
        if (GameObject.Find("Player 1") != null)
        {
            player1 = GameObject.Find("Player 1").GetComponent<NetworkedCorgiCharacter>();
            player1.NetworkFreeze();
        }

        //Find player 2 and reset position
        if (GameObject.Find("Player 2") != null)
        {
            player2 = GameObject.Find("Player 2").GetComponent<NetworkedCorgiCharacter>();
            player2.NetworkFreeze();
        }
        yield return new WaitForSeconds(4);

        /**
         * After delay
         **/

        if (player1 != null)
        {
            player1.ResetPosition();
        }
        if (player2 != null)
        {
            player2.ResetPosition();
        }
        resetting = false;
    }
}
