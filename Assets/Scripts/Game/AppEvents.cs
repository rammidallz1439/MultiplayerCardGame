using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vault;

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