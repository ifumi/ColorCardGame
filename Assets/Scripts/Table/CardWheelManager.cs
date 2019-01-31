using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardWheelManager : MonoBehaviour
{
    public StaticCardWheel player2, player3, player4;

    public void SetCards(int playerCount, int[] cardCounts, int myPlayerIndex)
    {
        switch (playerCount)
        {
            case 4:
                player4.SetCardCount(cardCounts[(myPlayerIndex + 3) % playerCount]);
                player4.RotateWheel();
                goto case 3;
            case 3:
                player3.SetCardCount(cardCounts[(myPlayerIndex + 2) % playerCount]);
                player3.RotateWheel();
                goto case 2;
            case 2:
                player2.SetCardCount(cardCounts[(myPlayerIndex + 1) % playerCount]);
                player2.RotateWheel();
                break;
        }
    }

    public void SpawnWheels(int count)
    {
        // Disable all unneccessary
        switch(count)
        {
            case 1:
                DisableChildren(player2.gameObject);
                goto case 2;
            case 2:
                DisableChildren(player3.gameObject);
                goto case 3;
            case 3:
                DisableChildren(player4.gameObject);
                break;
        }

        // Enable all
        switch(count)
        {
            case 4:
                EnableChildren(player4.gameObject);
                goto case 3;
            case 3:
                EnableChildren(player3.gameObject);
                goto case 2;
            case 2:
                EnableChildren(player2.gameObject);
                break;
        }

    }

    public void DisableChildren(GameObject obj)
    {
        foreach(Transform child in obj.transform)
        {
            child.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    public void EnableChildren(GameObject obj)
    {
        foreach (Transform child in obj.transform)
        {
            child.GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}
