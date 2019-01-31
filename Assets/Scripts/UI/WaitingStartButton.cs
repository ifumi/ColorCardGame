using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingStartButton : MonoBehaviour
{
    public void StartGame()
    {
        FindObjectOfType<ConnectionDiscovery>().Stop();
        FindObjectOfType<CustomNetworkManager>().ServerChangeScene("MainScene");
    }
}
