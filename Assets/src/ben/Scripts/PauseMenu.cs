using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PauseMenu : GameScreen
{
    GameScreen gameUI;


    /*************************************************************************
                                    Pause Menu
    ************************************************************************/
    const string pauseMenuName = "PauseMenu";
    VisualElement PauseMenuEl;

    /*************************************************************************
                                Pause Menu Buttons
    ************************************************************************/
    const string pauseResumeButtonName = "PauseResumeButton";
    const string pauseQuitButtonName = "PauseQuitButton";
    Button PauseResumeButton;
    Button PauseQuitButton;

    /*************************************************************************
                                    Lifecycles
    ************************************************************************/
    protected override void Awake()
    {
        base.Awake();

        // Needed to access the 'isGameOver' variable
        gameUI = GameObject.FindGameObjectWithTag("GameUIDocument").GetComponent<GameScreen>();

        // Pause Menu
        PauseMenuEl = root.Q<VisualElement>(pauseMenuName);

        // Pause Menu Buttons
        PauseResumeButton = root.Q<Button>(pauseResumeButtonName);
        PauseQuitButton = root.Q<Button>(pauseQuitButtonName);

        // Pause Menu Click Events
        PauseResumeButton?.RegisterCallback<ClickEvent>(ClickResumeButton);
        PauseQuitButton?.RegisterCallback<ClickEvent>(ClickQuitButton);

        CloseAllMenus();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !gameUI.GetIsGameOver())
        {
            // Toggles the Pause Menu so long as the game isn't over
            PauseGame(!GetIsUIElementVisible(PauseMenuEl));
        }
    }

    /*************************************************************************
                                Click Events
    ************************************************************************/
    private void ClickResumeButton(ClickEvent evt)
    {
        PauseGame(false);
    }

    /*************************************************************************
                                Menu Visibility
    ************************************************************************/
    public override void CloseAllGameMenus()
    {
        base.CloseAllGameMenus();
        SetUIElementVisibility(PauseMenuEl, false);
    }

    public void SetPauseMenuVisibility(bool visibility)
    {
        CloseAllGameMenus();
        SetUIElementVisibility(PauseMenuEl, visibility);
    }

    /*************************************************************************
                                Actions
    ************************************************************************/
    public void PauseGame(bool status)
    {
        // Hides or shows the Pause Menu
        SetPauseMenuVisibility(status);

        // If the game is paused, stop the game time, otherwise resume the game time
        if (status)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }


}