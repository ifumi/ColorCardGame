using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public CardWheel cardWheel;
    public ColorPicker colorPicker;
    public ColorIndicator colorIndicator;
    public TablesManager tablesManager;

    private ColorCard topCard;
    private List<ColorCard> MyCards;

    // For game logic
    private bool hasTurn = false;
    private int currentPlayerIndex;
    private int currentDrawCount;
    private bool reverse;

    private PlayerConnection connection;

    public void SetCurrentPlayerIndex(int idx)
    {
        currentPlayerIndex = idx;
    }

    public int GetCurrentPlayerIndex()
    {
        return currentPlayerIndex;
    }

    public void SetCurrentDrawCount(int count)
    {
        currentDrawCount = count;
    }

    public int GetCurrentDrawCount()
    {
        return currentDrawCount;
    }

    public void SetConnection(PlayerConnection conn)
    {
        connection = conn;
    }

    public void AddCard(ColorCard cc)
    {
        MyCards.Add(cc);
        cardWheel.AddCard(cc);
    }

    public void SetTopCard(ColorCard c)
    {
        topCard = c;
        colorIndicator.SetColor(c.color);
    }

    public List<ColorCard> GetCards()
    {
        return MyCards;
    }

    public ColorCard GetTopCard()
    {
        return topCard;
    }

    public void SetTurn(bool turn)
    {
        hasTurn = turn;
    }

    public void PlayCard(ColorCard card)
    {
        connection.CmdPlayCard(card); // send the card to the server
        if (card.type == ColorCard.Type.WILD || card.type == ColorCard.Type.WILD4)
        {
            // we first have to set the color by the user
            colorPicker.Show(); // show the color picker
            StartCoroutine(WaitForColorpickerToFinish(card)); // Wait for the colorpicker to finish before giving turn
        } else
        {           
            connection.CmdSetNextTurn();        
        }
        MyCards.Remove(card);
    }

    public void SetTurn(bool turn, int drawCount)
    {
        hasTurn = turn;

        if (!CanPlay())
        {
            if (drawCount != 0)
                currentDrawCount = drawCount; // a series of special cards was played. the server is telling us how much we have to draw
            else
                currentDrawCount = 1; // Draw 1 if we cannot play and there is no special card on top
        } else
        {
            if (drawCount != 0)
                currentDrawCount = drawCount;
        }
    }

    public void DrawCards()
    {
        // Start coroutine to wait until we get cards from the server
        StartCoroutine(WaitForCardsToArrive(currentDrawCount + MyCards.Count));

        while (currentDrawCount > 0)
        {
            connection.CmdGetCard(); // Get a card from the server.
            if (!connection.isServer) // If we are on the server the server will decrement the count for us
                currentDrawCount--;
        }       
    }

    private IEnumerator WaitForCardsToArrive(int targetCountCard)
    {
        yield return new WaitUntil(() => MyCards.Count == targetCountCard); // Wait til cards arrive

        if (!CanPlay())
        { // Cannot play after drawing, set next turn 
            hasTurn = false; // Remove turn permission first
            connection.CmdSetNextTurn();
        }
        yield return null;
    }

    private IEnumerator WaitForColorpickerToFinish(ColorCard playedCard)
    {
        yield return new WaitUntil(() => colorPicker.GetPickedColor() != ColorCard.Color.NONE); // Wait till user selects color

        playedCard.color = colorPicker.GetPickedColor();
        colorPicker.Reset();

        connection.CmdChangeTopColor(playedCard.color);
        connection.CmdSetNextTurn();
        MyCards.Remove(playedCard);
        colorPicker.Hide();
    }


    public bool HasTurn()
    {
        return hasTurn;
    }
    
    public bool CanPlay()
    {
        foreach(ColorCard c in MyCards)
        {
            if (ColorGame.CheckCardMatch(topCard, c)) return true;
        }
        return false;
    }

    public bool MustDrawBeforePlaying()
    {
        if (currentDrawCount > 0)
            return true;
        return false;
    }

    // Start is called before the first frame update
    void Awake()
    {
        MyCards = new List<ColorCard>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(hasTurn);
        Debug.Log(currentDrawCount);
        Debug.Log(topCard.color);
    }

    public void SpawnTables(int count)
    {
        tablesManager.SpawnTables(count);
    }

    public void RotateWheel()
    {
        cardWheel.RotateWheel();
    }

    public void SetReverse(bool reverse)
    {
        this.reverse = reverse;
    }

    public bool GetReverse()
    {
        return reverse;
    }
}
