/*!*******************************************************************
\file         StartButtonUI.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/01/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]

/*!*******************************************************************
\class        StartButtonUI
\brief        Button makes a battle begin.
********************************************************************/
public class StartButtonUI : MonoBehaviour
{
    private UnitManager unitManager; //! Reference to unit manager.
    private Text text; //! Text of the button.

    private bool battleStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        unitManager = UnitManager.instance;
        text = gameObject.transform.Find("Text").GetComponent<Text>();

        text.text = "Start!";
    }

    void OnMouseOver()
    {
        // If game started, do nothing.
        if (battleStarted == true)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            // Start the battle.
            battleStarted = true;
            unitManager.BattleStart(this);

            // Play sound and change text.
            AudioManager.instance.Play("BattleStart");
            text.text = "Fighting";
        }
    }

    /*!
     * \brief Battle ends. Change text.
     */
    public void BattleFinish()
    {
        text.text = "Start!";
        battleStarted = false;
    }
}
