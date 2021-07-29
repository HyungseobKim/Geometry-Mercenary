/*!*******************************************************************
\file         PlaySoundOnAwake.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         07/17/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        PlaySoundOnAwake
\brief		  Play the sound when this object has awakened.
********************************************************************/
public class PlaySoundOnAwake : MonoBehaviour
{
    public string soundName; //! Name of sound to play.

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.Play(soundName);
    }
}
