using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    UIDocument MainMenuDocument;
    protected VisualElement root;

    UIManager uiManager;

    const string playButtonName = "PlayButton";
    const string helpButtonName = "HelpButton";
    const string settingsButtonName = "SettingsButton";
    const string quitButtonName = "QuitButton";
    Button PlayButton;
    Button HelpButton;
    Button SettingsButton;
    Button QuitButton;

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
    private Button selectedButton;
    const string selectedButtonClassName = "main-menu-selected-button";

    private Button[] buttonsInOrder = new Button[4];

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
    private void Awake()
    {
        input = new PlayerControls();
    }

    private void OnEnable()
    {
        input.Enable();
        input.UI.Navigate.performed += OnNavigationPerformed;
        input.UI.Navigate.canceled += OnNavigationCancelled;

        input.UI.Select.performed += ctx => ClickSelectedButton();
    }

    private void OnDisable()
    {
        input.Disable();
        input.UI.Navigate.performed -= OnNavigationPerformed;
        input.UI.Navigate.canceled -= OnNavigationCancelled;

        input.UI.Select.performed -= ctx => ClickSelectedButton();
    }
    private void Start()
    {
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();

        MainMenuDocument = GetComponent<UIDocument>();

        if (MainMenuDocument == null)
        {
            Debug.LogError("Main Menu UI Document is null");
            return;
        }
        else
        {
            Debug.Log("Main menu detected!");
        }

        root = MainMenuDocument.rootVisualElement;

        PlayButton = root.Q<Button>(playButtonName);
        HelpButton = root.Q<Button>(helpButtonName);
        SettingsButton = root.Q<Button>(settingsButtonName);
        QuitButton = root.Q<Button>(quitButtonName);

        // Assigns the Menu Buttons in order and sets the selected button
        buttonsInOrder = new Button[4] { PlayButton, HelpButton, SettingsButton, QuitButton };
        UpdateSelectedButton(PlayButton);

        PlayButton?.RegisterCallback<ClickEvent>(ClickPlayButton);
        QuitButton?.RegisterCallback<ClickEvent>(ClickQuitButton);
    }

    private void Update()
    {
        keyInputTimer++;

        if (keyInputTimer > keyInputDelay)
        {
            // If navigating up
            if (navigationInput.y == 1.0f)
            {
                // Iterate over every button except the first button (Since if the Selected Button is the first one
                // then you can't navigate up)
                for (int i = 1; i < buttonsInOrder.Length; i++)
                {
                    // If the current Button is the Selected Button
                    if (buttonsInOrder[i] == selectedButton)
                    {
                        // Update the Selected Button to be the button above this one
                        UpdateSelectedButton(buttonsInOrder[i - 1]);
                        break;
                    }
                }

                // Unselect every button that isn't the Selected Button
                for (int i = 0; i < buttonsInOrder.Length; i++)
                {
                    if (buttonsInOrder[i] != selectedButton) UnselectButton(buttonsInOrder[i]);
                }

                keyInputTimer = 0;
            }
            // If navigating down
            else if (navigationInput.y == -1.0f)
            {
                // Iterate over every button except the last button (Since if the Selected Button is the last one
                // then you can't navigate down)
                for (int i = 0; i < buttonsInOrder.Length - 1; i++)
                {
                    // If the current Button is the Selected Button
                    if (buttonsInOrder[i] == selectedButton)
                    {
                        // Update the Selected Button to be the button below this one
                        UpdateSelectedButton(buttonsInOrder[i + 1]);
                        break;
                    }
                }

                // Unselect every button that isn't the Selected Button
                for (int i = 0; i < buttonsInOrder.Length; i++)
                {
                    if (buttonsInOrder[i] != selectedButton) UnselectButton(buttonsInOrder[i]);
                }

                keyInputTimer = 0;
            }
        }
    }

    private void ClickPlayButton(ClickEvent evt)
    {
        PlayGame();
    }

    private void PlayGame()
    {
        uiManager.PlayGame("SampleScene");
    }

    private void ClickQuitButton(ClickEvent evt)
    {
        QuitGame();
    }

    private void QuitGame()
    {

        uiManager.Quit();
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
        if (selectedButton == PlayButton)
        {
            PlayGame();
        }
        else if (selectedButton == QuitButton)
        {
            QuitGame();
        }
    }
}

