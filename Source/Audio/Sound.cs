/*!*******************************************************************
\file         Sound.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         07/15/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;
using UnityEngine.Audio;

/*!*******************************************************************
\class        Sound
\brief		  Helper class to store information related with audio.
********************************************************************/
[System.Serializable]
public class Sound
{
    public AudioClip clip; //! Original sound source file.

    [HideInInspector]
    public string name; //! Name of audio for searching.

    [HideInInspector]
    public AudioSource source; //! AuidoSource component that allows to play sound by Unity.
}
