using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class WaitingBehaviour : MonoBehaviour
{

    PlayerConnection localPlayer;
    private Player player;
    private WaitingPlayersPanel waitingPlayersPanel;

    // Start is called before the first frame update
    void Start()
    {
        // Find local player object
        PlayerConnection[] allPlayers = GameObject.FindObjectsOfType<PlayerConnection>();

        foreach(PlayerConnection player in allPlayers)
        {
            if (player.isLocalPlayer)
                localPlayer = player;
        }

        player = GameObject.Find("Player").GetComponent<Player>();
        waitingPlayersPanel = GameObject.Find("PlayersPanel").GetComponent<WaitingPlayersPanel>();

        // localPlayer.CmdRegisterPlayer(PlayerPrefs.GetString("Name"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
