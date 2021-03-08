using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager
{
    private static int myId = -1;
    private static int gameId = -1;
    public static string username { get; set; }
    public static string ip { get; set; } = "192.168.1.187";

    public static void HandlePackage(Package package)
    {
        switch (package.packageId)
        {
            case PackageType.Welcome:
                break;
            case PackageType.WelcomeReceived:
                myId = package.playerId;
                Debug.Log($"My id is {myId}");
                break;
            default:
                break;
        }
    }

    public static int GetMyId()
    {
        return myId;
    }

    public static int GetGameId()
    {
        return gameId;
    }
}
