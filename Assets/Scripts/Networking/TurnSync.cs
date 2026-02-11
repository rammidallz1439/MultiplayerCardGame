using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSync : MonoBehaviour
{
    public static TurnSync Instance;

    private HashSet<string> readyPlayers = new HashSet<string>();
    private Dictionary<string, List<int>> foldedCards =
        new Dictionary<string, List<int>>();

    void Awake()
    {
        Instance = this;
    }

    public void OnPlayerEndedTurn(string playerId)
    {
        readyPlayers.Add(playerId);

        Debug.Log("Player ready: " + playerId);

        if (readyPlayers.Count == 2)
        {
            ResolveTurn();
        }
    }

    public void StoreFoldedCards(string playerId, List<int> cardIds)
    {
        foldedCards[playerId] = cardIds;
    }

    void ResolveTurn()
    {
        Debug.Log("Both players ready. Resolving turn.");

        readyPlayers.Clear();

        StartCoroutine(RevealSequence());
    }

    IEnumerator RevealSequence()
    {
        yield return new WaitForSeconds(0.5f);

        List<string> players = new List<string>(foldedCards.Keys);

        if (players.Count < 2)
            yield break;

        string p1 = players[0];
        string p2 = players[1];

        List<int> p1Cards = foldedCards[p1];
        List<int> p2Cards = foldedCards[p2];

        int max = Mathf.Max(p1Cards.Count, p2Cards.Count);

        for (int i = 0; i < max; i++)
        {
            if (i < p1Cards.Count)
            {
                SendReveal(p1, p1Cards[i], i);
                yield return new WaitForSeconds(0.5f);
            }

            if (i < p2Cards.Count)
            {
                SendReveal(p2, p2Cards[i], i);
                yield return new WaitForSeconds(0.5f);
            }
        }

        foldedCards.Clear();
    }

    void SendReveal(string playerId, int cardId, int orderIndex)
    {
        RevealCardMessage reveal = new RevealCardMessage
        {
            playerId = playerId,
            cardId = cardId,
            orderIndex = orderIndex
        };

        NetworkMessageRouter.Instance.SendMessage(reveal);
    }
}
