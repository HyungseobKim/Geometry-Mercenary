/*!*******************************************************************
\file         Projectile.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         05/27/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/*!*******************************************************************
\class        Projectile
\brief		  Base class for all kinds of projectiles using in a battle.
********************************************************************/
public class Projectile : MonoBehaviour
{
    public float speed = 1.0f; //! Speed of projectile.
    public bool enemy; //! Is this projectile by enemy?

    protected Vector3 destination; //! Destination position of this projectile.
    protected GameObject target; //! Target object of this projectile.
    protected GameObject owner; //! Who shot this projectile.

    protected static Tilemap tilemap = null; //! Reference to tile map.

    private List<Vector3Int> passedTiles = new List<Vector3Int>(); //! Keep tracking of all tiles it passed for few special abilities.

    /*!
     * \brief When projectile launched, save essential information.
     */
    public virtual void Initialize(GameObject targetObject, GameObject ownerObject)
    {
        enemy = ownerObject.GetComponent<Status>().enemy;

        target = targetObject;
        owner = ownerObject;

        ChangeColor();
    }

    /*!
     * \brief When projectile launched, save essential information.
     */
    public void Initialize(Vector3 newDestination, GameObject ownerObject)
    {
        enemy = ownerObject.GetComponent<Status>().enemy;

        destination = newDestination;
        owner = ownerObject;

        ChangeColor();
    }

    /*!
     * \brief Helper function to change color same with owner.
     */
    private void ChangeColor()
    {
        gameObject.GetComponent<SpriteRenderer>().color = owner.GetComponent<SpriteRenderer>().color;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (tilemap == null)
            tilemap = TileManager.instance.tilemap;
    }

    // Update is called once per frame
    void Update()
    {
        if (UpdateStatus())
            return;

        if (InteractionCheck())
            return;

        Action();

        if (ArrivedToDestination())
            return;

        MoveToDestination();
    }

    /*!
     * \brief Update destination by target.
     */
    protected bool UpdateStatus()
    {
        if (owner == null)
        {
            Destroy(gameObject);
            return true;
        }

        // If target is assigned as object rather than position,
        if (target) // update destination every time to follow that object.
            destination = target.transform.position;

        return false;
    }

    /*!
     * \brief Interacts with ally or enemy depending on ability.
     */
    protected virtual bool InteractionCheck()
    {
        Vector3Int currentPos = tilemap.WorldToCell(gameObject.transform.position);
        
        // Don't check same tile twice.
        if (passedTiles.Contains(currentPos))
            return false;
        
        Cell cell;

        if (TileManager.instance.cells.TryGetValue(currentPos, out cell) == false)
            return false;

        if (cell.owner)
            return cell.owner.GetComponent<UnitBase>().InteractWithProjectile(gameObject);

        return false;
    }

    /*!
     * \brief Do anything should be done for every frame.
     */
    protected virtual void Action()
    {
        // do nothing
    }

    /*!
     * \brief Do anything should be done when it arrived to destination.
     */
    protected virtual bool ArrivedToDestination()
    {
        // Not arrived yet.
        if (tilemap.WorldToCell(gameObject.transform.position) != tilemap.WorldToCell(destination))
            return false;

        if (target)
            owner.GetComponent<UnitBase>().ApplyDamage(target);

        Destroy(gameObject);
        return true;
    }

    /*!
     * \brief Update position.
     */
    protected void MoveToDestination()
    {
        Vector3 currentPos = gameObject.transform.position;
        Vector3 direction = Vector3.Normalize(destination - currentPos);

        // Rotate to moving direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        gameObject.transform.rotation = Quaternion.AngleAxis(angle - 90.0f, Vector3.forward);

        // Move to target
        currentPos += (direction * speed * Time.deltaTime * GameSpeedController.instance.speed);
        currentPos.z = 0.0f;
        gameObject.transform.position = currentPos;
    }
}
