/*!*******************************************************************
\file         ShieldTeleporter.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/28/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        ShieldTeleporter
\brief		  Get shield when move.
********************************************************************/
public class ShieldTeleporter : Teleporter
{
    public SpriteRenderer square;

    public override void Initialize()
    {
        base.Initialize();

        square.color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    public override int MoveTo(Vector3Int cellPos)
    {
        int result = base.MoveTo(cellPos);

        if (result != -1)
            status.IncreasesShield();

        return result;
    }

    public override string GetThirdAbilityDescription()
    {
        return "Gain one shield on each turn. Shield will block one next attack.";
    }
}
