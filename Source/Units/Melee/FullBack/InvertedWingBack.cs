/*!*******************************************************************
\file         InvertedWingBack.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/28/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        InvertedWingBack
\brief		  Possibility of evasion increases 10% per adjacent enemy.
********************************************************************/
public class InvertedWingBack : FullBack
{
    public SpriteRenderer circle;

    public override void Initialize()
    {
        base.Initialize();

        circle.color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    protected override bool Evade()
    {
        return (Random.Range(NumberofAdjacentEnemies(status.cellPos), 10) >= 5);
    }

    public override string GetThirdAbilityDescription()
    {
        return "Possibility of evasion increases 10% per adjacent enemy.";
    }
}
