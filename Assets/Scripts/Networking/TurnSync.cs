using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vault;

public class TurnSync : MonoBehaviour
{
    public static TurnSync Instance;

    private HashSet<string> readyPlayers = new HashSet<string>();
    private Dictionary<string, List<int>> foldedCards =
        new Dictionary<string, List<int>>();

    private Dictionary<string, int> totalScores =
    new Dictionary<string, int>();

    private int currentTurn = 1;


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
        if (!NetworkServer.active)
            return;

        Debug.Log("Both players ready. Resolving turn.");

        List<string> players = new List<string>(foldedCards.Keys);

        if (players.Count < 2)
            return;

        string p1 = players[0];
        string p2 = players[1];

        List<int> p1Cards = foldedCards[p1];
        List<int> p2Cards = foldedCards[p2];

        int p1TurnScore = CalculateScore(p1Cards);
        int p2TurnScore = CalculateScore(p2Cards);

        // initialize total scores if not present
        if (!totalScores.ContainsKey(p1))
            totalScores[p1] = 0;

        if (!totalScores.ContainsKey(p2))
            totalScores[p2] = 0;

        // add turn score to total
        totalScores[p1] += p1TurnScore;
        totalScores[p2] += p2TurnScore;

        Debug.Log("P1 Total Score: " + totalScores[p1]);
        Debug.Log("P2 Total Score: " + totalScores[p2]);

        ScoreMessage scoreMsg = new ScoreMessage
        {
            p1Score = totalScores[p1],
            p2Score = totalScores[p2]
        };

        NetworkMessageRouter.Instance.SendMessage(scoreMsg);

        readyPlayers.Clear();

        StartCoroutine(RevealSequence());
    }

    int CalculateScore(List<int> cardIds)
    {
        int total = 0;

        CardConfig config =
            DataManager.Instance.LoadJsonFromResources<CardConfig>(
                GameConstants.CardDataPath);

        foreach (int id in cardIds)
        {
            CardData data = config.Cards.Find(c => c.ID == id);
            if (data != null)
                total += data.Power;
        }

        return total;
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

        yield return new WaitForSeconds(1f);

        if (currentTurn >= GameConstants.TotalTurns)
        {
            Debug.Log("Game Over after turn " + currentTurn);

            GameEvents.GameEnd?.Invoke();
            yield break;
        }

        currentTurn++;

        StartTurnMessage msg = new StartTurnMessage
        {
            turn = currentTurn
        };

        NetworkMessageRouter.Instance.SendMessage(msg);

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
