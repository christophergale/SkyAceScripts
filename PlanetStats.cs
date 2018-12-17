using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Planet", menuName = "Planet", order = 1)]
public class PlanetStats : ScriptableObject {
    public string planetName;
    [Range(0.5f, 8.0f)]
    public float size;
    [Range(0.5f, 30.0f)]
    public float gravityRadiusMultiplier;
    [Range(0.25f, 3f)]
    public float orbitSpeed;
    public float scoreMultiplier;
    public bool isPlanet;
    public bool isSatellite;
    public bool isMilitary;
    public bool isCheckpoint;
    public string lore;
    public AudioClip planetAudio;
    public AudioClip announcer;
    public int soundtrackIndex;
}
