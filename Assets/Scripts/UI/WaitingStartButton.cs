using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingStartButton : MonoBehaviour
{
    public void StartGame()
    {
        FindObjectOfType<CustomNetworkManager>().ServerChangeScene("MainScene");
    }
}
