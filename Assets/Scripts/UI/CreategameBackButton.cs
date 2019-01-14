using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreategameBackButton : MonoBehaviour
{
    public void BackButtonClicked()
    {
        SceneManager.LoadScene("MultiplayerScene");
    }
}
