using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingStartButton : MonoBehaviour
{
    public void StartGame()
    {
        // We need at least 2 players to start the game
        if (FindObjectOfType<CustomNetworkManager>().GetConnectedClients().Count < 2)
            return;


        FindObjectOfType<ConnectionDiscovery>().Stop();
        FindObjectOfType<CustomNetworkManager>().ServerChangeScene("MainScene");
    }
}
