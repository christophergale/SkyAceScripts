using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetText : MonoBehaviour {

    string planetName;
    public Text planetText;

    public CameraFollow camFollow;
	
	// Update is called once per frame
	void Update () {
        planetName = Ball.instance.planetStats.name;
        planetText.text = planetName;

        if (camFollow.currentZoom > 10f)
        {
            planetText.enabled = false;
        } else
        {
            planetText.enabled = true;
        }
	}
}
