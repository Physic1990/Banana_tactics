using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class PauseMenu : GameScreen
{
    GameScreen gameUI;

    /*************************************************************************
                                Game Paused Status
    ************************************************************************/

    // Needed because the Input System still registers Inputs when the Timescale is set to 0.
    private bool isGamePaused;
    public void SetIsGamePaused(bool status)
    {
        isGamePaused = status;
    }

    public bool GetIsGamePaused()
    {
        return isGamePaused;
    }


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
                                Player Navigation
    ************************************************************************/
    private PlayerControls input = null;
    private Vector2 navigationInput = Vector2.zero;

    // These are needed because otherwise the "Update" Hook will execute multiple times per button click, so we need a delay.
    private int keyInputTimer = 0;
    private int keyInputDelay = 20;

    /*************************************************************************
                                Button Selection
    ************************************************************************/

    private Button[] buttonOrders = new Button[2];

    private Button selectedButton;
    const string selectedButtonClassName = "game-menu-selected-button";

    // Selects the passed Button
    private void UpdateSelectedButton(Button button)
    {
        selectedButton = button;
        selectedButton.AddToClassList(selectedButtonClassName);
    }

    // Ensures the passed Button is unselected
    private void UnselectButton(Button button)
    {
        if (selectedButton == button) selectedButton = null;

        button.RemoveFromClassList(selectedButtonClassName);
    }

    /*************************************************************************
                                    Lifecycles
    ************************************************************************/
    protected override void Awake()
    {
        base.Awake();

        input = new PlayerControls();

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

    private void OnEnable()
    {
        input.Enable();
        input.UI.Navigate.performed += OnNavigationPerformed;
        input.UI.Navigate.canceled += OnNavigationCancelled;

        input.UI.Select.performed += ctx => ClickSelectedButton();
        input.UI.Toggle.performed += ctx => ClickToggleButton();
    }

    private void OnDisable()
    {
        input.Disable();
        input.UI.Navigate.performed -= OnNavigationPerformed;
        input.UI.Navigate.canceled -= OnNavigationCancelled;

        input.UI.Select.performed -= ctx => ClickSelectedButton();
        input.UI.Toggle.performed -= ctx => ClickToggleButton();
    }

    private void Update()
    {
        keyInputTimer++;

        if (keyInputTimer > keyInputDelay && GetIsGamePaused())
        {
            // If navigating up
            if (navigationInput.y == 1.0f)
            {
                // Iterate over every button except the first button (Since if the Selected Button is the first one
                // then you can't navigate up)
                for (int i = 1; i < buttonOrders.Length; i++)
                {
                    // If the current Button is the Selected Button
                    if (buttonOrders[i] == selectedButton)
                    {
                        // Update the Selected Button to be the button above this one
                        UpdateSelectedButton(buttonOrders[i - 1]);
                        break;
                    }
                }

                // Unselect every button that isn't the Selected Button
                for (int i = 0; i < buttonOrders.Length; i++)
                {
                    if (buttonOrders[i] != selectedButton) UnselectButton(buttonOrders[i]);
                }
                keyInputTimer = 0;

            }
            // If navigating down
            else if (navigationInput.y == -1.0f)
            {
                // Iterate over every button except the last button (Since if the Selected Button is the last one
                // then you can't navigate down)
                for (int i = 0; i < buttonOrders.Length - 1; i++)
                {
                    // If the current Button is the Selected Button
                    if (buttonOrders[i] == selectedButton)
                    {
                        // Update the Selected Button to be the button below this one
                        UpdateSelectedButton(buttonOrders[i + 1]);
                        break;
                    }
                }

                // Unselect every button that isn't the Selected Button
                for (int i = 0; i < buttonOrders.Length; i++)
                {
                    if (buttonOrders[i] != selectedButton) UnselectButton(buttonOrders[i]);
                }
                keyInputTimer = 0;
            }
        }
    }


    /*************************************************************************
                                Player Navigation Functions
    ************************************************************************/
    private void OnNavigationPerformed(InputAction.CallbackContext value)
    {
        // Reads the vector2 value and assigns it to navigationInput
        navigationInput = value.ReadValue<Vector2>();
    }

    private void OnNavigationCancelled(InputAction.CallbackContext value)
    {
        // Clears navigationInput
        navigationInput = Vector2.zero;
    }

    // Clicks the selected Button
    private void ClickSelectedButton()
    {
        if (selectedButton == PauseResumeButton)
        {
            PauseGame(false);
        }
        else if (selectedButton == PauseQuitButton)
        {
            QuitToMainMenu();
        }
    }

    // Toggles the Pause Menu so long as the game isn't over
    private void ClickToggleButton()
    {
        TogglePausedGame();
    }

    public void TogglePausedGame()
    {
        if (gameUI.GetIsGameOver()) return;

        PauseGame(!GetIsUIElementVisible(PauseMenuEl));
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
        {
            Time.timeScale = 0;

            // Assigns the Menu Buttons in order and sets the selected button
            buttonOrders = new Button[2] { PauseResumeButton, PauseQuitButton };
            UpdateSelectedButton(PauseResumeButton);

            // Manually asserts that the game is paused
            SetIsGamePaused(true);
        }
        else
        {
            Time.timeScale = 1;

            // Manually asserts that the game is unpaused
            SetIsGamePaused(false);
        }
    }
}