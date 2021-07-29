/*!*******************************************************************
\file         DealingPriest.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/23/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        DealingPriest
\brief		  Healing projectile gives damage to all enemies it encounters.
********************************************************************/
public class DealingPriest : Priest
{
    public SpriteRenderer pentagon;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    public override void Initialize()
    {
        base.Initialize();

        pentagon.color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    public override string GetThirdAbilityDescription()
    {
        return "Healing projectile gives damage to all enemies it encounters.";
    }
}
