using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoingameCard : MonoBehaviour
{
    private Text cardText;
    private LanConnnectionInfo game;

    public LanConnnectionInfo GetGameInfo()
    {
        return game;
    }

    private void Awake()
    {
        cardText = GetComponentInChildren<Text>();
    }

    public void Initialize(LanConnnectionInfo newGame, Transform panelTransform)
    {
        this.game = newGame;
        cardText.text = game.name;
        transform.SetParent(panelTransform);
        GetComponent<RectTransform>().anchoredPosition = new Vector3(GameObject.Find("GameController").GetComponent<ScrollRectSnap>().GetNextPosition(), 0, 0);
        transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
    }

    public void SetName(string name)
    {
        GetComponentInChildren<Text>().text = name.ToUpper();
    }

    public string GetName()
    {
        return GetComponentInChildren<Text>().text;
    }
}
