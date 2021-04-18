using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // T20 Holds the single instance of the SoundManager that 
    // you can access from any script, a singleton
    public static SoundManager Instance = null;

    // T20 MsPacman audio clips
    public AudioClip sniffingDots;
    public AudioClip eatingGhost;
    public AudioClip ghostMove;
    public AudioClip pacmanDies;
    public AudioClip powerupEating;
    public AudioClip Dynomite;

    // T20 Refers to the audio source used for Pac-Man
    // eating dots, sniffing dots
    private AudioSource pacmanAudioSource;

    // T20 Plays Ghost moving loop sound
    private AudioSource ghostAudioSource;

    // T20 // Used for playing one shot audio clips
    private AudioSource oneShotAudioSource;

    // T20 Start is called before the first frame update
    void Start()
    {
        // This is a singleton that makes sure you only
        // ever have one Sound Manager
        // if there is no soundmanager (Instance == null)
        // then create one(Instance = this;)
        // If there is any other Sound Manager(Instance != this) created, destroy it
        if (Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }

        // T20 grab all in an array of Audiosources, to Use multiple AudioSources
        // gets the first one then the second one then the third
        AudioSource[] audioSources = GetComponents<AudioSource>();

        // T20 Used for Pac-Man eating dots
        pacmanAudioSource = audioSources[0];

        // T20 Used to play Ghost moving sound
        ghostAudioSource = audioSources[1];

        // T20 Used for one shots
        oneShotAudioSource = audioSources[2];

        // T20 called to Start Pac-Man eating sound, 
        // or start sniffing dots, in this version of the game
        PlayClipOnLoop(pacmanAudioSource, sniffingDots);
    }


    // T20 play Other GameObjects can call this to play sounds
    public void PlayOneShot(AudioClip clip)
    {
        oneShotAudioSource.PlayOneShot(clip);
    }



    // T20 Used to start playing the Pac-Man eating in a loop, aS = AudioSource
    // or start sniffing in a loop, in this version of the game, this also gets a AudioClip sent to it
    public void PlayClipOnLoop(AudioSource aS, AudioClip clip)
    {
        // Make sure we have an AudioSource and Clip to play
        if(aS != null && clip != null)
        {
            // set the sound to play on a loop
            aS.loop = true;

            // set the volume for the sound
            aS.volume = .3f;

            // play the clip
            aS.clip = clip;

            // play the clip out of the speakers
            aS.Play();
        }
    }


    // T20 easy public way to pause pacmans sniffing dots or eating dots sound
    // verify the AudioSource is not null and AudioSource is playing
    public void PausePacman()
    {
        if(pacmanAudioSource != null && pacmanAudioSource.isPlaying)
        {
            pacmanAudioSource.Stop();
        }
    }

    // T20 Restarts pac-Man sniffing dots, or eating dots sound
    // verify the AudioSource is not null and AudioSource is Not playing
    public void UnPausePacman()
    {
        if (pacmanAudioSource != null && !pacmanAudioSource.isPlaying)
        {
            pacmanAudioSource.Play();
        }
    }

    // T20 easy public way to pause ghosts eating sound
    // verify the AudioSource is not null and AudioSource is playing
    public void PauseGhost()
    {
        if (ghostAudioSource != null && ghostAudioSource.isPlaying)
        {
            ghostAudioSource.Stop();
        }
    }

    // T20 Restarts pac-Man sniffing dots, or eating dots sound
    // verify the AudioSource is not null and AudioSource is Not playing
    public void UnPauseGhost()
    {
        if (ghostAudioSource != null && !ghostAudioSource.isPlaying)
        {
            ghostAudioSource.Play();
        }
    }
}
