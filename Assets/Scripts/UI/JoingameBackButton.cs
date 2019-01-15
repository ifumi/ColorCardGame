using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JoingameBackButton : MonoBehaviour
{
    public void BackButtonClicked()
    {
        FindObjectOfType<ConnectionDiscovery>().Stop(); // Stop listening for games
        SceneManager.LoadScene("MultiplayerScene");
    }
}
