using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameScreen : MonoBehaviour
{
    protected UIDocument MainMenuDocument;
    protected VisualElement root;

    protected UIManager uiManager;
    protected ActionEventManager actionEvent;


    // Game Menus
    const string gameOverMenuName = "GameOverMenu";
    const string winMenuName = "WinMenu";
    // const string pauseMenuName = "PauseMenu";
    VisualElement GameOverMenu;
    VisualElement WinMenu;
    // VisualElement PauseMenu;

    // Game Menu Buttons
    const string winNextLevelButtonName = "WinNextLevelButton";
    const string winQuitButtonName = "WinQuitButton";
    const string gameOverRetryButtonName = "GameOverRetryButton";
    const string gameOverQuitButtonName = "GameOverQuitButton";
    // const string pauseResumeButtonName = "PauseResumeButton";
    // const string pauseQuitButtonName = "PauseQuitButton";
    Button WinNextLevelButton;
    Button WinQuitButton;
    Button GameOverRetryButton;
    Button GameOverQuitButton;
    // Button PauseResumeButton;
    // Button PauseQuitButton;

    // Unit Menu
    const string unitMenusContainerName = "UnitMenus";
    const string playerUnitMenuName = "UnitMenu";
    const string playerUnitName = "UnitNameText";
    const string playerUnitHealthName = "UnitHPText";
    const string enemyUnitMenuName = "EnemyUnitMenu";
    const string enemyUnitName = "EnemyUnitNameText";
    const string enemyUnitHealthName = "EnemyUnitHPText";
    VisualElement UnitMenusContainer;
    VisualElement PlayerUnitMenu;
    Label PlayerUnitName;
    Label PlayerUnitHealth;
    VisualElement EnemyUnitMenu;
    Label EnemyUnitName;
    Label EnemyUnitHealth;

    private GameObject playerUnit;
    private GameObject enemyUnit;


    // Unit Menu Buttons
    const string playerUnitAttackButtonName = "UnitAttackButton";
    const string playerUnitWaitButtonName = "UnitWaitButton";
    const string playerUnitCancelAttackButtonName = "UnitCancelAttackButton";
    const string playerUnitConfirmAttackButtonName = "UnitConfirmAttackButton";
    Button PlayerUnitAttackButton;
    Button PlayerUnitWaitButton;
    Button PlayerUnitCancelAttackButton;
    Button PlayerUnitConfirmAttackButton;

    private bool isInCombatPrediction = false;

    protected virtual void Awake()
    {
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        actionEvent = GameObject.FindGameObjectWithTag("ActionEvent").GetComponent<ActionEventManager>();

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

        // Game Menu Buttons
        WinNextLevelButton = root.Q<Button>(winNextLevelButtonName);
        WinQuitButton = root.Q<Button>(winQuitButtonName);
        GameOverRetryButton = root.Q<Button>(gameOverRetryButtonName);
        GameOverQuitButton = root.Q<Button>(gameOverQuitButtonName);

        // Game Menu Click Events
        WinNextLevelButton?.RegisterCallback<ClickEvent>(ClickNextLevelButton);
        WinQuitButton?.RegisterCallback<ClickEvent>(ClickQuitButton);
        GameOverRetryButton?.RegisterCallback<ClickEvent>(ClickRetryButton);
        GameOverQuitButton?.RegisterCallback<ClickEvent>(ClickQuitButton);

        // Unit Menus
        UnitMenusContainer = root.Q<VisualElement>(unitMenusContainerName);
        PlayerUnitMenu = root.Q<VisualElement>(playerUnitMenuName);
        PlayerUnitName = root.Q<Label>(playerUnitName);
        PlayerUnitHealth = root.Q<Label>(playerUnitHealthName);
        EnemyUnitMenu = root.Q<VisualElement>(enemyUnitMenuName);
        EnemyUnitName = root.Q<Label>(enemyUnitName);
        EnemyUnitHealth = root.Q<Label>(enemyUnitHealthName);

        // Unit Menu Buttons
        PlayerUnitAttackButton = root.Q<Button>(playerUnitAttackButtonName);
        PlayerUnitWaitButton = root.Q<Button>(playerUnitWaitButtonName);
        PlayerUnitCancelAttackButton = root.Q<Button>(playerUnitCancelAttackButtonName);
        PlayerUnitConfirmAttackButton = root.Q<Button>(playerUnitConfirmAttackButtonName);
        // Unit Menu Click Events
        PlayerUnitAttackButton?.RegisterCallback<ClickEvent>(ClickPlayerUnitAttackButton);
        PlayerUnitWaitButton?.RegisterCallback<ClickEvent>(ClickPlayerUnitWaitButton);
        PlayerUnitCancelAttackButton?.RegisterCallback<ClickEvent>(ClickPlayerUnitCancelAttackButton);
        PlayerUnitConfirmAttackButton?.RegisterCallback<ClickEvent>(ClickPlayerUnitConfirmAttackButton);
        CloseAllMenus();
    }

    private void OnEnable()
    {
        ActionEventManager.OnEnemyDeath += OnEnemyDeath;
        ActionEventManager.OnDeath += OnPlayerUnitDeath;
    }
    // unsubscribe to an event
    private void OnDisable()
    {
        ActionEventManager.OnEnemyDeath -= OnEnemyDeath;
        ActionEventManager.OnDeath -= OnPlayerUnitDeath;
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

    protected void ClickQuitButton(ClickEvent evt)
    {
        uiManager.Quit();
    }



    // UI ELement Visibility
    public virtual void CloseAllGameMenus()
    {
        SetUIElementVisibility(WinMenu, false);
        SetUIElementVisibility(GameOverMenu, false);
        // SetUIElementVisibility(PauseMenu, false);
    }

    public void CloseAllUnitMenus()
    {
        SetUIElementVisibility(PlayerUnitMenu, false);
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

    // public void SetPauseMenuVisibility(bool visibility)
    // {
    // CloseAllGameMenus();
    // SetUIElementVisibility(PauseMenu, visibility);
    // }

    // Pausing Game
    // public void PauseGame(bool status)
    // {
    //If status == true pause | if status == false unpause
    // // SetUIElementVisibility(PauseMenu, status);

    //When pause status is true change timescale to 0 (time stops)
    //when it's false change it back to 1 (time goes by normally)
    // if (status)
    // Time.timeScale = 0;
    // else
    // Time.timeScale = 1;
    // }

    /*************************************************************************
                                    Unit Menus
    ************************************************************************/
    /* UNIT MENUS */
    private void UpdateUnitMenus()
    {
        SetPlayerUnitMenuInfo(GetUnitAttributes(playerUnit));
        SetEnemyUnitMenuInfo(GetUnitAttributes(enemyUnit));
    }

    public void HandleUnitByTile(Tile tile)
    {
        if (tile._occupied)
        {
            HandleUnitByGameObject(tile._unit);
        }
        else
        {
            SetPlayerUnitMenuVisibility(false);
            SetEnemyUnitMenuVisibility(false);
        }
    }

    public void HandleUnitByGameObject(GameObject gameObject)
    {
        if (!gameObject) return;

        UnitAttributes selectedUnit = GetUnitAttributes(gameObject);

        if (gameObject.CompareTag("Player"))
        {
            playerUnit = gameObject;
            SetPlayerUnitMenuInfo(selectedUnit);
            SetPlayerUnitMenuVisibility(true);
        }
        else if (gameObject.CompareTag("Enemy"))
        {
            enemyUnit = gameObject;
            SetEnemyUnitMenuInfo(selectedUnit);
            SetEnemyUnitMenuVisibility(true);
        }

        SetPlayerUnitMenuInfo(GetUnitAttributes(playerUnit));
        SetEnemyUnitMenuInfo(GetUnitAttributes(enemyUnit));
    }

    private UnitAttributes GetUnitAttributes(GameObject gameObject)
    {
        if (!gameObject) return null;

        return gameObject.GetComponent<UnitAttributes>();
    }

    /* PLAYER UNIT */
    public void SetPlayerUnitMenuVisibility(bool visibility)
    {
        SetUIElementVisibility(PlayerUnitMenu, visibility);

        if (!visibility) playerUnit = null;

        if (visibility && GetIsEnemyUnitMenuOpen())
        {
            UnitMenusContainer.style.justifyContent = Justify.SpaceBetween;
        }
        else if (visibility && !GetIsEnemyUnitMenuOpen())
        {
            UnitMenusContainer.style.justifyContent = Justify.FlexEnd;
        }
        else if (!visibility && GetIsEnemyUnitMenuOpen())
        {
            UnitMenusContainer.style.justifyContent = Justify.FlexStart;
        }
    }

    public void SetPlayerUnitMenuInfo(UnitAttributes unit)
    {
        if (!unit) return;

        PlayerUnitName.text = unit.whatClass;
        PlayerUnitHealth.text = unit.GetHealth() + "/" + unit.GetMaxHealth();

        SetUIElementVisibility(PlayerUnitAttackButton, !unit.HasActed() && enemyUnit && GetIsEnemyUnitMenuOpen() && !isInCombatPrediction);
        SetUIElementVisibility(PlayerUnitWaitButton, !unit.HasActed() && !isInCombatPrediction);
        SetUIElementVisibility(PlayerUnitConfirmAttackButton, !unit.HasActed() && enemyUnit && GetIsEnemyUnitMenuOpen() && isInCombatPrediction);
        SetUIElementVisibility(PlayerUnitCancelAttackButton, !unit.HasActed() && enemyUnit && GetIsEnemyUnitMenuOpen() && isInCombatPrediction);
    }

    public bool GetIsPlayerUnitMenuOpen()
    {
        return GetIsUIElementVisible(PlayerUnitMenu);
    }

    private void OnPlayerUnitDeath()
    {
        SetPlayerUnitMenuVisibility(false);
    }

    // Player Unit Click Events
    private void ClickPlayerUnitAttackButton(ClickEvent evt)
    {
        isInCombatPrediction = true;

        SetPlayerUnitMenuInfo(GetUnitAttributes(playerUnit));
    }

    private void ClickPlayerUnitWaitButton(ClickEvent evt)
    {
        if (!playerUnit) return;

        GetUnitAttributes(playerUnit).SetActed(true);
    }

    private void ClickPlayerUnitCancelAttackButton(ClickEvent evt)
    {
        isInCombatPrediction = false;
        UpdateUnitMenus();
    }

    private void ClickPlayerUnitConfirmAttackButton(ClickEvent evt)
    {
        if (!isInCombatPrediction || !playerUnit || !enemyUnit) return;

        actionEvent.attackBattle(playerUnit, enemyUnit);

        GetUnitAttributes(playerUnit).SetActed(true);
        isInCombatPrediction = false;
        UpdateUnitMenus();
    }

    /* ENEMY UNIT */
    public void SetEnemyUnitMenuInfo(UnitAttributes unit)
    {
        if (!unit) return;

        EnemyUnitName.text = unit.whatClass;
        EnemyUnitHealth.text = unit.GetHealth() + "/" + unit.GetMaxHealth();
    }

    public void SetEnemyUnitMenuVisibility(bool visibility)
    {
        SetUIElementVisibility(EnemyUnitMenu, visibility);

        if (!visibility) enemyUnit = null;

        if (visibility && GetIsPlayerUnitMenuOpen())
        {
            UnitMenusContainer.style.justifyContent = Justify.SpaceBetween;
        }
        else if (visibility && !GetIsPlayerUnitMenuOpen())
        {
            UnitMenusContainer.style.justifyContent = Justify.FlexStart;
        }
        else if (!visibility && GetIsPlayerUnitMenuOpen())
        {
            UnitMenusContainer.style.justifyContent = Justify.FlexEnd;
        }

        SetPlayerUnitMenuInfo(GetUnitAttributes(playerUnit));
    }

    public bool GetIsEnemyUnitMenuOpen()
    {
        return GetIsUIElementVisible(EnemyUnitMenu);
    }

    private void OnEnemyDeath()
    {
        SetEnemyUnitMenuVisibility(false);
    }
}