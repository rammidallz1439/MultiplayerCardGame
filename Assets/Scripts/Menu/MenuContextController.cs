using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vault;
public class MenuContextController : Registerer
{
    [SerializeField] private MenuHandler _menuHandler;
    public override void Enable()
    {
    }

    public override void OnAwake()
    {
        AddController(new MenuController(_menuHandler));
    }

    public override void OnStart()
    {
        
    }
}
