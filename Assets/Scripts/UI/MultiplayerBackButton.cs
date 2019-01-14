using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiplayerBackButton : MonoBehaviour
{
    public void BackButtonClicked()
    {
        SceneManager.LoadScene("StartupScene");
    }
}
