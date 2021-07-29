/*!*******************************************************************
\file         RewardReroll.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         07/08/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

/*!*******************************************************************
\class        RewardReroll
\brief		  When player clicks this button, reroll reward choices.
********************************************************************/
public class RewardReroll : MonoBehaviour
{
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RewardManager.instance.Reroll();
            gameObject.SetActive(false);
        }
    }
}
