using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartupEnterButton : MonoBehaviour
{
    public ScrollRectSnap scrollRect;

    public void EnterButtonClicked()
    {
        switch (scrollRect.GetSelectedImage())
        {
            case 0:             
                break;
            case 1:
                SceneManager.LoadScene("MultiplayerScene");
                break;
            case 2:
                break;
            default:
                break;
        }
    }
}
