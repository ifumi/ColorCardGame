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
            FindObjectOfType<ConnectionDiscovery>().StartBroadcasting(infield.text); // Broadcast my game name
            FindObjectOfType<CustomNetworkManager>().StartHosting(); // Start as Host

            SceneManager.LoadScene("WaitingScene");
        }
    }
}
