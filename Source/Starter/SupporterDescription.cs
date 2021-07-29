/*!*******************************************************************
\file         SupporterDescription.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/06/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        SupporterDescription
\brief		  Description class for supporter unit icon.
********************************************************************/
public class SupporterDescription : Description
{
    private static GameObject description = null; //! Actual description object.

    // Start is called before the first frame update
    void Start()
    {
        Initialize();

        if (description == null)
        {
            description = GameObject.Find("SupporterDescription");
            description.SetActive(false);
        }
    }

    protected override void OnMouseEnter()
    {
        base.OnMouseEnter();

        description.SetActive(true);
    }

    /*!
     * \brief Tells to starting unit selector that supporter assigned to this column.
     */
    protected override void AssignClass()
    {
        StartingUnitSelector.instance.AssignSupporter(parent.index);
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
