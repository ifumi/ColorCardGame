using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsBackButton : MonoBehaviour
{
    public void SettingsBackClicked()
    {
        SceneManager.LoadScene("StartupScene");
    }
}
