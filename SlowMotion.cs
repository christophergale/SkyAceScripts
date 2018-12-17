using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMotion : MonoBehaviour {

    public float modifier = 0.25f;
    float deltaTime;

    void Awake()
    {
        deltaTime = Time.fixedDeltaTime;
    }

    // Update is called once per frame
    void Update () {
		if (Input.GetMouseButton(0) && Ball.instance.planetStats.isSatellite)
        {
            AudioManager.instance.slowedTime = true;
            Time.timeScale = modifier;
            Time.fixedDeltaTime = deltaTime * modifier;
            // AudioManager.instance.soundtrack.pitch = modifier * 2f;
        } else
        {
            AudioManager.instance.slowedTime = false;
            Time.timeScale = 1f;
            // AudioManager.instance.soundtrack.pitch = 1f;
            Time.fixedDeltaTime = 0.02f;
        }
	}
}
