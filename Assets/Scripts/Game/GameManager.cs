using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vault;

public class GameManager
{
    protected GameHandler Handler;

    #region Handler

    protected void OnCardSelectedEventHandler(OnCardSelectedEvent e)
    {

        if (Handler.SelectedCard != null)
        {
            GenericEventsController.Instance.PopUpEvent(Handler.SelectedCard.gameObject, 1f);
            Handler.SelectedCard.CardButton.interactable = true;
        }


        Handler.SelectedCard = e.Card;
        GenericEventsController.Instance.PopUpEvent(Handler.SelectedCard.gameObject, 1.2f);

        Handler.SelectedCard.CardButton.interactable = false;
        Handler.PlayCardButton.gameObject.SetActive(true);

    }


    protected void UpdateScoreEventHandler(UpdateScoreEvent e)
    {
        Handler.ScoreTest.text = e.PlayerScore.ToString();
        Handler.OpponentScoreText.text = e.OpponentScore.ToString();
    }
    #endregion

    #region methods

    protected void Init()
    {
        Handler.CurrentTime = Handler.TurnDuration;
        Handler.TurnCount.text = $"{Handler.CurrentTurn} / {GameConstants.TotalTurns}";


        Handler.PlayCardButton.onClick.AddListener(OnPlayCardButton);
        Handler.EndTurnButton.onClick.AddListener(EndTurn);


        Handler.CurrentCost = Handler.CurrentTurn;
        Handler.CostText.text = Handler.CurrentCost.ToString();

        Handler.CardConfig = DataManager.Instance.LoadJsonFromResources<CardConfig>(GameConstants.CardDataPath);


    }

    protected void DeIniti()
    {
        GameEvents.GameStart -= OnGameStart;
        GameEvents.SyncBoard -= OnSyncBoard;
        GameEvents.RevealCard -= OnRevealCard;
        GameEvents.StartNewTurn -= OnStartNewTurn;




        Handler.PlayCardButton.onClick.RemoveListener(OnPlayCardButton);
        Handler.EndTurnButton.onClick.RemoveListener(EndTurn);

    }

    protected void InitializeDeck()
    {
        Debug.Log("InitializeDeck called");
        CardConfig config = DataManager.Instance.LoadJsonFromResources<CardConfig>(GameConstants.CardDataPath);

        if (Handler.CurrentCost == 1)
        {
            for (int i = 0; i < GameConstants.DeckSize; i++)
            {

                GameObject card = MonoHelper.Instance.InstantiateObject(Handler.CardPrefab, Handler.HandCardsHolder);
                CardHandler cardHandler = card.GetComponent<CardHandler>();
                cardHandler.SetCardDetails(config.Cards[i].ID, config.Cards[i].Cost, config.Cards[i].Power, config.Cards[i].Name);
                cardHandler.BackFace.SetActive(false);
            }

        }
        else
        {
            List<CardData> shuffledCards = GenericEventsController.Instance.Shuffle(config.Cards);
            for (int i = 0; i < GameConstants.DeckSize; i++)
            {

                GameObject card = MonoHelper.Instance.InstantiateObject(Handler.CardPrefab, Handler.HandCardsHolder);
                CardHandler cardHandler = card.GetComponent<CardHandler>();
                cardHandler.SetCardDetails(shuffledCards[i].ID, shuffledCards[i].Cost, shuffledCards[i].Power, shuffledCards[i].Name);
                cardHandler.BackFace.SetActive(false);

            }
        }


    }



    protected void RunTimer()
    {
        if (!Handler.IsRunning) return;

        Handler.CurrentTime -= Time.deltaTime;
        Handler.TimerText.text = Mathf.CeilToInt(Handler.CurrentTime).ToString();

        if (Handler.CurrentTime <= 0)
        {
            Handler.CurrentTime = 0;
            Handler.IsRunning = false;

            EndTurn();
        }

    }


    protected void OnStartNewTurn()
    {

        if (Handler.CurrentTurn >= GameConstants.TotalTurns)
        {
            DetermineWinner();
            return;
        }

        Handler.CurrentTurn++;

        Handler.CurrentCost = Handler.CurrentTurn;
        Handler.CostText.text = Handler.CurrentCost.ToString();

        Handler.CurrentTime = Handler.TurnDuration;
        Handler.IsRunning = true;

        Handler.TurnCount.text =
            $"{Handler.CurrentTurn} / {GameConstants.TotalTurns}";
    }


    void DetermineWinner()
    {
        int playerScore = int.Parse(Handler.ScoreTest.text);
        int opponentScore = int.Parse(Handler.OpponentScoreText.text);

        if (playerScore > opponentScore)
        {
            Handler.GameOverHeader.text = "YOU WIN!";
            Handler.TotalScoreText.text = Handler.ScoreTest.text;
            Handler.GameOverPanel.SetActive(true);
        }
        else if (playerScore < opponentScore)
        {
            Handler.GameOverHeader.text = "YOU LOST!";
            Handler.TotalScoreText.text = Handler.ScoreTest.text;
            Handler.GameOverPanel.SetActive(true);
        }
        else
        {
            Handler.GameOverHeader.text = "ITS A DRAW";
            Handler.TotalScoreText.text = Handler.ScoreTest.text;
            Handler.GameOverPanel.SetActive(true);
        }

        Handler.IsRunning = false;
    }



    private void OnPlayCardButton()
    {
        if (Handler.SelectedCard == null) return;

        if (Handler.CurrentCost >= Handler.SelectedCard.Cost)
        {
            Handler.SelectedCard.transform.SetParent(Handler.PickcardsHolder);
            GenericEventsController.Instance.PopUpEvent(Handler.SelectedCard.gameObject, 1f);
            Handler.FoldedCardCount++;


            Handler.SelectedCard = null;

            Handler.CurrentCost--;
            Handler.CostText.text = Handler.CurrentCost.ToString();

            Handler.PlayCardButton.gameObject.SetActive(false);


            Debug.Log("InitializeDeck called");
            CardConfig config = DataManager.Instance.LoadJsonFromResources<CardConfig>(GameConstants.CardDataPath);

            List<CardData> shuffledCards = GenericEventsController.Instance.Shuffle(config.Cards);

            GameObject card = MonoHelper.Instance.InstantiateObject(Handler.CardPrefab, Handler.HandCardsHolder);
            CardHandler cardHandler = card.GetComponent<CardHandler>();
            cardHandler.SetCardDetails(shuffledCards[0].ID, shuffledCards[0].Cost, shuffledCards[0].Power, shuffledCards[0].Name);
            cardHandler.BackFace.SetActive(false);
        }
        else
        {
            GenericEventsController.Instance.PopUpEvent(Handler.SelectedCard.gameObject, 1f);
            Handler.SelectedCard = null;
            Handler.PlayCardButton.gameObject.SetActive(false);

        }
    }


    private void EndTurn()
    {
        if (!Handler.IsRunning)
            return;

        Handler.IsRunning = false;

        string playerId = NetworkMessageRouter.Instance.GetLocalPlayerId();

        // collect folded card IDs
        List<int> foldedIds = new List<int>();

        foreach (Transform card in Handler.PickcardsHolder)
        {
            CardHandler ch = card.GetComponent<CardHandler>();
            foldedIds.Add(ch.ID);
        }

        SyncBoardMessage syncMsg = new SyncBoardMessage
        {
            playerId = playerId,
            cardIds = foldedIds
        };
        NetworkMessageRouter.Instance.SendMessage(syncMsg);

        EndTurnMessage msg = new EndTurnMessage
        {
            playerId = playerId
        };
        NetworkMessageRouter.Instance.SendMessage(msg);

    }



    protected void OnGameStart()
    {

        if (Handler.GameStarted) return;

        Handler.GameStarted = true;
        Debug.Log("GameStart event received");

        Init();
        InitializeDeck();

        Handler.IsRunning = true;
    }


    protected void OnSyncBoard(string playerId, int opponentCardCount)
    {
        string localId = NetworkMessageRouter.Instance.GetLocalPlayerId();

        if (playerId == localId)
            return;

        int currentCount = Handler.OpponentCardsHolder.childCount;
        int cardsToAdd = opponentCardCount - currentCount;

        Debug.Log($"Opponent folded cards: {opponentCardCount}, current: {currentCount}");

        for (int i = 0; i < cardsToAdd; i++)
        {
            GameObject obj = MonoHelper.Instance.InstantiateObject(
                Handler.CardPrefab,
                Handler.OpponentCardsHolder
            );

            CardHandler card = obj.GetComponent<CardHandler>();

            if (card.BackFace != null)
                card.BackFace.SetActive(true);
        }
    }

    protected void OnRevealCard(string playerId, int cardId, int orderIndex)
    {
        string localId = NetworkMessageRouter.Instance.GetLocalPlayerId();
        bool isOpponent = playerId != localId;

        Transform holder = isOpponent
            ? Handler.OpponentCardsHolder
            : Handler.PickcardsHolder;

        if (orderIndex >= holder.childCount)
        {
            Debug.LogWarning("Reveal index out of bounds: " + orderIndex);
            return;
        }

        CardConfig config =
            DataManager.Instance.LoadJsonFromResources<CardConfig>(
                GameConstants.CardDataPath);

        CardData data = config.Cards.Find(c => c.ID == cardId);
        if (data == null) return;

        Transform slot = holder.GetChild(orderIndex);
        CardHandler card = slot.GetComponent<CardHandler>();

        card.SetCardDetails(
            data.ID,
            data.Cost,
            data.Power,
            data.Name
        );

        // Reveal face
        if (card.BackFace != null)
            card.BackFace.SetActive(false);
    }

    public void RevealAllCards(string playerId, List<int> cardIds)
    {
        for (int i = 0; i < cardIds.Count; i++)
        {
            GameEvents.RevealCard?.Invoke(playerId, cardIds[i], i);
        }
    }


    public void OnContinueButton()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopHost();
        }
        else if (NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopClient();
        }
        else if (NetworkServer.active)
        {
            NetworkManager.singleton.StopServer();
        }

        SceneManager.LoadScene("Menu");
    }


    protected void OnStartNewTurn(int turn)
    {
        Handler.CurrentTurn = turn;
        Handler.TurnCount.text = $"{Handler.CurrentTurn} / {GameConstants.TotalTurns}";

        Handler.CurrentCost = Handler.CurrentTurn;
        Handler.CostText.text = Handler.CurrentCost.ToString();

        Handler.CurrentTime = Handler.TurnDuration;
        Handler.IsRunning = true;
    }

    #endregion
}
