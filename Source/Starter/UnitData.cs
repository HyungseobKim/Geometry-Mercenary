/*!*******************************************************************
\file         StartingUnitSelector.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/04/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        UnitData
\brief		  Contains minimum information about each game unit,
              which is needed to spawn actual object.
********************************************************************/
public class UnitData
{
    public float power; //! Power of the unit.
    public float speed; //! Speed of the unit.
    public float health; //! Health of the unit.
    public int protection; //! Protection of the unit.

    /*!*******************************************************************
    \enum        Type
    \brief		 Types of abilities. There are total 5 abilities.
    ********************************************************************/
    public enum Type
    {
        Defender,
        Healer,
        Supporter,
        Melee,
        Forward,
        Default
    }

    public Type baseType = Type.Default; //! Base type of ability.
    public Type secondType = Type.Default; //! First upgrade ability.
    public Type thirdType = Type.Default; //! Second upgrade ability.

    public int index; //! Index of this unit in UnitManager.

    public GameObject gameObject; //! Actual object of this unit.
    public double movement; //! Bonus on movement for tactical option.
    public double aggression; //! Bonus on aggression for tactical option.

    public Item item1 = null; //! First item that this object equipped.
    public Item item2 = null; //! Second item that this object equipped.

    public Vector3 position;  //! Position of unit before battle starts.
}
