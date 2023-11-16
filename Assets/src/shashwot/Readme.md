The PerformAction method in the Tile class is an example of a dynamically bound method. The method dynamically determines the type of the attached component (PlayerUnit or EnemyUnit) and calls the corresponding action method. Let's explore the scenarios you mentioned:

Dynamically Bound Method:
Current Dynamic Type: If _unit is of type PlayerUnit, then PerformPlayerAction will be called.

    if (unitComponent is PlayerUnit)
    {
        PlayerUnit playerUnit = (PlayerUnit)unitComponent;
        playerUnit.PerformPlayerAction();
    }
Change Dynamic Type: If _unit is of type EnemyUnit, then PerformEnemyAction will be called.

    else if (unitComponent is EnemyUnit)
    {
        EnemyUnit enemyUnit = (EnemyUnit)unitComponent;
        enemyUnit.PerformEnemyAction();
    }

Statically Bound Method:
Since C# is a statically typed language, methods that are called based on their declared type are statically bound. For example:
    If have a reference of type PlayerUnit and call a method, it will statically bind to the PlayerUnit class's method:
        PlayerUnit playerUnit = new PlayerUnit();
        playerUnit.PerformPlayerAction(); // Calls PerformPlayerAction statically

    If you have a reference of type EnemyUnit and call a method, it will statically bind to the EnemyUnit class's method:

        EnemyUnit enemyUnit = new EnemyUnit();
        enemyUnit.PerformEnemyAction(); // Calls PerformEnemyAction statically
        
In summary, dynamic binding allows the method to be determined at runtime based on the actual type of the object, while static binding determines the method at compile time based on the declared type of the reference.