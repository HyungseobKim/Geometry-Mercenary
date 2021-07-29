/*!*******************************************************************
\file         UnitManager.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         05/11/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

/*!*******************************************************************
\class        UnitManager
\brief		  Manages units on the field and battles.
********************************************************************/
public class UnitManager : MonoBehaviour
{
    // Singleton pattern
    public static UnitManager instance;
    private UnitManager() { }

    [HideInInspector]
    public List<GameObject> playerUnits; //! Player units on field.
    [HideInInspector]
    public List<GameObject> enemyUnits; //! Enemy units.

    [HideInInspector]
    public bool onBattle = false; //! Is battle going on?
    private StartButtonUI button; //! Battle start button object.

    [HideInInspector]
    public GameObject goalPrefab; //! Prefab of goal unit.
    private Vector3 playerGoalPosition; //! Fixed position where to spawn player goal unit.
    private Vector3 enemyGoalPosition; //! Fixed position where to spawn enemy goal unit.

    private bool lastRound = false; //! Is this battle last stage of the game?

    public GameObject playerHealthProjectilePrefab; //! Prefab of player healt projectile.
    private int index = -1; //! Temporal index being used for spawning.
    private float spawnTimer = 0.0f; //! Timer for spawning player health projectile.
    public float spawnTime; //! Time interval for player health projectile.

    // Need to activate when game starts
    public GameObject grid;
    public GameObject startButton;

    private void Awake()
    {
        // Singlton
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        playerGoalPosition = TileManager.instance.tilemap.CellToWorld(new Vector3Int(0, -5, 0));
        enemyGoalPosition = TileManager.instance.tilemap.CellToWorld(new Vector3Int(0, 5, 0));

        grid.SetActive(false);
        startButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        CleanLists();

        // Spawn player health projectiles.
        if (index >= 0 && index < enemyUnits.Count)
        {
            spawnTimer -= Time.deltaTime;

            if (spawnTimer <= 0.0f)
            {
                GameObject enemy = enemyUnits[index];

                if (enemy.name.Contains("Creature"))
                {
                    // Skip this enemy and move to next enemy.
                    ++index;
                    return;
                }

                Object.Instantiate(playerHealthProjectilePrefab, enemy.transform.position, Quaternion.identity);

                // Spwan finished.
                if (++index >= enemyUnits.Count)
                {
                    // Reset all values for the next time.
                    index = -1;
                    spawnTimer = 0.0f;
                    return;
                }

                spawnTimer = spawnTime;
            }
        }
    }

    /*!
     * \brief When the player clicks start button, button will call this function.
     * 
     * \param buttonUI
     *        Store start button object.
     */
    public void BattleStart(StartButtonUI buttonUI)
    {
        onBattle = true;
        button = buttonUI;

        InputManager.instance.BattleStart();

        CleanLists();
    }

    /*!
     * \brief Helper function to remove all null objects from the lists.
     */
    private void CleanLists()
    {
        playerUnits = playerUnits.Where(unit => unit != null).ToList();
        enemyUnits = enemyUnits.Where(unit => unit != null).ToList();
    }

    /*!
     * \brief Battle ended. Clear field and lists.
     *        Spawn new enemies and player units again.
     */
    public void BattleEnd()
    {
        button.BattleFinish();
        onBattle = false;

        InputManager.instance.BattleEnd();

        // Destroy all units in lists.
        foreach (GameObject unit in playerUnits)
            Destroy(unit);
        foreach (GameObject unit in enemyUnits)
            Destroy(unit);

        // Clear the lists.
        playerUnits.Clear();
        enemyUnits.Clear();

        // Spawn goal units.
        PrepareGoalUnits();

        // Initialize player units and enemy units.
        MemberManager.instance.PlaceUnitsOnPosition();
        lastRound = EnemyManager.instance.NextRound();
    }

    /*!
     * \brief Each unit on field register them through this method.
     *        Add them to the correct list.
     *        
     * \param unit
     *        Recruit.   
     */
    public void Register(GameObject unit)
    {
        if (unit == null)
            return;

        Status status = unit.GetComponent<Status>();
        if (status == null)
            return;

        if (status.enemy)
            enemyUnits.Add(unit);
        else
            playerUnits.Add(unit);
    }

    /*!
     * \brief Each unit on filed report their death when they lost all health.
     *        If the unit is goal unit, finish the battle.
     *        Ohterwise, remove them
     *        
     * \param unit
     *        Died unit.
     */
    public void ReportDeath(GameObject unit)
    {
        if (unit == null)
            return;

        Status status = unit.GetComponent<Status>();
        if (status == null)
            return;

        Goal goal = unit.GetComponent<Goal>();
        if (status.enemy)
        {
            if (goal == null)
                enemyUnits.Remove(unit);
            else // Player win.
            {
                // It was last round, so game ends.
                if (lastRound)
                {
                    SceneManager.LoadScene("WinScene", LoadSceneMode.Single);
                    return;
                }

                // Ready for next battle.
                RewardManager.instance.PlayerWin();
                BattleEnd();
            }
        }
        else // Player unit
        {
            if (goal == null)
                playerUnits.Remove(unit);
            else // Enemy win.
            {
                RewardManager.instance.PlayerDefeated(enemyUnits.Count);
                index = 0;
            }
        }
    }

    /*!
     * \brief It would be called only once at the start of the game.
     *        Prepare first round and activates some other objects.
     */
    public void GameStart()
    {
        grid.SetActive(true);
        startButton.SetActive(true);

        PrepareGoalUnits();
        lastRound = EnemyManager.instance.NextRound();
    }

    /*!
     * \brief Helper function to spawn goal units on their position.
     */
    public void PrepareGoalUnits()
    {
        // Respawn goal units.
        Status playerGoal = Object.Instantiate(goalPrefab, playerGoalPosition, Quaternion.identity).GetComponent<Status>();
        playerGoal.enemy = false;
        playerGoal.maxHealth = 100.0f;

        Status enemyGoal = Object.Instantiate(goalPrefab, enemyGoalPosition, Quaternion.identity).GetComponent<Status>();
        enemyGoal.enemy = true;
        enemyGoal.maxHealth = 100.0f;
    }
}
