using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JoingameEnterButton : MonoBehaviour
{
    public ScrollRectSnap gameScrollRect;

    public void EnterSelectedGame()
    {
        // Get selected game
        LanConnnectionInfo game = gameScrollRect.GetSelectedCard().GetGameInfo();

        SceneManager.LoadScene("WaitingScene");

        // Stop listening
        FindObjectOfType<ConnectionDiscovery>().Stop();
        // Connect to selected game
        FindObjectOfType<CustomNetworkManager>().StartConnection(game);
    }
}
