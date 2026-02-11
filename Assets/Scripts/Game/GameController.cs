using Mirror.BouncyCastle.Crmf;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vault;

public class GameController : GameManager, IController, ITick
{
    public GameController(GameHandler handler)
    {
        Handler = handler;
    }
    public void OnInitialized()
    {
        GameEvents.GameStart += OnGameStart;
        GameEvents.SyncBoard += OnSyncBoard;
        GameEvents.RevealCard += OnRevealCard;


    }

    public void OnRegisterListeners()
    {
        EventManager.Instance.AddListener<OnCardSelectedEvent>(OnCardSelectedEventHandler);
    }

    public void OnRelease()
    {
        DeIniti();
    }

    public void OnRemoveListeners()
    {
        EventManager.Instance.RemoveListener<OnCardSelectedEvent>(OnCardSelectedEventHandler);

    }

    public void OnStarted()
    {
      
    }

    public void OnUpdate()
    {
        RunTimer();
    }

    public void OnVisible()
    {
    }
}
