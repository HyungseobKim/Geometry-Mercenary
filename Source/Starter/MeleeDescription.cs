/*!*******************************************************************
\file         MeleeDescription.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/06/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        MeleeDescription
\brief		  Description class for melee unit icon.
********************************************************************/
public class MeleeDescription : Description
{
    private static GameObject description = null;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();

        if (description == null)
        {
            description = GameObject.Find("MeleeDescription");
            description.SetActive(false);
        }
    }

    protected override void OnMouseEnter()
    {
        base.OnMouseEnter();

        description.SetActive(true);
    }

    /*!
     * \brief Tells to starting unit selector that melee assigned to this column.
     */
    protected override void AssignClass()
    {
        StartingUnitSelector.instance.AssignMelee(parent.index);
    }

    protected override void OnMouseExit()
    {
        base.OnMouseExit();

        description.SetActive(false);
    }

    void OnDestroy()
    {
        if (description)
            Destroy(description);
    }
}
