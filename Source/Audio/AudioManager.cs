/*!*******************************************************************
\file         AudioManager.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         07/15/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/*!*******************************************************************
\class        AudioManager
\brief		  Manages all sounds and music being used for this project.
              Provide interfaces for sound like playting, mute, etc.
********************************************************************/
public class AudioManager : MonoBehaviour
{
    // Singleton pattern
    public static AudioManager instance;
    private AudioManager() { }

    public AudioSource background; //! Background music.
    public List<Sound> sounds = new List<Sound>(); //! List of all SFX used in this project.

    private void Awake()
    {
        // Singlton
        if (instance == null)
            instance = this;
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        // Create AudioSource for each sound file.
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            
            sound.source.volume = 1.0f;
            sound.name = sound.source.clip.name;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Play("Click");
    }

    /*!
     * \brief Find a sound corresponds to given name and play that.
     */
    public void Play(string name)
    {
        Sound sound = sounds.Find(s => s.name == name);

        if (sound != null)
            sound.source.PlayOneShot(sound.clip);
    }

    /*!
     * \brief Set pitch of a sound corresponds to given name by given pitch.
     */
    public void SetPitch(string name, float pitch)
    {
        Sound sound = sounds.Find(s => s.name == name);

        if (sound == null)
            return;

        sound.source.pitch = pitch;
    }

    /*!
     * \brief Set volume of all SFX.
     */
    public void SetSFXVolume(float volume)
    {
        foreach (Sound sound in sounds)
            sound.source.volume = volume;
    }

    /*!
     * \brief Set volume of backgournd music.
     */
    public void SetMusicVolume(float volume)
    {
        background.volume = volume;
    }

    /*!
     * \brief Set mute of all SFX.
     */
    public void SetSFXMute(bool mute)
    {
        foreach (Sound sound in sounds)
            sound.source.mute = mute;
    }

    /*!
     * \brief Set mute of background music.
     */
    public void SetMusicMute(bool mute)
    {
        background.mute = mute;
    }
}
