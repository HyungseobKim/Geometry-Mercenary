/*!*******************************************************************
\file         BattleUIManager.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         05/27/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;
using UnityEngine.UI;

/*!*******************************************************************
\class        BattleUIManager
\brief        Manages temporal UIs that will be created during battle.
********************************************************************/
public class BattleUIManager : MonoBehaviour
{
    // Singleton pattern.
    public static BattleUIManager instance = null;
    private BattleUIManager() { }

    public GameObject Canvas; //! Canvas for texts.
    public GameObject textBattleUIPrefab; //! UI prfeab.

    private Color poisonColor = new Color(180.0f/255.0f, 0.0f, 230.0f/255.0f, 1.0f); //! Color of health bar when poisoned.
    private Color originalHealthColor = new Color(0.0f, 1.0f, 0.0f, 1.0f); //! Default health bar color.
    
    private Color stunColor = new Color(0.0f, 222.0f / 255.0f, 245.0f / 255.0f, 1.0f); //! Color of cooldown bar when stunned.
    private Color originalStunColor = new Color(245.0f / 255.0f, 240.0f / 255.0f, 0.0f, 1.0f); //! Default cooldown bar color.

    private float randomRange = 0.3f; //! Range of random position modifier.

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    /*!
     * \brief Create common damaging UI.
     * 
     * \param victim
     *        Who get damaged.
     *        
     * \param damage
     *        The amount of damage.
     *        
     * \param critical
     *        Is this critical hit?
     */
    public void CreateDamageUI(GameObject victim, float damage, bool critical = false)
    {
        if (victim == null)
            return;

        Vector3 textPos = victim.transform.position;

        // Move position slightly.
        textPos.x += Random.Range(-randomRange, randomRange);

        // Create text UI.
        Text damageText = Object.Instantiate(textBattleUIPrefab, textPos, Quaternion.identity, Canvas.transform).GetComponent<Text>();
        
        // Set text.
        damageText.text = Mathf.Floor(damage).ToString();
        // Set damage color (Red).
        damageText.color = new Color(1.0f, 0.2f, 0.2f, 1.0f);

        // If it is critical hit.
        if (critical)
        {
            // Make it more noticable.
            damageText.fontSize = (int)(damageText.fontSize * 1.2f);
            damageText.text += "!";
        }
    }

    /*!
     * \brief Create UI when a unit evaded attack.
     * 
     * \param unit
     *        Who succeeded to evade.
     */
    public void CreateMissUI(GameObject unit)
    {
        if (unit == null)
            return;

        Vector3 textPos = unit.transform.position;

        // Move position slightly.
        textPos.x += Random.Range(-randomRange, randomRange);

        // Create text UI.
        Text damageText = Object.Instantiate(textBattleUIPrefab, textPos, Quaternion.identity, Canvas.transform).GetComponent<Text>();
        // Set text and color.
        damageText.text = "Miss!";
        damageText.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }

    /*!
     * \brief Create UI when a unit encounter ambush.
     * 
     * \param unit
     *        Who was ambushed.
     */
    public void CreateAmbushUI(GameObject unit)
    {
        if (unit == null)
            return;

        Vector3 textPos = unit.transform.position;

        // Move position slightly.
        textPos.x += Random.Range(-randomRange, randomRange);

        // Create text UI.
        Text damageText = Object.Instantiate(textBattleUIPrefab, textPos, Quaternion.identity, Canvas.transform).GetComponent<Text>();
        // Set text and color.
        damageText.text = "Ambush!";
        damageText.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }

    /*!
     * \brief Create common healing UI.
     * 
     * \param patient
     *        Who get healed.
     *        
     * \param amount
     *        The amount of healing.
     */
    public void CreateHealUI(GameObject patient, float amount)
    {
        if (patient == null)
            return;

        Vector3 textPos = patient.transform.position;

        // Move position slightly.
        textPos.x += Random.Range(-randomRange, randomRange);

        // Create text UI.
        Text healText = Object.Instantiate(textBattleUIPrefab, textPos, Quaternion.identity, Canvas.transform).GetComponent<Text>();
        // Set text and color.
        healText.text = Mathf.Floor(amount).ToString();
        healText.color = new Color(0.2f, 1.0f, 0.2f, 1.0f);
    }

    /*!
     * \brief Set poison color to health bar.
     * 
     * \param victim
     *        Who get poisoned.
     */
    public void PoisonStart(GameObject victim)
    {
        if (victim == null)
            return;

        victim.transform.Find("HealthBar").GetComponent<SpriteRenderer>().color = poisonColor;
    }

    /*!
     * \brief Set health bar to original color.
     * 
     * \param victim
     *        Who was poisoned.
     */
    public void PoisonEnd(GameObject victim)
    {
        if (victim == null)
            return;

        victim.transform.Find("HealthBar").GetComponent<SpriteRenderer>().color = originalHealthColor;
    }

    /*!
     * \brief Set stun color to cooldown bar.
     * 
     * \param victim
     *        Who get stunned.
     */
    public void StunStart(GameObject victim)
    {
        if (victim == null)
            return;

        victim.transform.Find("CooldownBar").GetComponent<SpriteRenderer>().color = stunColor;
    }

    /*!
     * \brief Set cooldown bar to original color.
     * 
     * \param victim
     *        Who was stunned.
     */
    public void StunEnd(GameObject victim)
    {
        if (victim == null)
            return;

        victim.transform.Find("CooldownBar").GetComponent<SpriteRenderer>().color = originalStunColor;
    }

    /*!
     * \brief Create UI when shield is used.
     * 
     * \param beneficiary
     *        Who used shield.
     */
    public void CreateUsingShieldUI(GameObject beneficiary)
    {
        if (beneficiary == null)
            return;

        Vector3 textPos = beneficiary.transform.position;

        // Move position slightly.
        textPos.x += Random.Range(-randomRange, randomRange);

        // Create text UI.
        Text shieldText = Object.Instantiate(textBattleUIPrefab, textPos, Quaternion.identity, Canvas.transform).GetComponent<Text>();
        // Set text and color.
        shieldText.text = "Blocked!";
        shieldText.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }

    /*!
     * \brief Create UI when a unit is resurrected.
     * 
     * \param beneficiary
     *        Who resurrected.
     */
    public void CreateResurrectionUI(GameObject beneficiary)
    {
        if (beneficiary == null)
            return;

        Vector3 textPos = beneficiary.transform.position;
        // Move position slightly.
        textPos.x += Random.Range(-randomRange, randomRange);

        // Create text UI.
        Text shieldText = Object.Instantiate(textBattleUIPrefab, textPos, Quaternion.identity, Canvas.transform).GetComponent<Text>();
        // Set text and color.
        shieldText.text = "Resurrected!";
        shieldText.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }

    /*!
     * \brief Create UI when a unit is executed.
     * 
     * \param victim
     *        Who executed.
     */
    public void CreateExecuteUI(GameObject victim)
    {
        if (victim == null)
            return;

        Vector3 textPos = victim.transform.position;
        // Move position slightly.
        textPos.x += Random.Range(-randomRange, randomRange);

        // Create text UI.
        Text shieldText = Object.Instantiate(textBattleUIPrefab, textPos, Quaternion.identity, Canvas.transform).GetComponent<Text>();
        // Set text and color.
        shieldText.text = "Execute!";
        shieldText.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }

    /*!
     * \brief Create UI when a unit is taunted.
     * 
     * \param victim
     *        Who taunted.
     */
    public void CreateTauntUI(GameObject victim)
    {
        if (victim == null)
            return;

        Vector3 textPos = victim.transform.position;
        // Move position slightly.
        textPos.x += Random.Range(-randomRange, randomRange);

        // Create text UI.
        Text shieldText = Object.Instantiate(textBattleUIPrefab, textPos, Quaternion.identity, Canvas.transform).GetComponent<Text>();
        // Set text and color.
        shieldText.text = "Taunted!";
        shieldText.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }
}
