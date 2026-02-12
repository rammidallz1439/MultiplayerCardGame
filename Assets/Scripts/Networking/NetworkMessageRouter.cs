using Newtonsoft.Json;
using UnityEngine;
using Vault;
using Mirror;

public class NetworkMessageRouter : NetworkBehaviour
{
    public static NetworkMessageRouter Instance;

    private CardNetworkPlayer localPlayer;

    private void Awake()
    {
        Instance = this;
    }

    public void SetLocalPlayer(CardNetworkPlayer player)
    {
        localPlayer = player;
    }

    public string GetLocalPlayerId()
    {
        if (localPlayer == null)
            return null;

        return localPlayer.playerId;
    }

    public void SendMessage(object msg)
    {
        string json = JsonConvert.SerializeObject(msg);

        if (NetworkServer.active)
        {
            ProcessServerMessage(json);

            BroadcastFromServer(json);
            return;
        }

        if (localPlayer != null)
        {
            localPlayer.CmdSendJson(json);
        }
    }

    public void ProcessServerMessage(string json)
    {
        BaseMessage baseMsg =
            JsonConvert.DeserializeObject<BaseMessage>(json);

        switch (baseMsg.action)
        {
            case "endTurn":
                {
                    EndTurnMessage endMsg =
                        JsonConvert.DeserializeObject<EndTurnMessage>(json);

                    TurnSync.Instance.OnPlayerEndedTurn(endMsg.playerId);
                    break;
                }

            case "syncBoard":
                {
                    SyncBoardMessage syncMsg =
                        JsonConvert.DeserializeObject<SyncBoardMessage>(json);

                    TurnSync.Instance.StoreFoldedCards(
                        syncMsg.playerId,
                        syncMsg.cardIds
                    );
                    break;
                }
        }
    }


    public void ProcessClientMessage(string json)
    {
        Debug.Log("Client received: " + json);

        BaseMessage baseMsg =
            JsonConvert.DeserializeObject<BaseMessage>(json);

        switch (baseMsg.action)
        {
            case "gameStart":
                GameEvents.GameStart?.Invoke();
                break;

            case "revealSingleCard":
                HandleReveal(json);
                break;

            case "syncBoard":
                HandleSyncBoard(json);
                break;

            case "endTurn":
                break;

            case "scoreUpdate":
                ScoreMessage score =
                    JsonConvert.DeserializeObject<ScoreMessage>(json);

                EventManager.Instance.TriggerEvent(
                    new UpdateScoreEvent(score.p1Score, score.p2Score)
                );
                break;
            case "startTurn":
                StartTurnMessage turnMsg =
                    JsonConvert.DeserializeObject<StartTurnMessage>(json);

                GameEvents.StartNewTurn?.Invoke(turnMsg.turn);
                break;


        }
    }



    void HandleSyncBoard(string json)
    {
        SyncBoardMessage msg =
            JsonConvert.DeserializeObject<SyncBoardMessage>(json);

        GameEvents.SyncBoard?.Invoke(
            msg.playerId,
            msg.cardIds.Count
        );
    }

    void HandleReveal(string json)
    {
        RevealCardMessage msg =
            JsonConvert.DeserializeObject<RevealCardMessage>(json);

        GameEvents.RevealCard?.Invoke(
            msg.playerId,
            msg.cardId,
            msg.orderIndex
        );
    }

    public void BroadcastFromServer(string json)
    {
        foreach (var conn in NetworkServer.connections.Values)
        {
            if (conn.identity != null)
            {
                CardNetworkPlayer player =
                    conn.identity.GetComponent<CardNetworkPlayer>();

                if (player != null)
                {
                    player.RpcReceiveJson(json);
                }
            }
        }
    }
}
