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

    private bool isUnitMenuOpen = false;
    private bool isEnemyUnitMenuOpen = false;

    // Unit Menu Buttons
    const string unitAttackButtonName = "UnitAttackButton";
    const string unitWaitButtonName = "UnitWaitButton";
    const string unitCancelAttackButtonName = "UnitCancelAttackButton";
    const string unitConfirmAttackButtonName = "UnitConfirmAttackButton";
    Button UnitAttackButton;
    Button UnitWaitButton;
    Button UnitCancelAttackButton;
    Button UnitConfirmAttackButton;

    private void Awake()
    {
        closeAllMenus();
    }

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
    }

    // Game Menu Click Events
    private void ClickNextLevelButton(ClickEvent event)
    {
        uiManager.Restart();
    }

    private void ClickRetryButton(ClickEvent event) {
        uiManager.Restart();
    }

    private void ClickQuitButton(ClickEvent event) {
        uiManager.Quit();
    }

     private void ClickResumeButton(ClickEvent event) {
        // uiManager.Quit();
    }

    // Unit Menu Click Events
    private void ClickUnitAttackButton(ClickEvent event) {
    }

    private void ClickUnitWaitButton(ClickEvent event) {
    }

    private void ClickUnitCancelAttackButton(ClickEvent event) {
    }

    private void ClickUnitConfirmAttackButton(ClickEvent event) {
    }

    // UI ELement Visibility
    public void closeAllGameMenus()
    {
        SetUIElementVisibility(WinMenu, false);
        SetUIElementVisibility(GameOverMenu, false);
        SetUIElementVisibility(PauseMenu, false);
    }

    public void closeAllUnitMenus()
    {
        SetUIElementVisibility(UnitMenu, false);
        SetUIElementVisibility(EnemyUnitMenu, false);
    }

    public void closeAllMenus()
    {
        closeAllGameMenus();
        closeAllUnitMenus();
    }

    void SetUIElementVisibility(VisualElement uiElement, bool isVisible)
    {
        if (uiElement == null)
            return;

        uiElement.style.display = (isVisible) ? DisplayStyle.Flex : DisplayStyle.None;
    }

    public void SetWinMenuVisibility(bool visibility)
    {
        closeAllGameMenus();
        SetUIElementVisibility(WinMenu, visibility);
    }

    public void SetGameOverMenuVisibility(bool visibility)
    {
        closeAllGameMenus();
        SetUIElementVisibility(GameOverMenu, visibility);
    }

    public void SetPauseMenuVisibility(bool visibility)
    {
        closeAllGameMenus();
        SetUIElementVisibility(PauseMenu, visibility);
    }

    // Unit Menu Visibility
    public void SetUnitMenuVisibility(bool visibility)
    {
        SetUIElementVisibility(UnitMenu, visibility);
        isUnitMenuOpen = visibility;
    }

    public void SetEnemyUnitMenuVisibility(bool visibility)
    {
        SetUIElementVisibility(EnemyUnitMenu, visibility);
        isEnemyUnitMenuOpen = visibility;
    }

    public bool isUnitMenuOpen()
    {
        return isUnitMenuOpen;
    }

    public bool isEnemyUnitMenuOpen()
    {
        return isEnemyUnitMenuOpen;
    }
}

