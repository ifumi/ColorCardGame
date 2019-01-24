using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class CustomNetworkManager : NetworkManager
{
    private List<NetworkConnection> connectedClients;

    public bool isHosting = false;
    public bool isConnected = false;

    private int connectedPlayers = 0;

    void SetPort(int port)
    {
        NetworkManager.singleton.networkPort = port;
    }

    void SetIP(string ip)
    {
        NetworkManager.singleton.networkAddress = ip;
    }

    /// <summary>
    /// Start hosting a new game
    /// </summary>
    public void StartHosting()
    {
        if (!isHosting)
        {
            connectedClients = new List<NetworkConnection>();
            NetworkManager.singleton.StartHost();
            isHosting = true;
        }
    }

    /// <summary>
    /// Stop hosting the game
    /// </summary>
    public void StopHosting()
    {
        if (isHosting)
        {
            NetworkManager.singleton.StopHost();
            isHosting = false;
        }
    }

    /// <summary>
    /// Start connection as a client to specified LanConnection.
    /// </summary>
    /// <param name="info">Connection infos like IP and port.</param>
    public void StartConnection(LanConnnectionInfo info)
    {
        if (!isConnected)
        {
            SetIP(info.ipAddress);
            SetPort(LanConnnectionInfo.PORT);
            NetworkManager.singleton.StartClient();
            isConnected = true;
        }
    }

    /// <summary>
    /// Stop connection as a client.
    /// </summary>
    public void StopConnection()
    {
        if (isConnected)
        {
            NetworkManager.singleton.StopClient();
            isConnected = false;
        }
    }

    /// <summary>
    /// Called on the server when a new client connects
    /// </summary>
    /// <param name="conn"></param>
    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
        Debug.Log("New client " + conn.address + " connected!");
        connectedPlayers++;

        connectedClients.Add(conn);
    }

    /// <summary>
    /// Called on the server when a client disconnects
    /// </summary>
    /// <param name="conn"></param>
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
        Debug.Log("Client " + conn.address + " disconnected!");
        connectedPlayers--;

        connectedClients.Remove(conn);

        // Tell clients that the server state changed
        if (SceneManager.GetActiveScene().name == "WaitingScene")
        {
            
        }
    }

    /// <summary>
    /// Called on the client when connected to the server.
    /// </summary>
    /// <param name="conn"></param>
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

    }

    /// <summary>
    /// Called on the client when disconnected from the server.
    /// </summary>
    /// <param name="conn"></param>
    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);

        // Switch to Lobby if client was disconnected while in game scene
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if (sceneName == "MainScene")
        {
            StopConnection();
            StopHosting();
        }     
    }

    public void StartGame()
    {
        if (connectedPlayers >= 1)
        {
            // if more than 2 players are connected
            NetworkManager.singleton.ServerChangeScene("MainScene"); // change to main game scene
            FindObjectOfType<ConnectionDiscovery>().Stop();
        }
    }
    
}
