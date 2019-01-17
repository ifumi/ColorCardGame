using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsNameText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Text name = gameObject.GetComponent<Text>();
        name.text = PlayerPrefs.GetString("Name");
    }
}
