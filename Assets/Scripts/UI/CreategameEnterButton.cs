using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreategameEnterButton : MonoBehaviour
{
    public void CreateGame()
    {
        InputField infield = GameObject.Find("InputFieldName").GetComponent<InputField>();
        if (infield.text != "")
        {
            // Start Hosting
            string playerName = PlayerPrefs.GetString("Name");
            if (playerName == "")
                playerName = "Unknown";

            SceneManager.LoadScene("WaitingScene");

            FindObjectOfType<ConnectionDiscovery>().StartBroadcasting(infield.text, playerName); // Broadcast my game and host name
            FindObjectOfType<CustomNetworkManager>().StartHosting(); // Start as Host
        }
    }
}
