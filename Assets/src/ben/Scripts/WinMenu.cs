using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WinMenu : GameScreen
{
    GameScreen gameUI;
    GameOverMenu gameOverMenu;

    /*************************************************************************
                                    Win Menu
    ************************************************************************/
    const string winMenuName = "WinMenu";
    VisualElement WinMenuEl;

    /*************************************************************************
                                Win Menu Buttons
    ************************************************************************/
    const string winNextLevelButtonName = "WinNextLevelButton";
    const string winQuitButtonName = "WinQuitButton";
    Button WinNextLevelButton;
    Button WinQuitButton;

    /*************************************************************************
                                    Lifecycles
    ************************************************************************/
    protected override void Awake()
    {

        base.Awake();

        // Needed to access the 'isGameOver' variable
        gameUI = GameObject.FindGameObjectWithTag("GameUIDocument").GetComponent<GameScreen>();
        // Needed to close Game Over Menu
        gameOverMenu = GameObject.FindGameObjectWithTag("GameUIDocument").GetComponent<GameOverMenu>();

        // Win Menu
        WinMenuEl = root.Q<VisualElement>(winMenuName);

        // Win Menu Buttons
        WinNextLevelButton = root.Q<Button>(winNextLevelButtonName);
        WinQuitButton = root.Q<Button>(winQuitButtonName);

        // Win Menu Click Events
        WinNextLevelButton?.RegisterCallback<ClickEvent>(ClickNextLevelButton);
        WinQuitButton?.RegisterCallback<ClickEvent>(ClickQuitButton);

        CloseAllMenus();
    }

    // Temp function for showing that the Win Menu can be displayed
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            // Toggles the Win Menu
            WinGame(!GetIsUIElementVisible(WinMenuEl));
        }
    }


    /*************************************************************************
                                Click Events
    ************************************************************************/
    private void ClickNextLevelButton(ClickEvent evt)
    {
        uiManager.Restart();

        // Resumes playing the game
        gameUI.SetIsGameOver(false);
    }

    /*************************************************************************
                                Menu Visibility
    ************************************************************************/
    public override void CloseAllGameMenus()
    {
        base.CloseAllGameMenus();
        SetUIElementVisibility(WinMenuEl, false);
    }

    public void SetWinMenuVisibility(bool visibility)
    {
        gameOverMenu.CloseAllGameMenus();
        CloseAllGameMenus();
        SetUIElementVisibility(WinMenuEl, visibility);
    }

    /*************************************************************************
                                    Actions
    ************************************************************************/
    public void WinGame(bool status)
    {
        // Sets whether the game is over or not
        gameUI.SetIsGameOver(status);

        // Hides or shows the Win Menu
        SetWinMenuVisibility(status);

        // If the game is over, stop the game time, otherwise resume the game time
        if (status)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
}