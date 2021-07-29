/*!*******************************************************************
\file         Mezzala.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/27/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        Mezzala
\brief		  Choose two random enemies and swap their position.
********************************************************************/
public class Mezzala : CentreMidfielder
{
    public SpriteRenderer pentagon;

    public override void Initialize()
    {
        base.Initialize();

        pentagon.color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    /*!
     * \brief It must not change position of goal unit.
     */
    protected override void UseAbility(GameObject target)
    {
        base.UseAbility(target);

        bool targetSet = false;
        GameObject secondTarget = null;

        int count = EnemyList.Count;
        int start = Random.Range(0, count);
        
        for (int i = start; i < start + count; ++i)
        {
            GameObject enemy = EnemyList[i % count];

            if (enemy == null)
                continue;

            // Cannot move goal unit.
            if (enemy.GetComponent<Goal>())
                continue;

            // It is moving, so don't disturb.
            if (enemy.GetComponent<UnitControl>().movingTimer > 0.0f)
                continue;

            if (targetSet == false)
            {
                target = enemy;
                targetSet = true;
            }
            else
            {
                secondTarget = enemy;
                break;
            }
        }

        // There were no proper targets.
        if (target == secondTarget || target == null || secondTarget == null)
            return;

        // Swap position.
        Vector3Int targetPos = target.GetComponent<Status>().cellPos;
        target.GetComponent<UnitBase>().ForceToMoveTo(secondTarget.GetComponent<Status>().cellPos);
        secondTarget.GetComponent<UnitBase>().ForceToMoveTo(targetPos);
    }

    public override string GetThirdAbilityDescription()
    {
        return "Choose two random enemies and swap their position.";
    }
}
