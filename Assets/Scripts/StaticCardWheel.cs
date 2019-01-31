using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticCardWheel : CardWheel
{
    // Start is called before the first frame update
    void Start()
    {
        isStatic = true;
    }

    public void SetCardCount(int count)
    {
        while (cardCount < count)
            AddCard(new ColorCard(ColorCard.Type.NONE, ColorCard.Color.NONE, -1));

        while (cardCount > count)
            RemoveFirstCard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
