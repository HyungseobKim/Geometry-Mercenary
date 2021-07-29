/*!*******************************************************************
\file         WideTargetMan.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/23/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using System;
using UnityEngine;

/*!*******************************************************************
\class        WideTargetMan
\brief		  Critical hit ignores all effects.
********************************************************************/
public class WideTargetMan : WingForward
{
    public SpriteRenderer square;

    public override void Initialize()
    {
        base.Initialize();

        square.color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    public override bool ApplyDamage(GameObject target)
    {
        if (target == null)
            return false;

        float damage = status.power;
        bool critical = false;

        if (UnityEngine.Random.Range(0, 2) == 1)
        {
            damage *= 4.0f;
            critical = true;
        }

        //if (critical)
        //{
        //    var funcPtr = typeof(UnitBase).GetMethod("TakeDamage").MethodHandle.GetFunctionPointer();
        //    var takeDamage = (Func<float, bool, bool>)Activator.CreateInstance(typeof(Func<float, bool, bool>), target.GetComponent<UnitBase>(), funcPtr);
        //    takeDamage(damage, critical);
        //}

        if (critical)
        {
            target.GetComponent<Status>().ChangeHealth(-damage);
            BattleUIManager.instance.CreateDamageUI(target, damage, critical);
            return target.GetComponent<UnitBase>().IsDied();
        }

        return target.GetComponent<UnitBase>().TakeDamage(damage, gameObject, critical);
    }

    public override string GetThirdAbilityDescription()
    {
        return "Critical hit ignores all effects.";
    }
}
