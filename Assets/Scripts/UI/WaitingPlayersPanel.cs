using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitingPlayersPanel : MonoBehaviour
{
    public Sprite player1Active;
    public Sprite player1Inactive;
    public Sprite player2Active;
    public Sprite player2Inactive;
    public Sprite player3Active;
    public Sprite player3Inactive;
    public Sprite player4Active;
    public Sprite player4Inactive;

    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public GameObject player4;

    public void UpdatePlayersPanel(int connectedPlayers, string[] playerNames)
    {
        // Set all inactive
        player1.GetComponentInChildren<Image>().sprite = player1Inactive;
        player2.GetComponentInChildren<Image>().sprite = player2Inactive;
        player3.GetComponentInChildren<Image>().sprite = player3Inactive;
        player4.GetComponentInChildren<Image>().sprite = player4Inactive;

        player1.GetComponentInChildren<Text>().text = "";
        player2.GetComponentInChildren<Text>().text = "";
        player3.GetComponentInChildren<Text>().text = "";
        player4.GetComponentInChildren<Text>().text = "";


        switch (connectedPlayers)
        {
            case 4:
                player4.GetComponentInChildren<Image>().sprite = player4Active;
                player4.GetComponentInChildren<Text>().text = playerNames[3];
                goto case 3;
            case 3:
                player3.GetComponentInChildren<Image>().sprite = player3Active;
                player3.GetComponentInChildren<Text>().text = playerNames[2];
                goto case 2;
            case 2:
                player2.GetComponentInChildren<Image>().sprite = player2Active;
                player2.GetComponentInChildren<Text>().text = playerNames[1];
                goto case 1;
            case 1:
                player1.GetComponentInChildren<Image>().sprite = player1Active;
                player1.GetComponentInChildren<Text>().text = playerNames[1];
                break;
            default:
                break;
        }
    }
}
