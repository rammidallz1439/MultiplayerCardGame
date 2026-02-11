using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using Vault;

public class TurnSync : MonoBehaviour
{
    public static TurnSync Instance;

    private HashSet<string> readyPlayers = new HashSet<string>();

    private void Awake()
    {
        Instance = this;
    }

    public void OnPlayerEndedTurn(string json)
    {
        EndTurnMessage msg =
            JsonConvert.DeserializeObject<EndTurnMessage>(json);

        readyPlayers.Add(msg.playerId);

        Debug.Log("Player ready: " + msg.playerId);

        if (readyPlayers.Count == 2)
        {
            readyPlayers.Clear();
            ResolveTurn();
        }
    }

    void ResolveTurn()
    {
        Debug.Log("Both players ready. Resolving turn.");

        GameEvents.AllPlayersReady?.Invoke();

        // Example reveal message
        RevealCardMessage reveal = new RevealCardMessage
        {
            playerId = "P1",
            cardId = 1,
            orderIndex = 0
        };

        NetworkMessageRouter.Instance.SendMessage(reveal);
    }
}
