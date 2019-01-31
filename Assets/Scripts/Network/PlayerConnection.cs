using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerConnection : NetworkBehaviour
{
    private GameObject currentCard;

    private Player player;

    private Stack<ColorCard> SpawnedCards;

    private bool isRegistered = false;
    private bool isStarted = false;

    private CustomNetworkManager networkManager;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        
        networkManager = FindObjectOfType<CustomNetworkManager>();

        if (isLocalPlayer)
            networkManager.SetServerConnection(this);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
    {
        switch (scene.name)
        {
            case "MainScene":
                if (!isStarted)
                {

                }
                break;
        }
    }

    private void Update()
    {
        // Register when entering waiting scene
        if (SceneManager.GetActiveScene().name == "WaitingScene" && isLocalPlayer && !isRegistered)
        {
            player = GameObject.Find("Player").GetComponent<Player>();
            player.SetConnection(this);

            string playername = PlayerPrefs.GetString("Name");
            if (playername == "")
                playername = "Unknown";
            
            CmdRegisterPlayer(playername);
            isRegistered = true;         
        }

        // Start game when entering main scene
        if (SceneManager.GetActiveScene().name == "MainScene" && !isStarted)
        {                    
            // UI: Find current card display
            player = GameObject.Find("Player").GetComponent<Player>();
            currentCard = GameObject.Find("CurrentCard");

            if (player == null || currentCard == null) return; // Scene not fully loaded

            if (isServer)
            {
                // Initialize cards on game board
                SpawnedCards = new Stack<ColorCard>();
            }

            if (isLocalPlayer)
            {
                // Set reference for the player so we can play cards
                player.SetConnection(this);

                // Tell server to give us our initial set of cards
                CmdSignalPlayerReady();
                CmdGetInitialCards();
            }

            isStarted = true;
        }


        if (player == null)
        {
            GameObject p = GameObject.Find("Player");
            if (p != null) player = p.GetComponent<Player>();
        }
    }
    // -----------------------------------------
    // --- COMMANDS, RPCs and Network Stuff ----
    // -----------------------------------------

    [Command]
    public void CmdSetWinner(int index)
    {
        // Show UI
        player.SetWinner(index);
        RpcSetWinner(index);
    }

    [ClientRpc]
    public void RpcSetWinner(int index)
    {
        // Show UI
        player.SetWinner(index);
    }

    [Command]
    public void CmdSignalPlayerReady()
    {
        int index = networkManager.GetConnectedClients().IndexOf(connectionToClient);
        player.SetPlayerReady(index);

        // Start game
        if(player.AllPlayersReady())
        {
            // Spawn tables
            CmdSpawnTables();

            // Spawn initial card
            ColorCard cc;
            do
            {
                cc = ColorCardStack.DrawCard();
                CmdSpawnCardOnGameboard(cc);
            } while (cc.type != ColorCard.Type.STANDARD);

            // Init card count for each player
            Player.playerCardsCount = new int[] { 7, 7, 7, 7 };
            RpcSetPlayerCardsCount(Player.playerCardsCount); // Sync with clients

            // Start game
            player.SetCurrentPlayerIndex(0); // Server starts always first
            player.SetTurn(true, 0);
            RpcPlayerIndexChanged(0);
        }
    }

    [Command]
    public void CmdUnregisterPlayer(int index)
    {
        // Remove name from name array
        string[] names = Player.playerNames;
        for (int i = index; i < names.Length; i++)
        {
            if (index < names.Length - 1)
                names[index] = names[index + 1];
            else
                names[index] = "";
        }

        if (!player.isGameOver)
        {
            // Save new player configuration
            player.SetPlayers(names, Player.connectedPlayers - 1);
            // Notify clients
            RpcConnectedPlayersUpdate(Player.playerNames, Player.connectedPlayers);
        }
    }

    [Command]
    public void CmdRegisterPlayer(string playerName)
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        if (Player.connectedPlayers < 4)
        {
            player.AddPlayer(playerName);
            RpcConnectedPlayersUpdate(Player.playerNames, Player.connectedPlayers);

            Debug.Log("Register player: " + Player.connectedPlayers + ", " + playerName);
        }
    }

    [ClientRpc]
    void RpcConnectedPlayersUpdate(string[] names, int players)
    {
        if (SceneManager.GetActiveScene().name == "WaitingScene" || SceneManager.GetActiveScene().name == "MainScene")
        {
            Debug.Log("SetPlayers Update: " + players );
            // UPDATE OUR UI
            if (player == null)
                player = GameObject.Find("Player").GetComponent<Player>();
            player.SetPlayers(names, players);
        }
    }

    [Command]
    public void CmdSpawnTables()
    {
        // Spawn tables
        player.SpawnTables(Player.connectedPlayers, Player.playerNames, Player.myPlayerIndex);
        RpcSpawnTables();
    }

    [ClientRpc]
    void RpcSpawnTables()
    {
        // We have to wait because player == null on clients when they first get the rpc
        player.SpawnTables(Player.connectedPlayers, Player.playerNames, Player.myPlayerIndex);
    }

    [Command]
    public void CmdPlayCard(ColorCard c)
    {
        Debug.Log("Player " + connectionToClient.connectionId + " played Unocard: " + c.value + ", " + c.color + ", " + c.value);

        // Decrement card count for playing player
        int playerIdx = networkManager.GetConnectedClients().IndexOf(connectionToClient);
        Player.playerCardsCount[playerIdx] -= 1;
        player.SetCardsCount(Player.playerCardsCount);

        // Sync new card count with all clients
        RpcSetPlayerCardsCount(Player.playerCardsCount);

        // Spawn the card on the board
        CmdSpawnCardOnGameboard(c);

        // Check win
        if (Player.playerCardsCount[playerIdx] == 0)
        {
            // Player won
            CmdSetWinner(playerIdx);
            return;
        }


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

    [ClientRpc]
    public void RpcSetPlayerCardsCount(int[] count)
    {
        player.SetCardsCount(count);
        Debug.Log("Update: " + count[0] + ", " + count[1] + ", " + count[2] + ", " + count[3]);
    }

    [Command]
    public void CmdSetNextTurn()
    {
        CmdIncrementPlayerIndex(); // Set next player
        
        RpcPlayerIndexChanged(player.GetCurrentPlayerIndex()); // Update Tables
     
        TargetSetPlayersTurn(networkManager.GetConnectedClients()[player.GetCurrentPlayerIndex()], true, player.GetCurrentDrawCount()); // Set next player active
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
        int idx = player.GetCurrentPlayerIndex();

        if (!player.GetReverse())
        {
            idx = (idx + 1) % Player.connectedPlayers;
        }        
        else
        {
            idx -= 1;
            if (idx < 0)
                idx = Player.connectedPlayers - 1;
        }
        player.SetCurrentPlayerIndex(idx);
    }

    [ClientRpc]
    public void RpcPlayerIndexChanged(int idx)
    {
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

        int index = networkManager.GetConnectedClients().IndexOf(connectionToClient);
        Player.playerCardsCount[index] += 1;
        player.SetCardsCount(Player.playerCardsCount);

        // Sync new card count with all clients
        RpcSetPlayerCardsCount(Player.playerCardsCount);
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
