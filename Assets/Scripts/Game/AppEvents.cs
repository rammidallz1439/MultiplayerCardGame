using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vault;
using System;

public class AppEvents 
{
  
}

public struct OnCardSelectedEvent : GameEvent
{
    public CardHandler Card;

    public OnCardSelectedEvent(CardHandler card)
    {
        Card = card;
    }
}





public struct UpdateScoreEvent : GameEvent
{
    public int PlayerScore;
    public int OpponentScore;

    public UpdateScoreEvent(int playerScore, int opponentScore)
    {
        PlayerScore = playerScore;
        OpponentScore = opponentScore;
    }
}