using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameScreen : MonoBehaviour
{
    UIDocument MainMenuDocument;
    protected VisualElement root;

    UIManager uiManager;

    // Game Menus
    const string gameOverMenuName = "GameOverMenu";
    const string winMenuName = "WinMenu";
    const string pauseMenuName = "PauseMenu";
    VisualElement GameOverMenu;
    VisualElement WinMenu;
    VisualElement PauseMenu;

    // Game Menu Buttons
    const string winNextLevelButtonName = "WinNextLevelButton";
    const string winQuitButtonName = "WinQuitButton";
    const string gameOverRetryButtonName = "GameOverRetryButton";
    const string gameOverQuitButtonName = "GameOverQuitButton";
    const string pauseResumeButtonName = "PauseResumeButton";
    const string pauseQuitButtonName = "PauseQuitButton";
    Button WinNextLevelButton;
    Button WinQuitButton;
    Button GameOverRetryButton;
    Button GameOverQuitButton;
    Button PauseResumeButton;
    Button PauseQuitButton;

    // Unit Menu
    const string unitMenuName = "UnitMenu";
    const string enemyUnitMenuName = "EnemyUnitMenu";
    VisualElement UnitMenu;
    VisualElement EnemyUnitMenu;

    // Unit Menu Buttons
    const string unitAttackButtonName = "UnitAttackButton";
    const string unitWaitButtonName = "UnitWaitButton";
    const string unitCancelAttackButtonName = "UnitCancelAttackButton";
    const string unitConfirmAttackButtonName = "UnitConfirmAttackButton";
    Button UnitAttackButton;
    Button UnitWaitButton;
    Button UnitCancelAttackButton;
    Button UnitConfirmAttackButton;

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

        // Game Menus
        GameOverMenu = root.Q<VisualElement>(gameOverMenuName);
        WinMenu = root.Q<VisualElement>(winMenuName);
        PauseMenu = root.Q<VisualElement>(pauseMenuName);

        // Game Menu Buttons
        WinNextLevelButton = root.Q<Button>(winNextLevelButtonName);
        WinQuitButton = root.Q<Button>(winQuitButtonName);
        GameOverRetryButton = root.Q<Button>(gameOverRetryButtonName);
        GameOverQuitButton = root.Q<Button>(gameOverQuitButtonName);
        PauseResumeButton = root.Q<Button>(pauseResumeButtonName);
        PauseQuitButton = root.Q<Button>(pauseQuitButtonName);

        // Game Menu Click Events
        WinNextLevelButton?.RegisterCallback<ClickEvent>(ClickNextLevelButton);
        WinQuitButton?.RegisterCallback<ClickEvent>(ClickQuitButton);
        GameOverRetryButton?.RegisterCallback<ClickEvent>(ClickRetryButton);
        GameOverQuitButton?.RegisterCallback<ClickEvent>(ClickQuitButton);
        PauseResumeButton?.RegisterCallback<ClickEvent>(ClickResumeButton);
        PauseQuitButton?.RegisterCallback<ClickEvent>(ClickQuitButton);

        // Unit Menus
        UnitMenu = root.Q<VisualElement>(unitMenuName);
        EnemyUnitMenu = root.Q<VisualElement>(enemyUnitMenuName);

        // Unit Menu Buttons
        UnitAttackButton = root.Q<Button>(unitAttackButtonName);
        UnitWaitButton = root.Q<Button>(unitWaitButtonName);
        UnitCancelAttackButton = root.Q<Button>(unitCancelAttackButtonName);
        UnitConfirmAttackButton = root.Q<Button>(unitConfirmAttackButtonName);

        // Unit Menu Click Events
        UnitAttackButton?.RegisterCallback<ClickEvent>(ClickUnitAttackButton);
        UnitWaitButton?.RegisterCallback<ClickEvent>(ClickUnitWaitButton);
        UnitCancelAttackButton?.RegisterCallback<ClickEvent>(ClickUnitCancelAttackButton);
        UnitConfirmAttackButton?.RegisterCallback<ClickEvent>(ClickUnitConfirmAttackButton);

        CloseAllMenus();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //If pause screen already active unpause and viceversa
            PauseGame(!GetIsUIElementVisible(PauseMenu));
        }
    }

    // Game Menu Click Events
    private void ClickNextLevelButton(ClickEvent evt)
    {
        uiManager.Restart();
    }

    private void ClickRetryButton(ClickEvent evt)
    {
        uiManager.Restart();
    }

    private void ClickQuitButton(ClickEvent evt)
    {
        uiManager.Quit();
    }

    private void ClickResumeButton(ClickEvent evt)
    {
        PauseGame(false);
    }

    // Unit Menu Click Events
    private void ClickUnitAttackButton(ClickEvent evt)
    {
    }

    private void ClickUnitWaitButton(ClickEvent evt)
    {
    }

    private void ClickUnitCancelAttackButton(ClickEvent evt)
    {
    }

    private void ClickUnitConfirmAttackButton(ClickEvent evt)
    {
    }

    // UI ELement Visibility
    public void CloseAllGameMenus()
    {
        SetUIElementVisibility(WinMenu, false);
        SetUIElementVisibility(GameOverMenu, false);
        SetUIElementVisibility(PauseMenu, false);
    }

    public void CloseAllUnitMenus()
    {
        SetUIElementVisibility(UnitMenu, false);
        SetUIElementVisibility(EnemyUnitMenu, false);
    }

    public void CloseAllMenus()
    {
        CloseAllGameMenus();
        CloseAllUnitMenus();
    }

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

    public void SetWinMenuVisibility(bool visibility)
    {
        CloseAllGameMenus();
        SetUIElementVisibility(WinMenu, visibility);
    }

    public void SetGameOverMenuVisibility(bool visibility)
    {
        CloseAllGameMenus();
        SetUIElementVisibility(GameOverMenu, visibility);
    }

    public void SetPauseMenuVisibility(bool visibility)
    {
        CloseAllGameMenus();
        SetUIElementVisibility(PauseMenu, visibility);
    }

    // Unit Menu Visibility
    public void SetUnitMenuVisibility(bool visibility)
    {
        SetUIElementVisibility(UnitMenu, visibility);
    }

    public void SetEnemyUnitMenuVisibility(bool visibility)
    {
        SetUIElementVisibility(EnemyUnitMenu, visibility);
    }

    public bool getIsUnitMenuOpen()
    {
        return GetIsUIElementVisible(UnitMenu);
    }

    public bool getIsEnemyUnitMenuOpen()
    {
        return GetIsUIElementVisible(EnemyUnitMenu);
    }

    // Pausing Game
    public void PauseGame(bool status)
    {
        //If status == true pause | if status == false unpause
        SetUIElementVisibility(PauseMenu, status);

        //When pause status is true change timescale to 0 (time stops)
        //when it's false change it back to 1 (time goes by normally)
        if (status)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
}

