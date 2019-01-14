using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateButton : MonoBehaviour
{
    private bool isHosting = false;

    public void CreateGame()
    {
            InputField infield = GameObject.Find("InputField_Name").GetComponent<InputField>();

            // Start Hosting
            FindObjectOfType<ConnectionDiscovery>().StartBroadcasting(infield.text); // Broadcast my game name
            FindObjectOfType<CustomNetworkManager>().StartHosting(); // Start as Host
    }
}
