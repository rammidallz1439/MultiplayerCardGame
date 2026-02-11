using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Configs 
{
 
}


[Serializable]
public class CardConfig
{
    public List<CardData> Cards;
}

[Serializable]
public class  CardData
{
    public int ID;
    public string Name;
    public int Cost;
    public int Power;
    public CardAbility Ability;
}


[Serializable]
public class  CardAbility
{
    public string Type;
    public int Value;
}


#region NetworkMessages

[Serializable]
public class BaseMessage
{
    public string action;
}

[Serializable]
public class GameStartMessage
{
    public string action = "gameStart";
    public string[] playerIds;
    public int totalTurns;
}

[Serializable]
public class EndTurnMessage
{
    public string action = "endTurn";
    public string playerId;
}

[Serializable]
public class RevealCardMessage
{
    public string action = "revealSingleCard";
    public string playerId;
    public int cardId;
    public int orderIndex;
}



[System.Serializable]
public class SyncBoardMessage
{
    public string action = "syncBoard";
    public string playerId;
    public List<int> cardIds;
}


#endregion