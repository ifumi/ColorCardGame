using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public CardWheel cardWheel;
    public ColorPicker colorPicker;
    public ColorIndicator colorIndicator;
    public DrawIndicator drawIndicator;

    public GameOverPanel gameOverPanel;

    public TablesManager tablesManager;
    public CardWheelManager cardWheelManager;

    private ColorCard topCard;
    private List<ColorCard> MyCards;

    // For game synchronisation
    public static string[] playerNames = new string[4];
    public static int connectedPlayers = 0;
    public static int myPlayerIndex;

    public static int[] playerCardsCount = new int[4]; // TODO
    public static bool[] playersReady = new bool[] {false, false, false, false};

    // For game logic
    private bool hasTurn = false;
    public bool isGameOver = false;
    private int currentPlayerIndex;
    private int currentDrawCount;
    private bool reverse;

    private PlayerConnection connection;
    private WaitingPlayersPanel waitingPlayersPanel;

    public void ResetAllValues()
    {
        playerNames = new string[4];
        connectedPlayers = 0;
        myPlayerIndex = 0;
        playerCardsCount = new int[4];
        playersReady = new bool[] { false, false, false, false };
    }

    public void SetPlayerReady(int index)
    {
        playersReady[index] = true;
    }

    public bool AllPlayersReady()
    {
        for (int i = 0; i < connectedPlayers; i++)
        {
            if (playersReady[i] == false) return false;
        }
        return true;
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "WaitingScene")
            waitingPlayersPanel = GameObject.Find("PlayersPanel").GetComponent<WaitingPlayersPanel>();           
    }

    public void AddPlayer(string name)
    {
        playerNames[connectedPlayers] = name;
        connectedPlayers++;

        if (waitingPlayersPanel != null)
            waitingPlayersPanel.UpdatePlayersPanel(connectedPlayers, playerNames);
    }

    public void SetPlayers(string[] names, int count)
    {
        playerNames = names;
        connectedPlayers = count;

        if (waitingPlayersPanel != null)
            waitingPlayersPanel.UpdatePlayersPanel(connectedPlayers, playerNames);

        foreach (string name in names)
        {
            if (name == PlayerPrefs.GetString("Name"))
            {
                myPlayerIndex = Array.IndexOf(names, name);
            }
        }

        if (tablesManager != null)
            tablesManager.SpawnTables(count, names, myPlayerIndex);
    }

    public void SetCurrentPlayerIndex(int idx)
    {
        currentPlayerIndex = idx;
        tablesManager.SetTableActive(playerNames[idx]);
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

    public bool HasWon()
    {
        return MyCards.Count == 0;
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

        // Check if we have to show the draw indicator
        if (hasTurn && MustDrawBeforePlaying())
        {
            drawIndicator.Show();
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
        {
            if (topCard.type == ColorCard.Type.WILD4 && HasCardOfType(ColorCard.Type.WILD4))
            {
                return false;
            }
            else if (topCard.type == ColorCard.Type.DRAW2 && HasCardOfType(ColorCard.Type.DRAW2))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        return false;
    }

    private bool HasCardOfType(ColorCard.Type type)
    {
        foreach(ColorCard card in MyCards)
        {
            if (card.type == type) return true;
        }
        return false;
    }

    // Start is called before the first frame update
    void Awake()
    {
        MyCards = new List<ColorCard>();
    }

    public void SpawnTables(int count, string[] names, int myPlayerIndex)
    {
        tablesManager.SpawnTables(count, names, myPlayerIndex);
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

    public void SetCardsCount(int[] count)
    {
        playerCardsCount = count;
        cardWheelManager.SetCards(connectedPlayers, playerCardsCount, myPlayerIndex);
    }

    public void SetWinner(int index)
    {
        gameOverPanel.Show(playerNames[index]);
        isGameOver = true;
    }
}
