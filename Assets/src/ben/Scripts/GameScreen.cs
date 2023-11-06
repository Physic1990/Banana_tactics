using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameScreen : MonoBehaviour
{
    protected UIDocument MainMenuDocument;
    protected VisualElement root;

    protected UIManager uiManager;
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
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        actionEvent = GameObject.FindGameObjectWithTag("ActionEvent").GetComponent<ActionEventManager>();

        MainMenuDocument = GetComponent<UIDocument>();

        if (MainMenuDocument == null)
        {
            Debug.LogError("Main Menu UI Document is null");
            return;
        }

        root = MainMenuDocument.rootVisualElement;

        CloseAllMenus();
    }


    /*************************************************************************
                                    Click Events
    ************************************************************************/
    protected void ClickQuitButton(ClickEvent evt)
    {
        QuitGame();
    }

    protected void QuitGame()
    {
        uiManager.Quit();

    }


    /*************************************************************************
                                UI Element Visibility
    ************************************************************************/
    public void SetUIElementVisibility(VisualElement uiElement, bool isVisible)
    {
        if (uiElement == null)
            return;

        uiElement.style.display = (isVisible) ? DisplayStyle.Flex : DisplayStyle.None;
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