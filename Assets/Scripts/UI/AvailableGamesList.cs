using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class AvailableGamesList
{
    public static event Action<List<LanConnnectionInfo>> OnAvailableMatchesChanged = delegate { };

    private static List<LanConnnectionInfo> gamesList = new List<LanConnnectionInfo>();

    public static void HandleNewGamesList(List<LanConnnectionInfo> list)
    {
        gamesList = list;

        OnAvailableMatchesChanged(gamesList);
    }

}
