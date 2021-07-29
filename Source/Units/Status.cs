/*!*******************************************************************
\file         Status.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         05/09/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*!*******************************************************************
\class        Status
\brief        Contains variables to define unit and methods to manipulate UI.
********************************************************************/
public class Status : MonoBehaviour
{
    [HideInInspector]
    public int indexOnMemberList = -1; //! Index of this unit on the list inside Member Manager.

    public bool enemy; //! If true, this is enemy unit. If false, this is player unit.

    public float maxHealth = 100.0f; //! Maximum health.
    [HideInInspector]
    public float health; //! Current health.
    
    public float power = 20.0f; //! Value of abilities such as attack damage, heal, etc.
    public float speed = 20.0f; //! How much cooldown will be reduced per second.
    public float protection = 0.0f; //! When get attack, actual damage = damage - protection.

    public int attackRange = 1; //! Range of attack.
    public int abilityRange = 1; //! Range of ability.

    public float cooldown = 100.0f; //! Amount of time to wait to act again. Only few units will have different value.
    [HideInInspector]
    public float cooldownRemain; //! Current cooldown.

    public Vector3Int cellPos; //! Index of tile that this unit is on.

    [HideInInspector]
    public List<Item> items = new List<Item>(); //! List of itmes that this unit equips.

    // Vairables for tactics.
    public double movement = 0.0; //! Bonus on movement for tactical option.
    public double aggression = 0.0; //! Bonus on aggression for tactical option.
    public bool useMovement = true; //! If false, this unit has its own movement formular.
    public bool useAggression = true; //! If false, this unit doesn't attack or has its own attack formular.

    public bool stealth = false; //! Is this unit in stealth?

    private BarScaler healthBar; //! UI to the health of this unit.
    private BarScaler cooldownBar; //! UI to the cooldown of this unit.

    // Variables for shield
    private int shield = 0; //! The number of available shield remained.
    private static GameObject shieldPrefab = null; //! Prefab of shield UI.
    private GameObject shieldObject; //! Object of shield UI.

    public int healBan = 0; //! The number of times that it cannot get healed.

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        cooldownRemain = cooldown;

        healthBar = gameObject.transform.Find("HealthBar").GetComponent<BarScaler>();
        cooldownBar = gameObject.transform.Find("CooldownBar").GetComponent<BarScaler>();

        if (shieldPrefab == null)
            shieldPrefab = Resources.Load("Prefabs/Units/Shield", typeof(GameObject)) as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        // Battle ends. Don't do anything other things.
        if (UnitManager.instance.onBattle == false)
            return;

        foreach (Item item in items)
            item.OnItemUpdate(Time.deltaTime, gameObject);
    }

    /*!
     * \brief Change health value and UI gradually.
     * 
     * \param amount
     *        Amount of change. Can be either negative or positive.
     */
    public void ChangeHealth(float amount)
    {
        health = Mathf.Clamp(health + amount, 0.0f, maxHealth);

        if (healthBar)
            healthBar.InterpolateToScale(health / maxHealth, 0.5f);
    }

    /*!
     * \brief Reduce cooldown and change UI gradually.
     * 
     * \param amount
     *        Amount of change. Must be positive.
     */
    public void ChangeCooldown(float amount)
    {
        cooldownRemain = Mathf.Clamp(cooldownRemain + amount, 0.0f, cooldown);
        cooldownBar.InterpolateToScale(cooldownRemain / cooldown, 0.5f);
    }

    /*!
     * \brief Reset cooldown and change UI immediately.
     */
    public void ResetCooldown()
    {
        cooldownRemain = cooldown;
        cooldownBar.InterpolateImmediate(1.0f);
    }

    /*!
     * \brief This unit gets shield.
     */
    public void IncreasesShield()
    {
        // There was no shield.
        if (shield++ == 0) // Add UI.
            shieldObject = Object.Instantiate(shieldPrefab, gameObject.transform);
    }

    /*!
     * \brief Blocks attack and reduce one shield.
     */
    public bool UseShield()
    {
        // No shield
        if (shield <= 0)
            return false;

        // Last shield used.
        if (--shield == 0)
            Destroy(shieldObject);

        // Create text UI.
        BattleUIManager.instance.CreateUsingShieldUI(gameObject);
        return true;
    }
}
