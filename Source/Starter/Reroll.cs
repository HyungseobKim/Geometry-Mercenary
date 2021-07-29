/*!*******************************************************************
\file         Reroll.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/04/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

/*!*******************************************************************
\class        Reroll
\brief		  Reroll button for starting UI.
********************************************************************/
public class Reroll : MonoBehaviour
{
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
            StartingUnitSelector.instance.Reroll();
    }
}
