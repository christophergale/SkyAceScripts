using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    // Define an AudioManager instance that will be this script
    public static AudioManager instance = null;

    // Define all of our AudioSource components. Some of these will be assigned in the Unity Editor, some will be assigned by the AddComponent calls in Awake
    public AudioSource swing;
    public AudioSource weakSwing;
    public AudioSource explosion;
    public AudioSource satellite;
    public AudioSource planetAudio;
    public AudioSource announcer;
    public AudioSource respawn;
    public AudioSource orbitBonus;
    public AudioSource orbitBonusCheer;
    public AudioSource scream;
    public AudioSource noNoNoNo;

    public AudioClip[] pickupSFXClips;
    public AudioSource pickupSFX;
    public int pickupSFXIndex = 0;

    public AudioSource[] soundtracks;
    public AudioClip[] soundtrackClips;
    public int currentSoundtrackIndex;

    public float musicVolume;
    public float sFXVolume;

    public bool slowedTime;
    bool zoomed;
    bool announcerReady;

    private void Awake()
    {
        #region instance
        // If there is no instance of an AudioManager in the scene, set this script to be our AudioManager instance
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else if (instance != this)
        {
            Destroy(this.gameObject);
        }
        #endregion

        // Create an AudioSource component and assign it to the planetAudio variable
        planetAudio = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
        planetAudio.volume = 0.0f;
        // Ensure that the planetAudio is looping
        planetAudio.loop = true;

        // Create an AudioSource component and assign it to the announcer variable
        announcer = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
        // Set to not loop, not play on awake and Stop it (we will Play it later)
        announcer.Stop();
        announcer.loop = false;
        announcer.playOnAwake = false;

        pickupSFX = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
        pickupSFX.loop = false;
        pickupSFX.playOnAwake = false;

        musicVolume = 0.6f;

        soundtracks = new AudioSource[soundtrackClips.Length];

        // Create soundtrack AudioSources
        for (int i = 0; i < soundtrackClips.Length; i++)
        {
            soundtracks[i] = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
            soundtracks[i].clip = soundtrackClips[i];
            soundtracks[i].loop = true;
            soundtracks[i].volume = 0.0f;
        }

        for (int i = 0; i < soundtracks.Length; i++)
        {
            soundtracks[i].Play();
        }
    }
	
	// Update is called once per frame
	void Update () {
        //soundtrack.volume = musicVolume;

        // Check if the camera is zoomed in
        if (Camera.main.orthographicSize <= 8.0f)
        {
            // Set the planet audio to 100%
            planetAudio.volume = 1.0f;
            // Set the soundtrack volume to 0%
            for (int i = 0; i < soundtracks.Length; i++)
                soundtracks[i].volume = 0.0f;
        } else
        {
            zoomed = false;
            // Set the planet audio to 0%
            planetAudio.volume = 0.0f;
            // Set the soundtrack volume to 100%
            soundtracks[Ball.instance.planetStats.soundtrackIndex].volume = 1.0f;
        }

        if (Camera.main.orthographicSize <= 8.0 && !zoomed)
        {
            zoomed = true;
            announcerReady = true;
        }

        if (zoomed && announcerReady)
        {
            Announcer();
            announcerReady = false;
        }

            
        // Every frame, we calculate the player's position on the x axis, compared with the planet's position on the x axis and apply the result (divided by 10) to the panStereo value of planetAudio
        if (Ball.instance.planet != null)
            planetAudio.panStereo = (Ball.instance.planet.transform.position.x - Ball.instance.transform.position.x) / 10f;

        if (Ball.instance.planet != null && Ball.instance.planetStats.isSatellite)
        {
            satellite.volume = 1f;
        } else
        {
            satellite.volume = 0f;
        }
	}

    // CheckPlanetAudio is called by Ball.instance.ReCentre()
    public void CheckPlanetAudio()
    {
        // Check if planetAudio does not equal null
        if (planetAudio != null)
        {
            // If planetAudio exists, Stop the audio playing and assign new audio clip
            planetAudio.Stop();
            planetAudio.clip = Ball.instance.planetStats.planetAudio;
        }
        // Play the new clip
        planetAudio.Play();
    }

    public void Announcer()
    {
        // Assign a new audio clip and Play it
        if (Ball.instance.planet != null)
        {
            announcer.clip = Ball.instance.planetStats.announcer;
        }

        announcer.Play();
    }

    public void HitSFX()
    {
        if (slowedTime)
        {
            weakSwing.Play();
        } else
        {
            swing.pitch = Random.Range(0.75f, 1.25f);
            swing.Play();
        }
    }

    public void PickupSFX()
    {
        pickupSFX.clip = pickupSFXClips[pickupSFXIndex];
        pickupSFX.Stop();
        pickupSFX.Play();
        if (pickupSFXIndex < (pickupSFXClips.Length - 1))
        {
            pickupSFXIndex += 1;
        } else
        {
            PickupSFXReset();
        } 
    }

    public void PickupSFXReset()
    {
        pickupSFXIndex = 0;
    }

    public void OrbitBonus()
    {
        orbitBonus.Play();
        int randomNum = Random.Range(1, 4);
        if (randomNum == 3)
        {
            orbitBonusCheer.Play();
        }
    }

    public void UpdateSoundtrack()
    {
            currentSoundtrackIndex = Ball.instance.planetStats.soundtrackIndex;
            for (int i = 0; i < soundtracks.Length; i++)
            {
                if (i == currentSoundtrackIndex)
                {
                    soundtracks[i].volume = 1f;
                } else
                {
                    soundtracks[i].volume = 0f;
                }
            }
    }
}
