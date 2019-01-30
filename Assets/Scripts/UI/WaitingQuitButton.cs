using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class WaitingQuitButton : MonoBehaviour
{
    public void CancelConnection()
    {
        FindObjectOfType<CustomNetworkManager>().StopConnection(); // Stop hosting

        GameObject networkManager = GameObject.Find("NetworkManager");
        if (networkManager != null)
        {
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
