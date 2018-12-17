using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitationalGraphics : MonoBehaviour {

    public PlanetStats planetStats;

    // Use this for initialization
    void Update()
    {
        planetStats = GetComponentInParent<Planet>().planetStats;
        float scale = planetStats.size * planetStats.gravityRadiusMultiplier + 0.5f;
        transform.localScale = new Vector3(scale, scale, scale);
    }
}
