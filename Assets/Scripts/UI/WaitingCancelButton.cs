﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class WaitingCancelButton : MonoBehaviour
{

    public void CancelHosting()
    {
        // Stop Hosting
        FindObjectOfType<ConnectionDiscovery>().Stop(); // Stop broadcasting game name
        FindObjectOfType<CustomNetworkManager>().StopHosting(); // Stop hosting

        GameObject networkManager = GameObject.Find("NetworkManager");
        GameObject player = GameObject.Find("Player");
        if (networkManager != null)
        {
            player.GetComponent<Player>().ResetAllValues();
            Destroy(networkManager);
            Destroy(player);
            NetworkManager.Shutdown();
        }
            
        SceneManager.LoadScene("CreategameScene");
    }


}
