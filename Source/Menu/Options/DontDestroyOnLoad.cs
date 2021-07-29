/*!*******************************************************************
\file         DontDestroyOnLoad.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         07/17/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        DontDestroyOnLoad
\brief		  Standalone component for objects do not want to destory
              on loading scene.
********************************************************************/
public class DontDestroyOnLoad : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
