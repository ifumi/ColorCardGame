using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WaitingBehaviour : NetworkBehaviour
{
    // The UI element displaying the connected players
    public WaitingPlayersPanel panel;
    public WaitingPlayercountText text;

    [ClientRpc]
    public void RpcSetConnectedPlayers(int count, string[] names)
    {
        panel.UpdatePlayersPanel(count, names);
        text.SetPlayerCount(count);
    }
}
