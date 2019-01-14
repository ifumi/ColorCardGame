using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitingPlayercountText : MonoBehaviour
{
    public void SetPlayerCount(int count)
    {
        Text countText = gameObject.GetComponent<Text>();
        countText.text = count + " / 4";
    }
}
