/*!*******************************************************************
\file         Mathematics.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         05/11/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using System;
using UnityEngine;

/*!*******************************************************************
\class        Mathematics
\brief		  Some useful mathematic functions.
********************************************************************/
public static class Mathematics
{
    /*!
     * \brief Clamping function for double.
     * 
     * \param value
     *        Value to clamp.
     *        
     * \param min
     *        Desire minimum value.
     *        
     * \param max
     *        Desire maximum value.
     *        
     * \return double
     *         Clamped value.
     */
    public static double Clamp(double value, double min, double max)
    {
        return Math.Min(max, Math.Max(value, min));
    }

    /*!
     * \brief Increases x depending on y.
     *        One of the way to handle values between 0 and 1 from fuzzy system.
     * 
     * \param x, y
     *        Values to combine. It should between 0 and 1.
     *        
     * \return double
     *         It is greater than both x and y, but it never goes greater than 1.
     */
    public static double GoguensTconorm(double x, double y)
    {
        return x + y - x * y;
    }
}
