/*!*******************************************************************
\file         StartingUnitColumn.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/05/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        StartingUnitColumn
\brief		  Represents each column of starting unit UI,
              which contains information of one unit.
********************************************************************/
public class StartingUnitColumn : MonoBehaviour
{
    private SpriteRenderer selection = null; //! Currently selected ability.
    public int index = -1; //! Index of this column in starting unit selector.

    /*!
     * \brief Player clicked new icon on this column.
     */
    public void SelectionChange(GameObject newSelection)
    {
        Reset();

        selection = newSelection.GetComponent<SpriteRenderer>();
        selection.color = new Color(0.2f, 0.2f, 1.0f, 1.0f);
    }

    /*!
     * \brief Change color of selection to original.
     */
    public void Reset()
    {
        if (selection)
        {
            selection.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            selection = null;
        }
    }
}
