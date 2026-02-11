using UnityEngine;
using Mirror;

public class GameStarter : NetworkBehaviour
{
    public override void OnStartServer()
    {
        base.OnStartServer();

        Invoke(nameof(SendGameStart), 1f);
    }

    void SendGameStart()
    {
        Debug.Log("Server sending gameStart");

        GameStartMessage msg = new GameStartMessage
        {
            playerIds = new string[] { "P1", "P2" },
            totalTurns = GameConstants.TotalTurns
        };

        NetworkMessageRouter.Instance.SendMessage(msg);
    }
}
