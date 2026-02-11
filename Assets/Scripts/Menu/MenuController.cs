using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vault;

public class MenuController : MenuManager, IController
{
    public MenuController(MenuHandler handler)
    {
        Handler = handler;
    }
    public void OnInitialized()
    {
    }

    public void OnRegisterListeners()
    {
    }

    public void OnRelease()
    {
        DeInit();
    }

    public void OnRemoveListeners()
    {
    }

    public void OnStarted()
    {
        Init();
    }

    public void OnVisible()
    {
    }
}
