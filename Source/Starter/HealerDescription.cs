/*!*******************************************************************
\file         HealerDescription.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/06/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        HealerDescription
\brief		  Description class for healer unit icon.
********************************************************************/
public class HealerDescription : Description
{
    private static GameObject description = null;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();

        if (description == null)
        {
            description = GameObject.Find("HealerDescription");
            description.SetActive(false);
        }
    }

    protected override void OnMouseEnter()
    {
        base.OnMouseEnter();

        description.SetActive(true);
    }

    /*!
     * \brief Tells to starting unit selector that healer assigned to this column.
     */
    protected override void AssignClass()
    {
        StartingUnitSelector.instance.AssignHealer(parent.index);
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
