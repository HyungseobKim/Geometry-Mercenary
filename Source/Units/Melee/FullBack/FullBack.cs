/*!*******************************************************************
\file         FullBack.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/01/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        Melee
\brief		  Eavade attacks with 50% chance.
********************************************************************/
public class FullBack : Melee
{
    public SpriteRenderer square;

    protected bool evade;

    public override void Initialize()
    {
        base.Initialize();

        square.color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    public override bool TakeDamage(float amount, GameObject enemy, bool critical = false)
    {
        evade = Evade();

        if (evade)
        {
            BattleUIManager.instance.CreateMissUI(gameObject);
            return false;
        }

        return base.TakeDamage(amount, gameObject, critical);
    }

    public override void ApplyPoison(float totalDamage, float time)
    {
        evade = Evade();

        if (evade)
        {
            BattleUIManager.instance.CreateMissUI(gameObject);
            return;
        }

        base.ApplyPoison(totalDamage, time);
    }

    protected virtual bool Evade()
    {
        return (Random.Range(0, 2) == 1);
    }

    public override string GetSecondAbilityDescription()
    {
        return "Can evade attacks with a 50% chance.";
    }
}
