using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    public void Show()
    {
        GetComponent<Canvas>().enabled = true;
    }

    public void Show(string winner)
    {
        SetWinner(winner);
        GetComponent<Canvas>().enabled = true;
    }

    private void SetWinner(string winner)
    {
        Text winnerText = GameObject.Find("WinnerText").GetComponent<Text>();
        winnerText.text = winner + "\nwins";
    }
}
