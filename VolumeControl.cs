using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour {

    public Slider musicVolume;
    public void UpdateVolume()
    {
        AudioManager.instance.musicVolume = musicVolume.value;
    }
}
