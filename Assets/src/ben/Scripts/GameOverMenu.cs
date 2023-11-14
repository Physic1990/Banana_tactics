using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameOverMenu : GameScreen
{
    GameScreen gameUI;
    PauseMenu pauseMenu;
    WinMenu winMenu;

    /*************************************************************************
                                   Game Over Menu
   ************************************************************************/
    const string gameOverMenuName = "GameOverMenu";
    VisualElement GameOverMenuEl;

    /*************************************************************************
                                Game Over Buttons
   ************************************************************************/
    const string gameOverRetryButtonName = "GameOverRetryButton";
    const string gameOverQuitButtonName = "GameOverQuitButton";
    Button GameOverRetryButton;
    Button GameOverQuitButton;


    /*************************************************************************
                                            Lifecycles
    ************************************************************************/
    protected override void Awake()
    {
        base.Awake();


        // Needed to access the 'isGameOver' variable
        gameUI = GameObject.FindGameObjectWithTag("GameUIDocument").GetComponent<GameScreen>();
        // Needed to access the 'isGamePaused' variable
        pauseMenu = GameObject.FindGameObjectWithTag("GameUIDocument").GetComponent<PauseMenu>();
        // Needed to close Win Menu
        winMenu = GameObject.FindGameObjectWithTag("GameUIDocument").GetComponent<WinMenu>();

        // Game Over Menu
        GameOverMenuEl = root.Q<VisualElement>(gameOverMenuName);

        // Game Over Menu Buttons
        GameOverRetryButton = root.Q<Button>(gameOverRetryButtonName);
        GameOverQuitButton = root.Q<Button>(gameOverQuitButtonName);

        // Game Over Menu Click Events
        GameOverRetryButton?.RegisterCallback<ClickEvent>(ClickRetryButton);
        GameOverQuitButton?.RegisterCallback<ClickEvent>(ClickQuitButton);

        CloseAllMenus();
    }

    // Temp function for showing that the Game Over Menu can be displayed
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            // Toggles the Game Over Menu
            GameOver(!GetIsUIElementVisible(GameOverMenuEl));
        }
    }


    /*************************************************************************
                                Click EVents
    ************************************************************************/

    private void ClickRetryButton(ClickEvent evt)
    {
        GameOver(false);
        uiManager.Restart();
    }

    /*************************************************************************
                                Menu Visibility
    ************************************************************************/
    public override void CloseAllGameMenus()
    {
        base.CloseAllGameMenus();
        SetUIElementVisibility(GameOverMenuEl, false);
    }


    public void SetGameOverMenuVisibility(bool visibility)
    {
        winMenu.CloseAllGameMenus();
        CloseAllGameMenus();
        SetUIElementVisibility(GameOverMenuEl, visibility);
    }

    /*************************************************************************
                                        Actions
    ************************************************************************/
    public void GameOver(bool status)
    {
        // Don't open the game over menu if the game is currently paused
        if (status && pauseMenu.GetIsGamePaused()) return;

        // Sets whether the game is over or not
        gameUI.SetIsGameOver(status);

        // Hides or shows the Game Over Menu
        SetGameOverMenuVisibility(status);

        // If the game is over, stop the game time, otherwise resume the game time
        if (status)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
}