using System;

public static class GameEvents
{
    public static Action GameStart;
    public static Action<string, int, int> RevealCard;
    public static Action<string, int> SyncBoard;
    public static Action<int> StartNewTurn;
}
