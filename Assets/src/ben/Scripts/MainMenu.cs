using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    BCModeTrigger bcModeTrigger;

    UIDocument UIDocument;
    protected VisualElement root;

    private VisualElement Main;

    const string playButtonName = "PlayButton";
    const string playAutoDemoButtonName = "PlayAutoDemoButton";
    const string helpButtonName = "HelpButton";
    const string settingsButtonName = "SettingsButton";
    const string quitButtonName = "QuitButton";
    Button PlayButton;
    Button PlayAutoDemoButton;
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
                                Settings Menu
    ************************************************************************/
    private VisualElement SettingsMenu;
    private Button SettingsBackButton;

    private VisualElement DrBCModeContainer;
    private Toggle DrBCModeToggle;

    private VisualElement MusicSliderContainer;
    private Slider MusicSlider;
    private VisualElement MusicDragger;
    private VisualElement MusicBar;
    private VisualElement MusicNewDragger;

    private VisualElement SFXSliderContainer;
    private Slider SFXSlider;
    private VisualElement SFXDragger;
    private VisualElement SFXBar;
    private VisualElement SFXNewDragger;

    /*************************************************************************
                                Help Menu
    ************************************************************************/
    private VisualElement HelpMenu;
    private Button HelpBackButton;

    /*************************************************************************
                                    Lifecycles
    ************************************************************************/
    private void Awake()
    {
        input = new PlayerControls();

        bcModeTrigger = GameObject.Find("BCModeManager").GetComponent<BCModeTrigger>();
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
        UIDocument = GetComponent<UIDocument>();

        if (UIDocument == null)
        {
            Debug.LogError("Main Menu UI Document is null");
            return;
        }
        else
        {
            Debug.Log("Main menu detected!");
        }

        root = UIDocument.rootVisualElement;

        Main = root.Q<VisualElement>("Main");

        PlayButton = root.Q<Button>(playButtonName);
        PlayAutoDemoButton = root.Q<Button>(playAutoDemoButtonName);
        HelpButton = root.Q<Button>(helpButtonName);
        SettingsButton = root.Q<Button>(settingsButtonName);
        QuitButton = root.Q<Button>(quitButtonName);

        // Assigns the Menu Buttons in order and sets the selected button
        buttonsInOrder = new Button[5] { PlayButton, PlayAutoDemoButton, HelpButton, SettingsButton, QuitButton };
        UpdateSelectedButton(PlayButton);

        PlayButton?.RegisterCallback<ClickEvent>(ClickPlayButton);
        PlayAutoDemoButton?.RegisterCallback<ClickEvent>(ClickPlayAutoDemoButton);
        SettingsButton?.RegisterCallback<ClickEvent>(ClickSettingsButton);
        HelpButton?.RegisterCallback<ClickEvent>(ClickHelpButton);
        QuitButton?.RegisterCallback<ClickEvent>(ClickQuitButton);

        // SETTINGS MENU
        SettingsMenu = root.Q<VisualElement>("Settings");
        SettingsBackButton = root.Q<Button>("SettingsBackButton");
        SettingsBackButton?.RegisterCallback<ClickEvent>(ClickBackButton);

        // DR. BC MODE TOGGLE
        DrBCModeContainer = root.Q<VisualElement>("DrBCModeContainer");
        DrBCModeToggle = DrBCModeContainer.Q<Toggle>("Toggle");
        DrBCModeToggle.value = PlayerPrefs.GetInt("DrBCMode") == 1 ? true : false;

        DrBCModeToggle?.RegisterValueChangedCallback(DrBCModeToggleChanged);

        // MUSIC VOLUME SLIDER
        MusicSliderContainer = root.Q<VisualElement>("MusicVolumeContainer");
        MusicSlider = MusicSliderContainer.Q<Slider>("Slider");
        MusicDragger = MusicSlider.Q<VisualElement>("unity-dragger");

        MusicBar = new VisualElement();
        MusicDragger.Add(MusicBar);
        MusicBar.name = "SliderFullBar";
        MusicBar.AddToClassList("bar");

        MusicNewDragger = new VisualElement();
        MusicSlider.Add(MusicNewDragger);
        MusicNewDragger.name = "NewDragger";
        MusicNewDragger.AddToClassList("new-dragger");
        MusicNewDragger.pickingMode = PickingMode.Ignore;

        MusicSlider?.RegisterValueChangedCallback(MusicSliderValueChanged);

        MusicSlider.value = PlayerPrefs.GetFloat("MusicVolume");

        // SFX VOLUME SLIDER
        SFXSliderContainer = root.Q<VisualElement>("SFXVolumeContainer");
        SFXSlider = SFXSliderContainer.Q<Slider>("Slider");
        SFXDragger = SFXSlider.Q<VisualElement>("unity-dragger");

        SFXBar = new VisualElement();
        SFXDragger.Add(SFXBar);
        SFXBar.name = "SliderFullBar";
        SFXBar.AddToClassList("bar");

        SFXNewDragger = new VisualElement();
        SFXSlider.Add(SFXNewDragger);
        SFXNewDragger.name = "NewDragger";
        SFXNewDragger.AddToClassList("new-dragger");
        SFXNewDragger.pickingMode = PickingMode.Ignore;

        SFXSlider?.RegisterValueChangedCallback(SFXSliderValueChanged);

        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume");

        // HELP MENU
        HelpMenu = root.Q<VisualElement>("Help");
        HelpBackButton = root.Q<Button>("HelpBackButton");
        HelpBackButton?.RegisterCallback<ClickEvent>(ClickBackButton);

        ChangeMenuScreen("Main");
    }

    private void Update()
    {
        UpdateDraggerPosition(MusicDragger, MusicNewDragger);
        UpdateDraggerPosition(SFXDragger, SFXNewDragger);

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


    // Changes the Main Menu screen to the passed menu
    private void ChangeMenuScreen(string menu)
    {
        if (menu == "Main")
        {
            UIManager.Instance.SetUIElementVisibility(SettingsMenu, false);
            UIManager.Instance.SetUIElementVisibility(HelpMenu, false);
            UIManager.Instance.SetUIElementVisibility(Main, true);
        }
        else if (menu == "Settings")
        {
            UIManager.Instance.SetUIElementVisibility(Main, false);
            UIManager.Instance.SetUIElementVisibility(HelpMenu, false);
            UIManager.Instance.SetUIElementVisibility(SettingsMenu, true);
        }
        else if (menu == "Help")
        {
            UIManager.Instance.SetUIElementVisibility(Main, false);
            UIManager.Instance.SetUIElementVisibility(SettingsMenu, false);
            UIManager.Instance.SetUIElementVisibility(HelpMenu, true);
        }
    }


    private void ClickPlayButton(ClickEvent evt)
    {
        PlayGame();
    }

    private void PlayGame()
    {
        UIManager.Instance.PlayGame("SampleScene");
    }

    private void ClickPlayAutoDemoButton(ClickEvent evt)
    {
        PlayAutoDemo();
    }

    private void PlayAutoDemo()
    {
        UIManager.Instance.PlayGame("DemoScene");
    }

    private void ClickSettingsButton(ClickEvent evt)
    {
        ChangeMenuScreen("Settings");
    }

    private void ClickHelpButton(ClickEvent evt)
    {
        ChangeMenuScreen("Help");
    }

    private void ClickQuitButton(ClickEvent evt)
    {
        QuitGame();
    }

    private void QuitGame()
    {
        UIManager.Instance.Quit();
    }

    private void ClickBackButton(ClickEvent evt)
    {
        ChangeMenuScreen("Main");
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
        else if (selectedButton == PlayAutoDemoButton)
        {
            PlayAutoDemo();
        }
        else if (selectedButton == QuitButton)
        {
            QuitGame();
        }
    }

    /*************************************************************************
                                Settings Sliders
    ************************************************************************/
    private void DrBCModeToggleChanged(ChangeEvent<bool> value)
    {
        bcModeTrigger.SwitchMode();
        PlayerPrefs.SetInt("DrBCMode", value.newValue ? 1 : 0);
    }

    private void MusicSliderValueChanged(ChangeEvent<float> value)
    {
        UpdateDraggerPosition(MusicDragger, MusicNewDragger);
        PlayerPrefs.SetFloat("MusicVolume", value.newValue);
    }

    private void SFXSliderValueChanged(ChangeEvent<float> value)
    {
        UpdateDraggerPosition(SFXDragger, SFXNewDragger);
        PlayerPrefs.SetFloat("SFXVolume", value.newValue);
    }

    private void UpdateDraggerPosition(VisualElement Dragger, VisualElement NewDragger)
    {
        Vector2 dist = new Vector2((NewDragger.layout.width - Dragger.layout.width) / 2, (NewDragger.layout.height - Dragger.layout.height) / 2);
        Vector2 pos = Dragger.parent.LocalToWorld(Dragger.transform.position);

        if (dist.x == float.NaN || pos.x == 0f) return;

        NewDragger.transform.position = NewDragger.parent.WorldToLocal(pos - dist);
    }


}

