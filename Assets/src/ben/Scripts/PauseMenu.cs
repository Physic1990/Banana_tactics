using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PauseMenu : GameScreen
{
    // protected UIDocument MainMenuDocument;

    // protected UIManager uiManager;
    // protected ActionEventManager actionEvent;


    // Game Menus
    const string pauseMenuName = "PauseMenu";
    VisualElement PauseMenuEl;

    // Game Menu Buttons
    const string pauseResumeButtonName = "PauseResumeButton";
    const string pauseQuitButtonName = "PauseQuitButton";
    Button PauseResumeButton;
    Button PauseQuitButton;

    protected override void Awake()
    {
        base.Awake();

        // Game Menus
        PauseMenuEl = root.Q<VisualElement>(pauseMenuName);

        // Game Menu Buttons
        PauseResumeButton = root.Q<Button>(pauseResumeButtonName);
        PauseQuitButton = root.Q<Button>(pauseQuitButtonName);

        // Game Menu Click Events
        PauseResumeButton?.RegisterCallback<ClickEvent>(ClickResumeButton);
        PauseQuitButton?.RegisterCallback<ClickEvent>(ClickQuitButton);

        CloseAllMenus();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //If pause screen already active unpause and viceversa
            PauseGame(!GetIsUIElementVisible(PauseMenuEl));
        }
    }

    private void ClickResumeButton(ClickEvent evt)
    {
        PauseGame(false);
    }

    // UI ELement Visibility
    public override void CloseAllGameMenus()
    {
        base.CloseAllGameMenus();
        SetUIElementVisibility(PauseMenuEl, false);
    }

    // public void CloseAllMenus()
    // {
    //     CloseAllGameMenus();
    //     CloseAllUnitMenus();
    // }

    public void SetPauseMenuVisibility(bool visibility)
    {
        CloseAllGameMenus();
        SetUIElementVisibility(PauseMenuEl, visibility);
    }

    // Pausing Game
    public void PauseGame(bool status)
    {
        //If status == true pause | if status == false unpause
        SetUIElementVisibility(PauseMenuEl, status);

        //When pause status is true change timescale to 0 (time stops)
        //when it's false change it back to 1 (time goes by normally)
        if (status)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }


}