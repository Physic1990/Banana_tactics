using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    UIDocument MainMenuDocument;
    protected VisualElement root;

    UIManager uiManager;

    const string playButtonName = "PlayButton";
    // const string helpButtonName = "HelpButton";
    // const string settingsButtonName = "SettingsButton";
    const string quitButtonName = "QuitButton";
    Button PlayButton;
    // Button HelpButton;
    // Button SettingsButton;
    Button QuitButton;

    // [Header("Level Data")]
    // [SerializeField] LevelSO levelData;

    private void Start()
    {
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
    }

    void onEnable()
    {
        MainMenuDocument = GetComponent<UIDocument>();

        if (MainMenuDocument == null)
        {
            Debug.LogError("Main Menu UI Document is null");
            return;
        }

        root = MainMenuDocument.rootVisualElement;

        PlayButton = root.Q<Button>(playButtonName);
        // HelpButton = root.Q<Button>(helpButtonName);
        // SettingsButton = root.Q<Button>(settingsButtonName);
        QuitButton = root.Q<Button>(quitButtonName);

        if (PlayButton != null)
        {
            Debug.Log("Main Menu Play Button detected");
        }

        // PlayButton?.RegisterCallback<ClickEvent>(ClickPlayButton);
        // QuitButton?.RegisterCallback<ClickEvent>(ClickQuitButton);
    }

    // private void ClickPlayButton(ClickEvent event)
    // {
    // uiManager.PlayGame(levelData.sceneName);
    // }

    // private void ClickQuitButton(ClickEvent event)
    // {
    // uiManager.Quit();
    // }
}

