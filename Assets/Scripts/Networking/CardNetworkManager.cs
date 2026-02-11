using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class CardNetworkManager : NetworkManager
{
    private bool gameStarted = false;
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        Debug.Log("OnServerAddPlayer called. Current players: " + numPlayers);


        if (numPlayers >= 2)
        {
            Debug.Log("Already Filled Check: " + numPlayers);

            return;

        }
        GameObject player = Instantiate(playerPrefab);
        NetworkServer.AddPlayerForConnection(conn, player);

        Debug.Log("Player added: " + numPlayers);


        if (MenuHandler.IsTesting && !gameStarted)
        {
            gameStarted = true;
            Debug.Log("Two players connected. Starting game...");
            ServerChangeScene("Game");
            return;
        }

        if (numPlayers == 2 && !gameStarted)
        {
            gameStarted = true;
            Debug.Log("Two players connected. Starting game...");
            ServerChangeScene("Game");
        }
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        Debug.Log("Server started. Waiting for players...");

    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        Debug.Log("Server stopped.");
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        Debug.Log("Connected to server.");
    }
}
