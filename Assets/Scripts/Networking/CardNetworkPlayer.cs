using Mirror;
using System.Collections;
using UnityEngine;

public class CardNetworkPlayer : NetworkBehaviour
{
    public string playerId;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        playerId = "P" + netId;
        StartCoroutine(WaitForRouter());
    }

    IEnumerator WaitForRouter()
    {
        while (NetworkMessageRouter.Instance == null)
            yield return null;

        NetworkMessageRouter.Instance.SetLocalPlayer(this);
    }


    [Command]
    public void CmdSendJson(string json)
    {
        Debug.Log("Server received: " + json);

        NetworkMessageRouter.Instance.ProcessServerMessage(json);
        RpcReceiveJson(json);
    }

    [ClientRpc]
   public void RpcReceiveJson(string json)
    {
        NetworkMessageRouter.Instance.ProcessClientMessage(json);
    }
}
