using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameScreen : MonoBehaviour
{
    protected UIDocument UIDocument;
    protected VisualElement root;
    protected ActionEventManager actionEvent;

    /*************************************************************************
                                Game Over Status
    ************************************************************************/
    private bool isGameOver;
    public void SetIsGameOver(bool status)
    {
        isGameOver = status;
    }

    public bool GetIsGameOver()
    {
        return isGameOver;
    }

    /*************************************************************************
                                    Lifecycles
    ************************************************************************/
    protected virtual void Awake()
    {
        actionEvent = GameObject.FindGameObjectWithTag("ActionEvent").GetComponent<ActionEventManager>();

        UIDocument = GetComponent<UIDocument>();

        if (UIDocument == null)
        {
            Debug.LogError("Main Menu UI Document is null");
            return;
        }

        root = UIDocument.rootVisualElement;

        CloseAllMenus();
    }


    /*************************************************************************
                                    Click Events
    ************************************************************************/
    protected void ClickQuitButton(ClickEvent evt)
    {
        QuitToMainMenu();
    }

    protected void QuitToMainMenu()
    {
        UIManager.Instance.QuitToMainMenu();
    }


    /*************************************************************************
                                UI Element Visibility
    ************************************************************************/
    public void SetUIElementVisibility(VisualElement uiElement, bool visibility)
    {
        UIManager.Instance.SetUIElementVisibility(uiElement, visibility);
    }

    public bool GetIsUIElementVisible(VisualElement uiElement)
    {
        if (uiElement == null)
            return false;

        return uiElement.style.display != DisplayStyle.None;
    }

    /*************************************************************************
                                    Menu Closing
       ************************************************************************/
    public virtual void CloseAllGameMenus()
    {
    }

    public virtual void CloseAllUnitMenus()
    {
    }

    public void CloseAllMenus()
    {
        CloseAllGameMenus();
        CloseAllUnitMenus();
    }
}