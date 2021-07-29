/*!*******************************************************************
\file         PermanentAssassin.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/28/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        PermanentAssassin
\brief		  Gain permanent stealth. Continuously lose health.
********************************************************************/
public class PermanentAssassin : Assassin
{
    public SpriteRenderer square;

    public override void Initialize()
    {
        base.Initialize();

        square.color = gameObject.GetComponent<SpriteRenderer>().color;

        Stealth();
    }

    // Update is called once per frame
    void Update()
    {
        // Keep losing health.
        if (UnitManager.instance.onBattle)
        {
            status.ChangeHealth(-5.0f * Time.deltaTime * GameSpeedController.instance.speed);
            IsDied();
        }
    }

    protected override void Stealth()
    {
        base.Stealth();

        square.color = stealthColor;
    }

    public override void Destealth()
    {
        Stealth();
    }

    public override string GetThirdAbilityDescription()
    {
        return "Gain permanent stealth. Continuously lose health 5 per second.";
    }
}
