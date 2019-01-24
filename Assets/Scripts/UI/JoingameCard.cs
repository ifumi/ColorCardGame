using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoingameCard : MonoBehaviour
{
    private Text gameNameText;
    private Text hostNameText;

    private LanConnnectionInfo game;

    public LanConnnectionInfo GetGameInfo()
    {
        return game;
    }

    private void Awake()
    {
        foreach (Transform t in transform)
        {
            if (t.name == "GameName")
            {
                gameNameText = t.GetComponent<Text>();
            }
            else if (t.name == "HostName")
            {
                hostNameText = t.GetComponent<Text>();
            }
        }
    }

    public void Initialize(LanConnnectionInfo newGame, Transform panelTransform)
    {
        this.game = newGame;
        gameNameText.text = game.gameName;
        hostNameText.text = game.hostName;
        transform.SetParent(panelTransform);
        GetComponent<RectTransform>().anchoredPosition = new Vector3(GameObject.Find("GameController").GetComponent<ScrollRectSnap>().GetNextPosition(), 0, 0);
        transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
    }

}
