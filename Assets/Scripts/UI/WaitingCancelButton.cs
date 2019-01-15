using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaitingCancelButton : MonoBehaviour
{

    public void CancelHosting()
    {
        // Stop Hosting
        FindObjectOfType<ConnectionDiscovery>().Stop(); // Stop broadcasting game name
        FindObjectOfType<CustomNetworkManager>().StopHosting(); // Stop hosting

        SceneManager.LoadScene("CreategameScene");
    }


}
