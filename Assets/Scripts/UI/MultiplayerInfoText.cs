using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerInfoText : MonoBehaviour
{
    public ScrollRectSnap scrollRect;

    // Update is called once per frame
    void Update()
    {
        switch(scrollRect.GetSelectedImage())
        {
            case 0:
                gameObject.GetComponent<Text>().text = "CREATE GAME";
                break;              
            case 1:
                gameObject.GetComponent<Text>().text = "JOIN GAME";
                break;
            default:
                break;
        }
    }
}
