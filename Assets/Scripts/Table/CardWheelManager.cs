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
                player3.SetCardCount(cardCounts[(myPlayerIndex + 3) % playerCount]);
                player3.RotateWheel();
                goto case 2;
            case 2:
                player2.SetCardCount(cardCounts[(myPlayerIndex + 3) % playerCount]);
                player2.RotateWheel();
                break;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
