using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameListPanel : MonoBehaviour
{

    [SerializeField]
    private JoingameCard joinCardPrefab;

    public ScrollRectSnap scrollRect;

    private void Awake()
    {
        AvailableGamesList.OnAvailableMatchesChanged += AvailableGamesList_OnAvailableMatchesChanged;    
    }

    private void OnDestroy()
    {
        AvailableGamesList.OnAvailableMatchesChanged -= AvailableGamesList_OnAvailableMatchesChanged;
    }

    private void AvailableGamesList_OnAvailableMatchesChanged(List<LanConnnectionInfo> games)
    {
        ClearExistingCards();
        CreateNewCards(games);
    }

    private void ClearExistingCards()
    {
        var cards = GetComponentsInChildren<JoingameCard>();
        if (cards == null) return;
        foreach (var card in cards)
        {
            Destroy(card.gameObject);
        }
        scrollRect.ClearLists();
    }

    private void CreateNewCards(List<LanConnnectionInfo> games)
    {
        foreach(var game in games)
        {
            var parent = GameObject.Find("ScrollPanel").transform;
            JoingameCard card = Instantiate(joinCardPrefab);
            card.Initialize(game, parent);
            scrollRect.AddImage(card.gameObject.GetComponent<Image>());
        }
    }
}
