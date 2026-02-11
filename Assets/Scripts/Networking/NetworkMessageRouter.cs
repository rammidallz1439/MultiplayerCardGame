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

    public void SendMessage(object msg)
    {
        string json = JsonConvert.SerializeObject(msg);

        if (NetworkServer.active)
        {
            BroadcastFromServer(json);
            return;
        }

        if (localPlayer != null)
        {
            localPlayer.CmdSendJson(json);
        }
    }


    void HandleSyncBoard(string json)
    {
        SyncBoardMessage msg =
            JsonConvert.DeserializeObject<SyncBoardMessage>(json);

        GameEvents.SyncBoard?.Invoke(msg.opponentCardCount);
    }

    public void ProcessServerMessage(string json)
    {
        BaseMessage baseMsg =
            JsonConvert.DeserializeObject<BaseMessage>(json);

        switch (baseMsg.action)
        {
            case "endTurn":
                TurnSync.Instance.OnPlayerEndedTurn(json);
                break;
        }
    }

    public void ProcessClientMessage(string json)
    {
        BaseMessage baseMsg =
            JsonConvert.DeserializeObject<BaseMessage>(json);

        switch (baseMsg.action)
        {
            case "gameStart":
                GameEvents.GameStart?.Invoke();
                break;

            case "revealSingleCard":
                GameEvents.RevealCard?.Invoke();
                break;
            case "syncBoard":
                HandleSyncBoard(json);
                break;
        }
    }

    void BroadcastFromServer(string json)
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
