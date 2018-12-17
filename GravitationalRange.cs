using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GravitationalRange : MonoBehaviour {

    public PlanetStats planetStats;

	// Use this for initialization
	void Update () {
        planetStats = GetComponentInParent<Planet>().planetStats;
        float scale = planetStats.size * planetStats.gravityRadiusMultiplier;
        transform.localScale = new Vector3(scale, scale, scale);
	}
	
}
