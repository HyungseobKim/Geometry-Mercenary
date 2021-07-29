/*!*******************************************************************
\file         UnitControl.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         05/09/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

[RequireComponent(typeof(Status))]

/*!*******************************************************************
\class        UnitControl
\brief        Keeps updating cooldown of this unit and make it have turn
              when it is ready.
********************************************************************/
public class UnitControl : MonoBehaviour
{
    private Status status; //! Status of this unit.
    public UnitBase unitBase; //! Class that has actual ability and behavior.

    public static float turnDelay = 0.1f; //! Delay between a turn and a turn.
    private float turnDelayTimer = 0.0f; //! Elapsed time from last turn.

    private Vector3 destination; //! Destination of this unit, if this is moving.
    private Vector3 posDifference; //! Destination - current position.

    [HideInInspector]
    public float movingTimer = 0.0f; //! Elapsed time from start of the moving.
    public static float movingTime = 0.5f; //! Needed time to move.
    private bool movedInThisFrame = false; //! Is this unit moved in this frame?

    private bool ambushed = false; //! This unit just encountered ambush.
    private Vector3 originalPos; //! The position where it came from.
    private GameObject ambushingEnemy; //! The enemy ambushed.

    [HideInInspector]
    public int extraTurn = 0; //! The number of available extra turn.
    private float stunTimer = 0.0f; //! Remained stunning time.
    [HideInInspector]
    public GameObject tauntingTarget = null; //! The unit who taunted this unit.

    // Variables for poison
    public static float poisonTimeInterval = 0.5f; //! Time among the poison damage.
    [HideInInspector]
    public float poisonDamage = 0.0f; //! The poison damage that this unit will take for the next time.
    [HideInInspector]
    public float poisonTimeRemain = 0.0f; //! Remained poison time.
    [HideInInspector]
    public float poisonTimer = 0.0f; //! Elapsed time from last poison damage.

    private float totalHeal; //! Remained heal amount.
    private float healTimeRemain; //! Remained heal time.

    // Start is called before the first frame update
    void Start()
    {
        status = gameObject.GetComponent<Status>();
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime * GameSpeedController.instance.speed;
        movedInThisFrame = false;

        // Update moving.
        if (movingTimer > 0.0f)
        {
            movingTimer -= dt;
            movedInThisFrame = true;

            // Moving done, so 
            if (movingTimer <= 0.0f)
            {
                // There was an ambush, so get back to original location.
                if (ambushed == true)
                {
                    ambushingEnemy.GetComponent<UnitBase>().ApplyDamage(gameObject);
                    gameObject.transform.position = originalPos;
                    ambushed = false;
                }
                else // Snap the sprite to the center of the cell.
                    gameObject.transform.position = destination;
            }
            else // Update moving.
                gameObject.transform.position += (dt * posDifference);
        }

        // Battle ends. Don't do anything other things.
        if (UnitManager.instance.onBattle == false)
            return;

        // Update poison damage. It must be updated under any circumstance.
        if (poisonTimeRemain > 0.0f)
        {
            poisonTimer -= dt;

            if (poisonTimer <= 0.0f)
            {
                // Reset timer.
                poisonTimer = poisonTimeInterval;
                poisonTimeRemain -= poisonTimeInterval;
                PoisonDamage();
            }

            if (poisonTimeRemain <= 0.0f)
                PoisonEnd();
        }

        // Restoration from some of abilities.
        if (healTimeRemain > 0.0f)
        {
            float heal = totalHeal * (dt / healTimeRemain);
            status.ChangeHealth(heal);

            totalHeal -= heal;
            healTimeRemain -= dt;
        }

        // Check stun after updating moving, because it should not prevent to move which started before stunned.
        if (stunTimer > 0.0f)
        {
            stunTimer -= dt;
            
            if (stunTimer <= 0.0f)
                BattleUIManager.instance.StunEnd(gameObject);

            return; // Can't do anything in this turn.
        }

        // Update Cooldown & Timer
        status.ChangeCooldown(-status.speed * dt);
        turnDelayTimer -= dt;

        // Don't do anything while moving.
        if (movedInThisFrame)
            return;

        // Wait until delay ends.
        if (turnDelayTimer >= 0.0f)
            return;

        // Take regular turn first.
        if (status.cooldownRemain <= 0.0f)
        {
            status.ResetCooldown();
            turnDelayTimer = turnDelay;

            if (TauntingCheckAndBehave())
                return;

            unitBase.TakeTurn();
            return;
        }

        // If this unit can have extra turn, do.
        if (extraTurn > 0)
        {
            extraTurn -= 1;
            turnDelayTimer = turnDelay;

            if (TauntingCheckAndBehave())
                return;

            unitBase.TakeTurn();
            return;
        }
    }

    /*!
     * \brief When the unit can have a turn, before it allows to move, check whether it is taunted or not.
     *        If it is taunted, try to attack the enemy.
     * 
     * \return bool
     *         True if it is taunted.
     *         False otherwise.
     */
    private bool TauntingCheckAndBehave()
    {
        if (tauntingTarget == null)
            return false;

        Cell currentCell = TileManager.instance.cells[status.cellPos];
        Cell enemyCell = TileManager.instance.cells[tauntingTarget.GetComponent<Status>().cellPos];

        int distance = Pathfinder.instance.FindPath(currentCell, enemyCell, status.attackRange, status.enemy);
        if (distance >= 0) // Try to get close to target.
            unitBase.MoveTo(Pathfinder.instance.GetNextPos(currentCell, enemyCell));

        if (unitBase.CanAttack(tauntingTarget))
            unitBase.Attack(tauntingTarget);

        tauntingTarget = null;
        return true;
    }

    /*!
     * \brief This unit decided to move. Compute related variables.
     * 
     * \param position
     *        Position to move.
     */
    public void MoveTo(Vector3 position)
    {
        destination = position;
        posDifference = (position - gameObject.transform.position) / movingTime;
        movingTimer = movingTime;
        AudioManager.instance.Play("Footstep");
    }

    /*!
     * \brief Helper function to compute total remained poison damage.
     * 
     * \return float
     *         Remained poison damage.
     */
    public float GetTotalDamage()
    {
        return poisonDamage * (poisonTimeRemain / poisonTimeInterval);
    }

    /*!
     * \brief Assign stunning time.
     * 
     * \param time
     *        Time to be stunned.
     */
    public void ApplyStun(float time)
    {
        if (stunTimer <= 0.0f)
            BattleUIManager.instance.StunStart(gameObject);

        stunTimer += time;
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
    public void ApplyPoison(float totalDamage, float time)
    {
        totalDamage += GetTotalDamage();
        poisonTimeRemain += time;
        poisonDamage = (totalDamage / (poisonTimeRemain / poisonTimeInterval));

        BattleUIManager.instance.PoisonStart(gameObject);
    }

    /*!
     * \brief Helper function to update poison damage.
     */
    private void PoisonDamage()
    {
        // Reduce health.
        status.ChangeHealth(-poisonDamage);

        // Generate UI.
        BattleUIManager.instance.CreateDamageUI(gameObject, poisonDamage);

        unitBase.IsDied();
    }

    /*!
     * \brief Poison ends, so reset all related values.
     */
    public void PoisonEnd()
    {
        // Remove UI.
        BattleUIManager.instance.PoisonEnd(gameObject);

        unitBase.poisonHeal = false;
        unitBase.poisonMove = false;
        unitBase.poisonAttack = false;
    }

    /*!
     * \brief Helper function to update heal over time.
     * 
     * \param amount
     *        Amount of heal this time.
     *        
     * \param time
     *        Elapsed time.
     */
    public void ApplyHoT(float amount, float time)
    {
        totalHeal += amount;
        healTimeRemain += time;
    }

    /*!
     * \brief This unit tried to move, but there was an ambushed enemy.
     * 
     * \param position
     *        Position to move.
     *        
     * \param enemy
     *        Encountered enemy.
     */
    public void MoveAndMeetAmbush(Vector3 position, GameObject enemy)
    {
        originalPos = gameObject.transform.position;
        ambushed = true;
        ambushingEnemy = enemy;

        MoveTo(position);
    }
}
