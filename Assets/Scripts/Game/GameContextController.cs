using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vault;

public class GameContextController : Registerer
{
    [SerializeField] private GameHandler _gameHandler;
    public override void Enable()
    {

    }

    public override void OnAwake()
    {
        AddController(new GameController(_gameHandler));
    }

    public override void OnStart()
    {
    }
}
