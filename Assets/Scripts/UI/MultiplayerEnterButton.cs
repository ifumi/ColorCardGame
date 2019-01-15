using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiplayerEnterButton : MonoBehaviour
{
    public ScrollRectSnap scrollRect;

    public void EnterButtonClicked()
    {
        switch (scrollRect.GetSelectedImage())
        {
            case 0:
                SceneManager.LoadScene("CreategameScene");
                break;
            case 1:
                FindObjectOfType<ConnectionDiscovery>().StartListening(); // Start listening for games
                SceneManager.LoadScene("JoingameScene");
                break;
            default:
                break;
        }
    }
}
