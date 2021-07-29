/*!*******************************************************************
\file         Blacksmith.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         05/30/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        Blacksmith
\brief		  Increases protection when heals.
********************************************************************/
public class Blacksmith : Healer
{
    public SpriteRenderer square;

    protected float protection;

    public override void Initialize()
    {
        base.Initialize();

        square.color = gameObject.GetComponent<SpriteRenderer>().color;

        protection = status.power / 20.0f;
    }

    public override void ApplyHeal(GameObject target)
    {
        if (target == null)
            return;

        // Heals, and then
        base.ApplyHeal(target);

        // increases protection.
        target.GetComponent<Status>().protection += protection;
    }

    public override double GetDangerValue(GameObject scorer)
    {
        if (scorer.GetComponent<Defender>() != null)
            return 0.9;
        else if (scorer.GetComponent<Healer>() != null)
            return 0.4;
        else if (scorer.GetComponent<Supporter>() != null)
            return 0.3;
        else if (scorer.GetComponent<Melee>() != null)
            return 0.9;
        else if (scorer.GetComponent<Forward>() != null)
            return 0.9;

        return 0.9;
    }

    public override double GetAllyValue(GameObject scorer)
    {
        if (scorer.GetComponent<Defender>() != null)
            return 0.65;
        else if (scorer.GetComponent<Healer>() != null)
            return 0.45;
        else if (scorer.GetComponent<Supporter>() != null)
            return 0.75;
        else if (scorer.GetComponent<Melee>() != null)
            return 0.45;
        else if (scorer.GetComponent<Forward>() != null)
            return 0.55;

        return 0.55;
    }

    public override string GetSecondAbilityDescription()
    {
        return "When heals, it increases protection too.\nThe amount is power/20.";
    }
}
