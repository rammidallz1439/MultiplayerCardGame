using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    [Header("UI")]
    public Transform PickcardsHolder;
    public Transform HandCardsHolder;
    public GameObject CardPrefab;
    public Button EndTurnButton;
    public Button PlayCardButton;
    public TMP_Text TurnCount;
    public TMP_Text TimerText;
    public TMP_Text ScoreTest;
    public TMP_Text CostText;
    public Transform OpponentCardsHolder;

    [Space(10)]
    [Header("cards")]
    public CardHandler SelectedCard = null;


    [Space(10)]
    [Header("Flags")]
    public bool IsRunning = true;
    public bool GameStarted = false;

    [Space(10)]
    [Header("Timer")]
    public float TurnDuration = 30f;
    public float CurrentTime;



    [Space(10)]
    [Header("Game")]
    public int CurrentTurn = 1;
    public int CurrentCost;
    public int FoldedCardCount = 0;
}
