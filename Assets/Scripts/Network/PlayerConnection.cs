using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class PlayerConnection : NetworkBehaviour
{
    private GameObject currentCard;
    private Player player;

    private Stack<ColorCard> SpawnedCards;

    // Start is called before the first frame update
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        if (sceneName == "MainScene")
        {
            // UI: Find our wheel and current card display
            player = GameObject.Find("Player").GetComponent<Player>();
            currentCard = GameObject.Find("CurrentCard");

            if (isServer)
            {
                // Spawn tables
                CmdSpawnTables();

                // Initialize cards on game board
                SpawnedCards = new Stack<ColorCard>();

                // Spawn initial card
                ColorCard cc;
                do
                {
                    cc = ColorCardStack.DrawCard();
                    CmdSpawnCardOnGameboard(cc);
                } while (cc.type != ColorCard.Type.STANDARD);

                // Start game
                player.SetCurrentPlayerIndex(0); // Server starts always first
                player.SetTurn(true);
            }

            if (!isLocalPlayer)
            {
                // This object belongs to another player
                return;
            }
            else
            {
                // Set reference for the player so we can play cards
                player.SetConnection(this);

                // Tell server to give us our initial set of cards
                CmdGetInitialCards();
            }          
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {

        }

        if (isServer)
        {

        }
    }


    private bool ClientsReady()
    {
        // Wait for all the clients to be ready
        foreach (NetworkConnection conn in NetworkServer.connections)
        {
            if (!conn.isReady) return false;
        }
        return true;
    }

    // -----------------------------------------
    // --- COMMANDS, RPCs and Network Stuff ----
    // -----------------------------------------

    [Command]
    public void CmdSpawnTables()
    {
        // Spawn tables
        int count = NetworkServer.connections.Count;

        player.SpawnTables(count);
        RpcSpawnTables(count);
    }

    [ClientRpc]
    void RpcSpawnTables(int count)
    {
        // We have to wait because player == null on clients when they first get the rpc
        StartCoroutine(SpawnTablesWhenReady(count));
    }

    public IEnumerator SpawnTablesWhenReady(int count)
    {
        yield return new WaitUntil(() => player != null);
        player.SpawnTables(count);
        yield return null;
    }


    [Command]
    public void CmdPlayCard(ColorCard c)
    {

        Debug.Log("Player " + connectionToClient.connectionId + " played Unocard: " + c.value + ", " + c.color + ", " + c.value);
        // Spawn the card on the board
        CmdSpawnCardOnGameboard(c);

        // Check if we have to do something (set reverse or set drawCount)
        int count = player.GetCurrentDrawCount();
        if (player.GetTopCard().type == ColorCard.Type.WILD4)
            count += 4;
        if (player.GetTopCard().type == ColorCard.Type.DRAW2)
            count += 2;
        if (player.GetTopCard().type == ColorCard.Type.REVERSE)
        {
            player.SetReverse(!player.GetReverse()); // toggle reverse

            if (NetworkServer.connections.Count == 2) // if its only 2 players we have to provide skip functionality
                CmdIncrementPlayerIndex();
        }
        if (player.GetTopCard().type == ColorCard.Type.SKIP) // Increment player by 1 more
            CmdIncrementPlayerIndex();

        player.SetCurrentDrawCount(count); // Save it on the server
    }

    [Command]
    public void CmdSetNextTurn()
    {
        CmdIncrementPlayerIndex(); // Set next player
        // Set next player active
        TargetSetPlayersTurn(NetworkServer.connections[player.GetCurrentPlayerIndex()], true, player.GetCurrentDrawCount());
    }

    [TargetRpc]
    void TargetSetPlayersTurn(NetworkConnection conn, bool turn, int currentDrawCount)
    {
        // We cannot create a bool in this object to hold this information (I DONT KNOW WHY)
        // But we can manipulate our local player object and control the game over that
        player.SetTurn(turn, currentDrawCount);
    }

    [Command]
    public void CmdChangeTopColor(ColorCard.Color color)
    {
        ColorCard c = player.GetTopCard();
        c.color = color;
        player.SetTopCard(c);

        RpcSetTopColor(color); 
    }

    [Command]
    public void CmdIncrementPlayerIndex()
    {
        ReadOnlyCollection<NetworkConnection> players = NetworkServer.connections; // All connections
        int idx = player.GetCurrentPlayerIndex();
        do
        {
            if (!player.GetReverse())
            {
                idx = (idx + 1) % players.Count;
            }        
            else
            {
                idx -= 1;
                if (idx < 0)
                    idx = players.Count - 1;
            }
        } while (players[idx] == null); // This loop is to check for disconnected clients which will be null, then we need to increment even further

        player.SetCurrentPlayerIndex(idx);
    }

    /// <summary>
    /// Request 7 cards from the card stack (server).
    /// Gives them to the calling client (target RPC)
    /// </summary>
    [Command]
    void CmdGetInitialCards()
    {     
        List<ColorCard> list = ColorCardStack.DrawCardHand();
        TargetTakeCards(connectionToClient, list.ToArray()); // Convert to array because the rpc cannot handle lists.
    }

    /// <summary>
    /// Response to the calling client with the requested cards.
    /// Take the cards provided by the server
    /// </summary>
    /// <param name="conn">The connection of the calling client.</param>
    /// <param name="cards">The cards provided from the server.</param>
    [TargetRpc]
    void TargetTakeCards(NetworkConnection conn, ColorCard[] cards)
    {   
        foreach (ColorCard c in cards)
        {
            // Add to our list
            player.AddCard(c);
        }

        // UI: Move the wheel to the center position
        player.RotateWheel();
    }

    /// <summary>
    /// Request a single card from the servers stack.
    /// </summary>
    [Command]
    public void CmdGetCard()
    {
        ColorCard cc = ColorCardStack.DrawCard();
        if (cc.Equals(default(ColorCard)))
        {
            // Stack is empty --> Reshuffle
            ColorCardStack.ReshuffleStack(SpawnedCards);
            cc = ColorCardStack.DrawCard();
        }

        TargetTakeCard(connectionToClient, cc);

        if (player.GetCurrentDrawCount() > 0)
            player.SetCurrentDrawCount(player.GetCurrentDrawCount() - 1);


    }

    /// <summary>
    /// Response to the calling client with the requested card .
    /// Take the card provided by the server
    /// </summary>
    /// <param name="conn">The connection of the calling client.</param>
    /// <param name="card">The cards provided from the server.</param>
    [TargetRpc]
    void TargetTakeCard(NetworkConnection conn, ColorCard card)
    {
        // Add to our list
        player.AddCard(card);
    }


    /// <summary>
    /// Spawns the given card on the game board.
    /// </summary>
    /// <param name="card">The card to spawn.</param>
    [Command]
    void CmdSpawnCardOnGameboard(ColorCard card)
    {
        // Add random angle here
        Quaternion newAngle = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-10.0f, 10.0f));
        currentCard.transform.rotation = newAngle; 
        // Set texture
        CardSpriteLoader ctl = currentCard.GetComponent<CardSpriteLoader>();
        ctl.SetSprite(card.type, card.color, card.value);

        //NetworkServer.Spawn(currentCard); // Spawn on network
        SpawnedCards.Push(card); // Store on server stack
        player.SetTopCard(card);
        
        // Propagate new card with texture and angle change also to all clients
        RpcSetTopCard(currentCard, card, newAngle);
    }


    /// <summary>
    /// Sets the texture of the given gameObject to the cards specific values on all clients.
    /// </summary>
    /// <param name="go">The gameobject to manipulate.</param>
    /// <param name="card">The card object with the values.</param>
    [ClientRpc]
    void RpcSetTopCard(GameObject go, ColorCard card, Quaternion angle)
    {
        // Set angle
        go.transform.rotation = angle;

        // Set texture
        CardSpriteLoader ctl = go.GetComponent<CardSpriteLoader>();
        ctl.SetSprite(card.type, card.color, card.value);

        // Save the top card to later check if this play is valid.
        player.SetTopCard(card);
    }

    [ClientRpc]
    void RpcSetTopColor(ColorCard.Color color)
    {
        ColorCard c = player.GetTopCard();
        c.color = color;
        player.SetTopCard(c);
    }



}
