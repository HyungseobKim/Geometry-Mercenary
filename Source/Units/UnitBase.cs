/*!*******************************************************************
\file         UnitBase.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         05/09/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(UnitControl))]

/*!*******************************************************************
\class        UnitBase
\brief        Base class for all actual class which having implementation 
              for abilities and behaviors, so provide interface to unit controller.
********************************************************************/
public class UnitBase : MonoBehaviour
{
    //  4 5
    // 2 x 3
    //  0 1
    // adjacentX[y&1, i]
    public static int[,] adjacentX { get; } = new int[,] { { -1, 0, -1, 1, -1, 0 }, { 0, 1, -1, 1, 0, 1 } }; //! Helper array about x-coordinates to the adjacent tiles.
    public static int[] adjacentY { get; } = new int[] { -1, -1, 0, 0, 1, 1 }; //! Helper array about y-coordinates to the adjacent tiles.

    protected Status status; //! Status of this unit.
    protected UnitControl unitControl; //! Reference to controller of this unit.

    [HideInInspector]
    public GameObject targetEnemy = null; //! Current target enemy of this unit.
    [HideInInspector]
    public GameObject targetAlly = null; //! Current target ally of this unit.

    protected List<GameObject> EnemyList; //! List of enemies of this unit.
    protected List<GameObject> AllyList; //! List of allies of this unit.

    protected static BattleUIManager battleUIManager; //! Reference to battle UI manager.

    protected Dictionary<Vector3Int, double> eachTileScores = new Dictionary<Vector3Int, double>(); //! Container for scores of each tile.

    protected static Tilemap tilemap; //! Reference of Unity Tilemap which is being used for current game.
    protected static TileManager tileManager; //! Instance of tile manager.

    public GameObject projectilePrefab; //! Prefab of projectile for attack.
    public GameObject abilityProjectilePrefab; //! Prefab of projectile for ability.

    // Variables for when this unit poisoned.
    [HideInInspector]
    public bool poisonHeal = false;
    [HideInInspector]
    public bool poisonMove = false;
    [HideInInspector]
    public bool poisonAttack = false;

    [HideInInspector]
    public Dazzle dazzle = null; //! Dazzle ally near this unit.
    [HideInInspector]
    public bool shallowGrave = false; //! It's death is postphoned for now.

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    /*!
     * \brief Initialize method which must be called from every derived classes in the Start method.
     */
    public virtual void Initialize()
    {
        status = gameObject.GetComponent<Status>();
        unitControl = gameObject.GetComponent<UnitControl>();

        if (tileManager == null)
            tileManager = TileManager.instance;

        if (tilemap == null)
            tilemap = tileManager.tilemap;

        if (battleUIManager == null)
            battleUIManager = BattleUIManager.instance;

        // Match position to the center of cell
        status.cellPos = tilemap.WorldToCell(transform.position);
        transform.position = tilemap.GetCellCenterWorld(status.cellPos);

        // Set color for each team
        if (status.enemy)
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 48.0f/255.0f, 0, 1);
        else
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 135.0f/255.0f, 1, 1);

        UnitManager.instance.Register(gameObject);

        tileManager.cells[status.cellPos].owner = gameObject;
    }

    /*!
     * \brief When this unit can act, this method will be called.
     */
    public virtual void TakeTurn()
    {
        eachTileScores.Clear();

        // Find best enemy & ally.
        ComputeEnemyScore();
        ComputeAllyScore();
        ComputeThreatenScore();

        bool attacked = false;

        // If best enemy is near, attack.
        if (CanAttack(targetEnemy))
        {
            Attack(targetEnemy);
            attacked = true;
        }

        Vector3Int nextPos = eachTileScores.Aggregate((lhs, rhs) => lhs.Value > rhs.Value ? lhs : rhs).Key;

        // If staying is not the best, move.
        if (MoveTo(nextPos) < 0)
            return; // Ambush.

        // Already attacked, so finish the turn.
        if (attacked == true)
            return;

        // If best enemy is near, attack and finish the turn.
        if (CanAttack(targetEnemy))
        {
            Attack(targetEnemy);
            return;
        }

        // Find enemey nearby
        GameObject target = FindEnemyOnAdjacentTiles();

        // If there is, attack.
        if (target != null)
            Attack(target);
    }

    /*!
     * \brief Apply damage and check whether it is died.
     * 
     * \param amount
     *        Amount of damage.
     *        
     * \param enemy
     *        Enemy who attacked.
     *        
     * \param critical
     *        Is this critical hit?
     *        
     * \return bool       
     *         Return true if died
     *         Otherwise, false.
     */
    public virtual bool TakeDamage(float amount, GameObject enemy, bool critical = false)
    {
        // This unit has shield.
        if (status.UseShield())
            return false;

        // Reduce damage by protection.
        amount = Mathf.Clamp(amount - status.protection, 0, amount + Math.Abs(status.protection));

        // Reduce health.
        status.ChangeHealth(-amount);

        // Generate UI.
        battleUIManager.CreateDamageUI(gameObject, amount, critical);

        // Play sound.
        AudioManager.instance.Play("Hit");

        return IsDied();
    }

    /*!
     * \brief This unit got poison attack.
     * 
     * \param totalDamage
     *        Total damage for poisoning.
     *        
     * \param time
     *        Total time to take poison damage.
     */
    public virtual void ApplyPoison(float totalDamage, float time)
    {
        gameObject.GetComponent<UnitControl>().ApplyPoison(totalDamage, time);
    }

    /*!
     * \brief Give damage to target.
     * 
     * \param target
     *        Target to give damage.
     *        
     * \return bool
     *         Return true if it succeeded.
     *         False otherwise.
     */
    public virtual bool ApplyDamage(GameObject target)
    {
        if (target == null)
            return false;

        return target.GetComponent<UnitBase>().TakeDamage(status.power, gameObject);
    }

    /*!
     * \brief Shoot projectile to target enemy.
     * 
     * \param target
     *        Target to attack.
     *        
     * \return bool
     *         Not used.
     */
    public virtual bool Attack(GameObject target)
    {
        if (target == null)
            return false;

        // If this unit was under special state.
        if (poisonAttack)
        {
            float damage = unitControl.GetTotalDamage();

            if (damage > status.health)
                damage = status.health - 1;
            
            // Take remained poison damage.
            status.ChangeHealth(-damage);
            battleUIManager.CreateDamageUI(gameObject, damage);

            // Finishes poison.
            unitControl.poisonDamage = 0.0f;
            unitControl.poisonTimeRemain = 0.0f;
            unitControl.poisonTimer = 0.0f;
            unitControl.PoisonEnd();
        }

        // Create projectile.
        GameObject projectile = UnityEngine.Object.Instantiate(projectilePrefab, gameObject.transform.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().Initialize(target, gameObject);

        return false;
    }

    /*!
     * \brief This unit get healed.
     * 
     * \param amount
     *        Amount of healing.
     */
    public virtual void GetHeal(float amount)
    {
        if (poisonHeal)
        {
            unitControl.ApplyPoison(amount * 2.0f, 1.0f);
            poisonHeal = false;
        }

        if (status.healBan > 0)
        {
            --status.healBan;
            amount = 0.0f;
        }

        // Store health before healing.
        float health = status.health;

        // Heal.
        status.ChangeHealth(amount);

        // Compare and show UI.
        battleUIManager.CreateHealUI(gameObject, status.health - health);

        // Play sound.
        AudioManager.instance.Play("Heal");
    }

    /*!
     * \brief Heal target ally.
     * 
     * \param target
     *        Ally to heal.
     */
    public virtual void ApplyHeal(GameObject target)
    {
        if (target == null)
            return;

        target.GetComponent<UnitBase>().GetHeal(status.power);
    }

    /*!
     * \brief Shoot healing projectile to target ally.
     * 
     * \param target
     *        Ally to heal.
     */
    protected virtual void HealAlly(GameObject target)
    {
        if (target == null)
            return;

        GameObject projectile = UnityEngine.Object.Instantiate(abilityProjectilePrefab, gameObject.transform.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().Initialize(target, gameObject);
    }

    /*!
     * \brief This unit get support.
     *        Increases extra turn.
     * 
     * \param amount
     *        The number of support.
     */
    public virtual void GetSupport(int amount)
    {
        unitControl.extraTurn += amount;

        // Play sound.
        AudioManager.instance.Play("Support");
    }

    /*!
     * \brief Defines behavior when a unit collides to some special projectiles.
     * 
     * \param projectile
     *        Projectile that collided.
     */
    public virtual bool InteractWithProjectile(GameObject projectile)
    {
        // do nothing as default
        return false;
    }

    /*!
     * \brief How much is this unit dangerous to given unit.
     * 
     * \param scorer
     *        The enemy requested danger value.
     */
    public virtual double GetDangerValue (GameObject scorer)
    {
        return 0.3;
    }

    /*!
     * \brief How much is this unit helpful to given unit.
     * 
     * \param scorer
     *        The ally requested ally value.
     */
    public virtual double GetAllyValue(GameObject scorer)
    {
        return 0.3;
    }

    /*!
     * \brief Check whether this unit can attack given enemy.
     * 
     * \param enemy
     *        Target to attack.
     *        
     * \return bool
     *         Return true if it can attack given enemy.
     */
    public virtual bool CanAttack(GameObject enemy)
    {
        if (enemy == null)
            return false;

        Status enemyStatus = enemy.GetComponent<Status>();

        // Invalid object
        if (enemyStatus == null)
            return false;
        
        // Ally
        if (enemyStatus.enemy == status.enemy)
            return false;

        // Stealth
        if (enemyStatus.stealth == true)
            return false;

        // Out of range
        Vector3Int enemyPos = tilemap.WorldToCell(enemy.GetComponent<Transform>().position);
        if (TileManager.instance.DistanceBetween(enemyPos, status.cellPos) > status.attackRange)
            return false;

        return true;
    }

    /*!
     * \brief Check whether this unit can use ability.
     * 
     * \param target
     *        Target to use ability.
     * 
     * \param toAlly
     *        Is ability of this unit for ally?
     * 
     * \return bool
     *         Return true if it can use ability to given unit.
     */
    protected virtual bool CanUseAbility(GameObject target, bool toAlly)
    {
        if (target == null)
            return false;

        Status targetStatus = target.GetComponent<Status>();

        // Invalid object
        if (targetStatus == null)
            return false;

        // Invalid target
        if (targetStatus.enemy == status.enemy)
        {
            if (toAlly == false)
                return false;
        }
        else
        {
            if (toAlly == true)
                return false;
            if (targetStatus.stealth == true)
                return false;
        }

        if (target == gameObject)
            return false;

        // Out of range
        Vector3Int targetPos = target.GetComponent<Status>().cellPos;
        if (TileManager.instance.DistanceBetween(targetPos, status.cellPos) > status.abilityRange)
            return false;

        return true;
    }

    /*!
     * \brief Check whether this unit is died or not.
     * 
     * \return bool
     *         Return true if it is died.
     */
    public bool IsDied()
    {
        // Alive.
        if (status.health > 0.0f)
            return false;

        // Resurrected.
        if (ResurrectionPlaymaker.Resurrect(gameObject))
            return false;

        // Death is delayed.
        if (shallowGrave)
            return false;
        else if (dazzle)
        {
            // Turn on the shallow grave.
            dazzle.ShallowGrave(gameObject);
            return false;
        }

        tileManager.cells[status.cellPos].owner = null;
        UnitManager.instance.ReportDeath(gameObject);

        Destroy(gameObject);
        return true;
    }

    /*!
     * \brief Check adjacent tiles and find enemy.
     * 
     * \return GameObject
     *         Return enemy near this unit.
     *         If there is none, return null.
     */
    protected GameObject FindEnemyOnAdjacentTiles()
    {
        int row = status.cellPos.y & 1;
        GameObject enemy = null;

        for (int i = 0; i < 6; ++i)
        {
            enemy = GetEnemyOn(new Vector3Int(adjacentX[row, i], adjacentY[i], 0) + status.cellPos);
            if (enemy != null && enemy.GetComponent<Status>().stealth == false)
                return enemy;
        }

        return null;
    }

    /*!
     * \brief Find an ally on adjacent tiles.
     * 
     * \return GameObject
     *         Return any ally near this unit.
     *         If there is none, return null.
     */
    protected virtual GameObject FindAllyOnAdjacentTiles()
    {
        int row = status.cellPos.y & 1;

        for (int i = 0; i < 6; ++i)
        {
            Vector3Int cellPos = new Vector3Int(adjacentX[row, i], adjacentY[i], 0) + status.cellPos;
            Cell cell;

            if (tileManager.cells.TryGetValue(cellPos, out cell) == false)
                continue;

            if (cell.owner != null && cell.owner.GetComponent<Status>().enemy == status.enemy)
                return cell.owner;
        }

        return null;
    }

    /*!
     * \brief Compute score for every enemy.
     * 
     * \return double
     *         The highest score.
     */
    protected virtual double ComputeEnemyScore()
    {
        if (status.enemy == true)
            EnemyList = UnitManager.instance.playerUnits;
        else
            EnemyList = UnitManager.instance.enemyUnits;

        Cell currentCell = tileManager.cells[status.cellPos];

        GameObject target = null;
        double enemyScore = -1.0;

        foreach (GameObject enemy in EnemyList)
        {
            // Get unique danger value of enemy
            double score = enemy.GetComponent<UnitBase>().GetDangerValue(gameObject) + status.aggression;

            Status enemyStatus = enemy.GetComponent<Status>();

            // Enemy cannot be detected.
            if (enemyStatus.stealth == true)
                continue;

            Cell enemyCell = tileManager.cells[enemyStatus.cellPos];
            
            int distance = Pathfinder.instance.FindPath(currentCell, enemyCell, status.attackRange, status.enemy);

            // If enemy is far away, lower the score
            if (distance < 0 || distance > tileManager.maxLength)
                score = 0.0; // No path OR too far
            else
                score *= Math.Sqrt(1.0 - ((double)distance / (double)(tileManager.maxLength - status.attackRange)));

            // If enemy has negative protection, increases score.
            if (enemyStatus.protection < -1)
                score = Mathematics.GoguensTconorm(score, 1.0 + (1.0 / (double)enemyStatus.protection));
            else if (enemyStatus.protection > 1) // If enemy has prositive protection, decreases score.
                score *= (1.0 / Math.Pow(enemyStatus.protection, 0.1));

            // Increases score depending on enemy's health. It never decreases the score.
            score = Mathematics.GoguensTconorm(score, 1.0 - (double)enemyStatus.health / (double)enemyStatus.maxHealth);

            // Store best enemy
            if (score > enemyScore)
            {
                target = enemy;
                enemyScore = score;
            }

            // Store next position value
            Vector3Int index = Pathfinder.instance.GetNextPos(currentCell, enemyCell);
            if (eachTileScores.ContainsKey(index))
                eachTileScores[index] = Mathematics.GoguensTconorm(eachTileScores[index], score);
            else
                eachTileScores.Add(index, score);
        }

        // Store target
        targetEnemy = target;
        return enemyScore;
    }

    /*!
     * \brief Compute score for every ally.
     * 
     * \return double
     *         The highest score.
     */
    protected virtual double ComputeAllyScore()
    {
        if (status.enemy == true)
        {
            AllyList = UnitManager.instance.enemyUnits;
            EnemyList = UnitManager.instance.playerUnits;
        }
        else
        {
            AllyList = UnitManager.instance.playerUnits;
            EnemyList = UnitManager.instance.enemyUnits;
        }

        if (EnemyList.Count == 1)
            return 0.0;

        Cell currentCell = tileManager.cells[status.cellPos];
        double healthScore = (double)status.health / (double)status.maxHealth;

        GameObject target = null;
        double allyScore = -1.0;

        double allyCountValue = (double)(AllyList.Count - 1) / (double)(AllyList.Count + EnemyList.Count);

        foreach (GameObject ally in AllyList)
        {
            if (ally == gameObject)
                continue;

            // Get unique ally value of enemy
            double score = ally.GetComponent<UnitBase>().GetAllyValue(gameObject) - status.aggression;
            score *= allyCountValue; // Decreases score by total number of allies.

            // If ally is healer, increases score depending on lost health.
            if (ally.GetComponent<Healer>() && ally.GetComponent<Priest>() == null)
                score = Mathematics.GoguensTconorm(score, 1.0 - healthScore);
            
            Status allyStatus = ally.GetComponent<Status>();
            Cell allyCell = tileManager.cells[allyStatus.cellPos];

            // If ally is far away, lower the score
            int distance = Pathfinder.instance.FindPath(currentCell, allyCell, 1, status.enemy);
            if (distance < 0 || distance > tileManager.maxLength + status.movement)
                score = 0.0; // No path OR too far
            else
                score *= Math.Sqrt(1.0 - (distance / (tileManager.maxLength + status.movement)));

            // Store best ally
            if (score > allyScore)
            {
                target = ally;
                allyScore = score;
            }

            // Store next position value
            Vector3Int index = Pathfinder.instance.GetNextPos(currentCell, allyCell);
            if (eachTileScores.ContainsKey(index))
                eachTileScores[index] = Mathematics.GoguensTconorm(eachTileScores[index], score);
            else
                eachTileScores.Add(index, score);
        }

        targetAlly = target;
        return allyScore;
    }

    /*!
     * \brief Check every adjacent tiles, and determine how much is each tile dangerous.
     */
    protected virtual void ComputeThreatenScore()
    {
        int row = status.cellPos.y & 1;
        Cell cell;

        double score;

        for (int i = 0; i < 6; ++i)
        {
            Vector3Int cellPos = new Vector3Int(adjacentX[row, i], adjacentY[i], 0) + status.cellPos;

            if (tileManager.cells.TryGetValue(cellPos, out cell) == false)
                continue;

            // Cannot move to here
            if (cell.owner != null)
                continue;

            score = -NumberofAdjacentEnemies(cellPos) / 6.0;

            // Store next position value
            if (eachTileScores.ContainsKey(cellPos))
                eachTileScores[cellPos] = Mathematics.GoguensTconorm(eachTileScores[cellPos], score);
            else
                eachTileScores.Add(cellPos, score);
        }

        // Score for current cell.
        score = -NumberofAdjacentEnemies(status.cellPos) / 6.0;
        if (eachTileScores.ContainsKey(status.cellPos))
            eachTileScores[status.cellPos] = Mathematics.GoguensTconorm(eachTileScores[status.cellPos], score);
        else
            eachTileScores.Add(status.cellPos, score);
    }

    /*!
     * \brief Find number of enemies on adjacent tiles.
     * 
     * \param position
     *        Position to check in cell coordinates.
     *        
     * \return int
     *         The number of adjacent enemies.
     */
    protected int NumberofAdjacentEnemies(Vector3Int position)
    {
        int row = position.y & 1;
        Cell cell;

        int result = 0;

        for (int i = 0; i < 6; ++i)
        {
            Vector3Int cellPos = new Vector3Int(adjacentX[row, i], adjacentY[i], 0) + position;

            if (tileManager.cells.TryGetValue(cellPos, out cell) == false)
                continue;

            if (cell.owner == null)
                continue;

            Status enemyStatus = cell.owner.GetComponent<Status>();

            if (enemyStatus.enemy == status.enemy)
                continue;

            if (enemyStatus.stealth == true)
                continue;

            ++result;
        }

        return result;
    }

    /*!
     * \brief Find number of allies on adjacent tiles.
     * 
     * \param position
     *        Position to check in cell coordinates.
     *        
     * \return int
     *         The number of adjacent allies.
     */
    protected int NumberofAdjacentAllies(Vector3Int position)
    {
        int row = position.y & 1;
        Cell cell;

        int result = 0;

        for (int i = 0; i < 6; ++i)
        {
            Vector3Int cellPos = new Vector3Int(adjacentX[row, i], adjacentY[i], 0) + position;

            if (tileManager.cells.TryGetValue(cellPos, out cell) == false)
                continue;

            if (cell.owner == null)
                continue;

            if (cell.owner.GetComponent<Status>().enemy == status.enemy)
                ++result;
        }

        return result;
    }

    /*!
     * \brief Get enemy on given tile.
     * 
     * \param cellPos
     *        Position of tile in grid coordinate.
     *        
     * \return GameObject
     *         Return enemy at this location.
     *         Return null if there is none or invalid.
     */
    private GameObject GetEnemyOn(Vector3Int cellPos)
    {
        Cell cell;

        // There is no such tile
        if (tileManager.cells.TryGetValue(cellPos, out cell) == false)
            return null;

        // Empty cell
        if (cell.owner == null)
            return null;

        // Ally
        if (cell.owner.GetComponent<Status>().enemy == status.enemy)
            return null;

        return cell.owner;
    }

    /*!
     * \brief Return description of base ability.
     */
    public virtual string GetBaseAbilityDescription()
    {
        return "-";
    }

    /*!
     * \brief Return first upgraded of base ability.
     */
    public virtual string GetSecondAbilityDescription()
    {
        return "-";
    }

    /*!
     * \brief Return second upgraded of base ability.
     */
    public virtual string GetThirdAbilityDescription()
    {
        return "-";
    }

    /*!
     * \brief Make this unit move to given position.
     * 
     * \param cellPos
     *        Position to move in grid coordinate.
     *        Must be adjacent.
     *        
     * \return int
     *         Return 1 if it succeeded to move.
     *         Return 0 it failed.
     *         Return -1, if there was ambush.
     */
    public virtual int MoveTo(Vector3Int cellPos)
    {
        if (cellPos == status.cellPos)
            return 0;

        Cell cell;

        if (tileManager.cells.TryGetValue(cellPos, out cell) == false)
            return 0;

        if (status.enemy)
        {
            if (cell.blockEnemyUnits > 0)
                return 0;
        }
        else
        {
            if (cell.blockPlayerUnits > 0)
                return 0;
        }

        if (cell.owner != null)
        {
            Status enemyStatus = cell.owner.GetComponent<Status>();

            // There was an ambush!
            if (enemyStatus.enemy != status.enemy && enemyStatus.stealth == true)
            {
                battleUIManager.CreateAmbushUI(cell.owner);
                cell.owner.GetComponent<Assassin>().Destealth();
                unitControl.MoveAndMeetAmbush(cell.worldPos, cell.owner);
                return -1;
            }

            return 0;
        }

        tileManager.cells[status.cellPos].owner = null;
        cell.owner = gameObject;

        status.cellPos = cellPos;
        unitControl.MoveTo(cell.worldPos);

        if (poisonMove)
        {
            unitControl.ApplyPoison(status.power + status.speed, 1.0f);
            poisonMove = false;
        }
        
        return 1;
    }

    /*!
     * \brief Make this unit move to given position.
     * 
     * \param cellPos
     *        Position to move in grid coordinate.
     */
    public virtual void ForceToMoveTo(Vector3Int cellPos)
    {
        // Same position.
        if (cellPos == status.cellPos)
            return;

        Cell cell;

        // Out of grid.
        if (tileManager.cells.TryGetValue(cellPos, out cell) == false)
            return;

        // Do not override other object.
        if (tileManager.cells[status.cellPos].owner == gameObject)
            tileManager.cells[status.cellPos].owner = null;

        cell.owner = gameObject;

        status.cellPos = cellPos;
        unitControl.MoveTo(cell.worldPos);
    }
}
