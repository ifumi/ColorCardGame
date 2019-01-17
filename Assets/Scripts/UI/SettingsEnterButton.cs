using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsEnterButton : MonoBehaviour
{
    public void SettingsEnterClicked()
    {
        Text playername = GameObject.Find("InputFieldName").GetComponentInChildren<Text>();

        if (playername.text != "")
        {
            PlayerPrefs.SetString("Name", playername.text);
            PlayerPrefs.Save();
            SceneManager.LoadScene("StartupScene");
        }
    }
}
