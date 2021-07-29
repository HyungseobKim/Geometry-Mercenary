/*!*******************************************************************
\file         Melee.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         05/12/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using System.Linq;
using UnityEngine;

/*!*******************************************************************
\class        Melee
\brief		  Half of the cooldown to take a turn.
********************************************************************/
public class Melee : UnitBase
{
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    public override void Initialize()
    {
        base.Initialize();

        status.cooldown /= 2.0f;
        status.cooldownRemain = status.cooldown;
    }

    public override double GetDangerValue(GameObject scorer)
    {
        if (scorer.GetComponent<Defender>() != null)
            return 0.8;
        else if (scorer.GetComponent<Healer>() != null)
            return 0.35;
        else if (scorer.GetComponent<Supporter>() != null)
            return 0.25;
        else if (scorer.GetComponent<Melee>() != null)
            return 0.7;
        else if (scorer.GetComponent<Forward>() != null)
            return 0.8;

        return 0.8;
    }

    public override double GetAllyValue(GameObject scorer)
    {
        if (scorer.GetComponent<Defender>() != null)
            return 0.2;
        else if (scorer.GetComponent<Healer>() != null)
            return 0.9;
        else if (scorer.GetComponent<Supporter>() != null)
            return 0.5;
        else if (scorer.GetComponent<Melee>() != null)
            return 0.2;
        else if (scorer.GetComponent<Forward>() != null)
            return 0.2;

        return 0.2;
    }

    public override string GetBaseAbilityDescription()
    {
        return "Half of the cooldown to take a turn.";
    }
}
