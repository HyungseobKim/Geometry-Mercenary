/*!*******************************************************************
\file         ResurrectionPlaymaker.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/2/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        ResurrectionPlaymaker
\brief		  Resurrect one ally, which died firstly.
********************************************************************/
public class ResurrectionPlaymaker : Playmaker
{
    public SpriteRenderer cross;

    private static bool resurrectPlayer = false; //! Did player unit already use this abilitys?
    private static bool resurrectEnemy = false; //! Did enemy unit already use this ability?

    public override void Initialize()
    {
        base.Initialize();

        cross.color = gameObject.GetComponent<SpriteRenderer>().color;

        if (status.enemy)
            resurrectEnemy = true;
        else
            resurrectPlayer = true;
    }

    public static bool Resurrect(GameObject target)
    {
        // This is summoned creature, so do not waste resurrection.
        if (target.GetComponent<Creature>())
            return false;

        Status targetStatus = target.GetComponent<Status>();

        if (targetStatus.enemy)
        {
            if (resurrectEnemy == false)
                return false;
            else
                resurrectEnemy = false;
        }
        else
        {
            if (resurrectPlayer == false)
                return false;
            else
                resurrectPlayer = false;
        }

        target.GetComponent<UnitBase>().GetHeal(targetStatus.maxHealth);
        BattleUIManager.instance.CreateResurrectionUI(target);

        return true;
    }

    public override string GetThirdAbilityDescription()
    {
        return "Resurrect one ally, which died firstly.";
    }
}
