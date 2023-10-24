using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// This is a helper class for the SFXManager class.
/// It is used to store information about the audiosources in the SFXManager game object.
/// </summary>
public class AudioInfo
{
    public AudioSource Player { get; }
    public string name { get; }
    // This is the volume of the audiosource when the game starts
    // It is being stored so that the overall SFX volume can be changed while maintaining the relative volume of each audiosource.
    public float InitVolume { get; }

    public AudioInfo(AudioSource player, float initVolume)
    {
        Player = player;
        // Base name on the clip assigned to the audiosource
        this.name = player.clip.ToString().Split(' ')[0];
        InitVolume = initVolume;
    }
}

/// <summary>
/// This class is used for controlling the in game SFX.
/// It is attached to the SFXManager game object.
/// It finds all the audiosources attached to the SFXManager game object and creates a list of AudioInfo objects.
/// When a sound effect is called to be played, it finds the audiosource with the name and plays it.
/// </summary>
public class SFXManager : MonoBehaviour
{
    #region Singleton
    public static SFXManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    #endregion

    [SerializeField] private List<AudioInfo> audioInfos = new List<AudioInfo>();
    // This is the initial overall SFX volume. This is what SFX slider in the settings menu is set to when the game starts.
    public float InitSFXLevel { get; } = 0.25f;

    private void Start()
    {
        // find all audiosources in this object
        foreach (AudioSource audioSource in GetComponents<AudioSource>())
        {
            AudioInfo newAudioInfo = new AudioInfo(audioSource, audioSource.volume);
            // set the volume of the audiosource to the initial SFX level relative to its initial volume.
            newAudioInfo.Player.volume = (newAudioInfo.InitVolume * InitSFXLevel);
            audioInfos.Add(newAudioInfo);
        }
    }

    /// <summary>
    /// This method is used to play a sound effect.
    /// </summary>
    /// <param name="name">The name of the sound effect to be played.</param>
    public void Play(string name)
    {
        // Find the audiosource with supplied name and play it
        foreach (AudioInfo audioInfo in audioInfos)
        {
            if (audioInfo.name == name)
            {
                audioInfo.Player.Play();
                return;
            }
        }
    }

    /// <summary>
    /// This method is used to change the overall SFX volume.
    /// </summary>
    /// <param name="volume">The new volume.</param>
    public void ChangeSFXVolume(float volume)
    {
        // Change the volume of each audiosource relative to its initial volume
        foreach (AudioInfo audioInfo in audioInfos)
        {
            audioInfo.Player.volume = (audioInfo.InitVolume * volume);
        }
    }
}