using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAlert : MonoBehaviour {

    public PlayerFollow[] enemies;
    bool alert = false;

    // Use this for initialization
    void Awake()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].enabled = false;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Ball.instance.planet.gameObject == this.gameObject) {
            alert = true;
        }

        if (alert) {
            AlertEnemies();
            alert = false;
        }
	}

    void AlertEnemies() {
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].enabled = true;
        }
    }
}
