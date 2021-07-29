/*!*******************************************************************
\file         StatusDescription.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/29/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

/*!*******************************************************************
\class        StatusDescription
\brief		  Turn on the description for each status on starting unit selection UI.
********************************************************************/
public class StatusDescription : MonoBehaviour
{
    public GameObject description;
    
    // Start is called before the first frame update
    void Start()
    {
        description.SetActive(false);
    }

    void OnMouseEnter()
    {
        description.SetActive(true);
    }

    void OnMouseExit()
    {
        description.SetActive(false);
    }
}
