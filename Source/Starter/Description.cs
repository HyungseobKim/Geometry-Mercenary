/*!*******************************************************************
\file         Description.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/06/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

/*!*******************************************************************
\class        Description
\brief		  Base class for starting unit selector class icons.
              Handles UI and call click method.
********************************************************************/
public abstract class Description : MonoBehaviour
{
    // Mouse position UI.
    private static GameObject verticalBar = null;
    private static GameObject horizontalBar = null;

    protected StartingUnitColumn parent; //! Each column inside starting unit UI, which is parent object of this.

    /*!
     * \brief Find UI objects and the parent object.
     */
    protected void Initialize()
    {
        if (horizontalBar == null)
        {
            horizontalBar = GameObject.Find("HorizontalBar");
            horizontalBar.SetActive(false);
        }

        if (verticalBar == null)
        {
            verticalBar = GameObject.Find("VerticalBar");
            verticalBar.SetActive(false);
        }

        parent = gameObject.transform.parent.gameObject.GetComponent<StartingUnitColumn>();
    }

    protected virtual void OnMouseEnter()
    {
        // Move vertical bar
        verticalBar.SetActive(true);

        var position = verticalBar.transform.position;
        position.x = gameObject.transform.position.x;

        verticalBar.transform.position = position;

        // Move horizontal bar
        horizontalBar.SetActive(true);

        position = horizontalBar.transform.position;
        position.y = gameObject.transform.position.y;

        horizontalBar.transform.position = position;
    }

    protected virtual void OnMouseExit()
    {
        verticalBar.SetActive(false);
        horizontalBar.SetActive(false);
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            parent.SelectionChange(gameObject);
            AssignClass();
        }
    }

    protected abstract void AssignClass();
}
