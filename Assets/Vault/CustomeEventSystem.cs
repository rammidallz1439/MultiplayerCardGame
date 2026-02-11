using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomeEventSystem : EventSystem
{
    private GameObject lastSelected;

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void Update()
    {
        base.Update();

        if (currentSelectedGameObject != null)
        {
            lastSelected = currentSelectedGameObject;
        }
        else if (lastSelected != null)
        {
            SetSelectedGameObject(lastSelected);
        }
    }
}
