using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UnitMenus : GameScreen
{
    /*************************************************************************
                               Unit Data
   ************************************************************************/
    private GameObject playerUnit;
    private GameObject enemyUnit;

    /*************************************************************************
                                Unit Menus
    ************************************************************************/
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


    /*************************************************************************
                               Unit Menu Buttons
   ************************************************************************/
    const string playerUnitAttackButtonName = "UnitAttackButton";
    const string playerUnitWaitButtonName = "UnitWaitButton";
    const string playerUnitCancelAttackButtonName = "UnitCancelAttackButton";
    const string playerUnitConfirmAttackButtonName = "UnitConfirmAttackButton";
    Button PlayerUnitAttackButton;
    Button PlayerUnitWaitButton;
    Button PlayerUnitCancelAttackButton;
    Button PlayerUnitConfirmAttackButton;

    /*************************************************************************
                            Combat Prediction Status
    ************************************************************************/
    private bool isInCombatPrediction = false;

    /*************************************************************************
                                Player Input
    ************************************************************************/
    private PlayerControls input = null;


    /*************************************************************************
                                   Lifecycles
    ************************************************************************/
    protected override void Awake()
    {
        base.Awake();

        input = new PlayerControls();

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
        ActionEventManager.onEnemyDeath += OnEnemyDeath;
        ActionEventManager.onDeath += OnPlayerUnitDeath;

        input.Enable();
        input.Player.ButtonWest.performed += ctx => ClickWestButton();
        input.Player.ButtonNorth.performed += ctx => ClickNorthButton();
    }
    // unsubscribe to an event
    private void OnDisable()
    {
        ActionEventManager.onEnemyDeath -= OnEnemyDeath;
        ActionEventManager.onDeath -= OnPlayerUnitDeath;

        input.Disable();
        input.Player.ButtonWest.performed -= ctx => ClickWestButton();
        input.Player.ButtonNorth.performed -= ctx => ClickNorthButton();
    }

    // Runs when the Square/X button on a controller is clicked
    private void ClickWestButton()
    {
        // Initiates combat is the Attack Button is visible
        if (GetIsUIElementVisible(PlayerUnitAttackButton))
        {
            PlayerAttack();
        }
        // Confirms combat if the Confirm Attack Button is visible
        else if (GetIsUIElementVisible(PlayerUnitConfirmAttackButton))
        {
            PlayerConfirmAttack();
        }
    }

    // Runs when the Triangle/Y button on a controller is clicked
    private void ClickNorthButton()
    {
        // Skips Unit turn if the Unit Wait Button is visible
        if (GetIsUIElementVisible(PlayerUnitWaitButton))
        {
            PlayerWait();
        }
        // Cancels combat initiation if the Cancel Attack Button is visible
        else if (GetIsUIElementVisible(PlayerUnitCancelAttackButton))
        {
            PlayerCancelAttack();
        }
    }

    /*************************************************************************
                                Menu Closing
    ************************************************************************/
    public override void CloseAllUnitMenus()
    {
        base.CloseAllUnitMenus();
        SetUIElementVisibility(PlayerUnitMenu, false);
        SetUIElementVisibility(EnemyUnitMenu, false);
    }

    /*************************************************************************
                                Unit Actions
    ************************************************************************/
    // Updates the Unit Menu info based on the player and enemy game object unit data.
    private void UpdateUnitMenus()
    {
        SetPlayerUnitMenuInfo(GetUnitAttributes(playerUnit));
        SetEnemyUnitMenuInfo(GetUnitAttributes(enemyUnit));
    }

    // Takes the unit gme object off of the passed tile (If one exists) and uses it to update the Unit menus if necessary
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

    // Takes a game object (That is presumed to be for a unit) and using the unit's attributes, updates the Unit Menu depending
    // on if the unit is for the player or is an enemy
    public void HandleUnitByGameObject(GameObject gameObject)
    {
        if (!gameObject) return;

        UnitAttributes selectedUnit = GetUnitAttributes(gameObject);

        // If the Unit is a player Unit
        if (gameObject.CompareTag("Player"))
        {
            playerUnit = gameObject;
            SetPlayerUnitMenuInfo(selectedUnit);
            SetPlayerUnitMenuVisibility(true);
        }
        // If the Unit is an enemy Unit.
        else if (gameObject.CompareTag("Enemy"))
        {
            enemyUnit = gameObject;
            SetEnemyUnitMenuInfo(selectedUnit);
            SetEnemyUnitMenuVisibility(true);
        }

        SetPlayerUnitMenuInfo(GetUnitAttributes(playerUnit));
        SetEnemyUnitMenuInfo(GetUnitAttributes(enemyUnit));
    }

    // Returns the Unit Attributes of the passed game object (That is presumed to be for a unit)
    private UnitAttributes GetUnitAttributes(GameObject gameObject)
    {
        if (!gameObject) return null;

        return gameObject.GetComponent<UnitAttributes>();
    }

    /*************************************************************************
                            Player Unit Actions
    ************************************************************************/
    // Handles showing and hiding the player Unit Menu
    public void SetPlayerUnitMenuVisibility(bool visibility)
    {
        SetUIElementVisibility(PlayerUnitMenu, visibility);

        // If the player Unit Menu is being hidden, empty the player unit data
        if (!visibility) playerUnit = null;

        // Update the Unit Menu Container's spacing depending on what Unit Menu(s) are visible
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

        // TODO: Maybe hide enemy Unit Menu if the player Unit Menu is hidden, since if the player Unit Menu is
        // hidden then the player Unit may have died, so the enemy Unit they were fighting shouldn't have their
        // menu visible
    }

    // Updates the player Unit Menu based on the passed unit attributes
    public void SetPlayerUnitMenuInfo(UnitAttributes unit)
    {
        if (!unit) return;

        // Sets Unit name
        PlayerUnitName.text = unit.whatClass;
        // Sets Unit health text
        PlayerUnitHealth.text = unit.GetHealth() + "/" + unit.GetMaxHealth();

        // Sets the visibility of the player Unit Menu's action buttons
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

    /*************************************************************************
                        Player Unit Click Events
    ************************************************************************/
    private void ClickPlayerUnitAttackButton(ClickEvent evt)
    {
        if (!GetIsUIElementVisible(PlayerUnitAttackButton)) return;

        PlayerAttack();
    }

    // Initiates combat predicition
    private void PlayerAttack()
    {
        // Enter Combat Predicition mode
        isInCombatPrediction = true;

        // Update the player Unit Menu because the visibility of some action buttons are dependent on the Combat Predicition status 
        SetPlayerUnitMenuInfo(GetUnitAttributes(playerUnit));
    }

    private void ClickPlayerUnitWaitButton(ClickEvent evt)
    {
        if (!GetIsUIElementVisible(PlayerUnitWaitButton)) return;

        PlayerWait();
    }

    // Skips the player Unit's turn
    private void PlayerWait()
    {
        if (!playerUnit) return;

        // Updates the player Unit to have acted for their turn
        GetUnitAttributes(playerUnit).SetActed(true);
    }


    private void ClickPlayerUnitCancelAttackButton(ClickEvent evt)
    {
        if (!GetIsUIElementVisible(PlayerUnitCancelAttackButton)) return;

        PlayerCancelAttack();
    }

    // Cancels combay predicition
    private void PlayerCancelAttack()
    {
        // Exit Combat Predicition mode
        isInCombatPrediction = false;

        // Updates the Unit Menus
        UpdateUnitMenus();
    }

    private void ClickPlayerUnitConfirmAttackButton(ClickEvent evt)
    {
        if (!GetIsUIElementVisible(PlayerUnitConfirmAttackButton)) return;

        PlayerConfirmAttack();
    }

    // Confirms attack
    private void PlayerConfirmAttack()
    {
        if (!isInCombatPrediction || !playerUnit || !enemyUnit) return;

        // Perform combat
        actionEvent.attackBattle(playerUnit, enemyUnit);

        // Updates the player Unit to have acted for their turn
        GetUnitAttributes(playerUnit).SetActed(true);

        // Exit Combat Predicition mode
        isInCombatPrediction = false;

        // Updates the Unit Menus
        UpdateUnitMenus();
    }

    /*************************************************************************
                        Enemy Unit Actions
    ************************************************************************/
    // Handles showing and hiding the enemy Unit Menu
    public void SetEnemyUnitMenuVisibility(bool visibility)
    {
        SetUIElementVisibility(EnemyUnitMenu, visibility);

        // If the enemy Unit Menu is being hidden, empty the enemy unit data
        if (!visibility) enemyUnit = null;

        // Update the Unit Menu Container's spacing depending on what Unit Menu(s) are visible
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

        // Updates the player Unit Menu in response to the enemy Unit being removed
        // This may affect things like the visibility of the player Unit's 'Attack' button
        SetPlayerUnitMenuInfo(GetUnitAttributes(playerUnit));
    }

    // Updates the enemy Unit Menu based on the passed unit attributes
    public void SetEnemyUnitMenuInfo(UnitAttributes unit)
    {
        if (!unit) return;

        // Sets Unit name
        EnemyUnitName.text = unit.whatClass;
        // Sets Unit health text
        EnemyUnitHealth.text = unit.GetHealth() + "/" + unit.GetMaxHealth();
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