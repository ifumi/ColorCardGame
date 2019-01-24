using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class JoingameBackButton : MonoBehaviour
{
    public void BackButtonClicked()
    {
        FindObjectOfType<ConnectionDiscovery>().Stop(); // Stop listening for games

        GameObject networkManager = GameObject.Find("NetworkManager");
        if (networkManager != null)
        {
            Destroy(networkManager);
            NetworkManager.Shutdown();
        }
            

        SceneManager.LoadScene("MultiplayerScene");
    }
}
