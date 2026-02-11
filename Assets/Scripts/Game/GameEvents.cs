using System;

public static class GameEvents
{
    public static Action GameStart;
    public static Action TurnStart;
    public static Action PlayerEndedTurn;
    public static Action AllPlayersReady;
    public static Action RevealCard;
    public static Action ScoreUpdated;
    public static Action TurnEnd;
    public static Action GameEnd;

    public static Action<int> SyncBoard;

}
