using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    #endregion

    #region methods

    protected void Init()
    {
        Handler.CurrentTime = Handler.TurnDuration; 
        Handler.TurnCount.text =$"{Handler.CurrentTurn } / { GameConstants.TotalTurns}" ;


        Handler.PlayCardButton.onClick.AddListener(OnPlayCardButton);  
        Handler.EndTurnButton.onClick.AddListener(EndTurn);


        Handler.CurrentCost = Handler.CurrentTurn;
        Handler.CostText.text = Handler.CurrentCost.ToString();

    }

    protected void DeIniti()
    {
        GameEvents.GameStart -= OnGameStart;
        GameEvents.SyncBoard -= OnSyncBoard;


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
            }
        }
 
     
    }



    protected void RunTimer()
    {
        if (!Handler.IsRunning) return;

        Handler.CurrentTime -= Time.deltaTime;
        Handler.TimerText.text =Mathf.CeilToInt( Handler.CurrentTime).ToString();

        if (Handler.CurrentTime <= 0)
        {
            Handler.CurrentTime = 0;
            Handler.IsRunning = false;
            MonoHelper.Instance.RunCouroutine(StartNewTurn());
        }

    }


    private IEnumerator StartNewTurn()
    {
        yield return new WaitForSeconds(1f);
        Handler.CurrentTurn++;
        Handler.TurnCount.text = $"{Handler.CurrentTurn} / {GameConstants.TotalTurns}";
    }


    private void OnPlayCardButton()
    {
        if(Handler.SelectedCard == null) return;

        if (Handler.CurrentCost >= Handler.SelectedCard.Cost)
        {
            Handler.SelectedCard.transform.SetParent(Handler.PickcardsHolder);
            GenericEventsController.Instance.PopUpEvent(Handler.SelectedCard.gameObject, 1f);
            Handler.FoldedCardCount++;


            Handler.SelectedCard = null;

            Handler.CurrentCost--;  
            Handler.CostText.text = Handler.CurrentCost.ToString();

            Handler.PlayCardButton.gameObject.SetActive(false);
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
        Handler.IsRunning = false;

        EndTurnMessage msg = new EndTurnMessage
        {
            playerId = NetworkMessageRouter.Instance
                .GetComponent<CardNetworkPlayer>()
                .playerId
        };

        NetworkMessageRouter.Instance.SendMessage(msg);


        SyncBoardMessage syncMsg = new SyncBoardMessage
        {
            opponentCardCount = Handler.FoldedCardCount
        };
        NetworkMessageRouter.Instance.SendMessage(syncMsg);
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


    protected  void OnSyncBoard(int opponentCardCount)
    {
        Debug.Log("Opponent folded cards: " + opponentCardCount);


        for (int i = 0; i < opponentCardCount; i++)
        {
            GameObject card = MonoHelper.Instance.InstantiateObject(
                Handler.CardPrefab,
                Handler.OpponentCardsHolder
            );
        }
    }


    #endregion
}
