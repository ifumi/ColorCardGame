using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HostingText : MonoBehaviour
{
    public static void SetNotHosting()
    {
        Text hostingText = GameObject.Find("Text_Connections").GetComponent<Text>();
        hostingText.text = "Not Hosting";
    }

    public static void SetConnections(int conn)
    {
        Text hostingText = GameObject.Find("Text_Connections").GetComponent<Text>();
        hostingText.text = "Hosting. " + conn + "/4 players connected.";
    }
}
