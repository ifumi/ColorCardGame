using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class MainBackButton : MonoBehaviour
{

    public void BackClicked()
    {
        // Stop Hosting
        FindObjectOfType<ConnectionDiscovery>().Stop(); // Stop broadcasting game name
        FindObjectOfType<CustomNetworkManager>().StopHosting(); // Stop hosting
        FindObjectOfType<CustomNetworkManager>().StopConnection(); // Stop connecion

        GameObject networkManager = GameObject.Find("NetworkManager");
        if (networkManager != null)
        {
            // Reset all buffered values         
            Destroy(networkManager);
            NetworkManager.Shutdown();
        }
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            player.GetComponent<Player>().ResetAllValues();
            Destroy(player);
        }

        SceneManager.LoadScene("MultiplayerScene");
    }


}
