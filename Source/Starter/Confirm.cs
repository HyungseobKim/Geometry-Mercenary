/*!*******************************************************************
\file         Confirm.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/05/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

/*!*******************************************************************
\class        Confirm
\brief		  When player clicks this button, calls confirm method on
              StartingUnitSelector class.
********************************************************************/
public class Confirm : MonoBehaviour
{
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
            StartingUnitSelector.instance.Confirm();
    }
}
