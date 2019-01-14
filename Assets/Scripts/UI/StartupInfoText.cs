using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartupInfoText : MonoBehaviour
{
    public ScrollRectSnap scrollRect;

    // Update is called once per frame
    void Update()
    {
        switch (scrollRect.GetSelectedImage())
        {
            case 0:
                gameObject.GetComponent<Text>().text = "SINGLEPLAYER";
                break;
            case 1:
                gameObject.GetComponent<Text>().text = "MULTIPLAYER";
                break;
            case 2:
                gameObject.GetComponent<Text>().text = "SETTINGS";
                break;
            default:
                break;
        }
    }
}
