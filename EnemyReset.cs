using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReset : MonoBehaviour {

    Vector3 startPos;
    PlayerFollow playerFollow;
    bool followActive;

	// Use this for initialization
	void Start () {
        startPos = transform.position;
        playerFollow = GetComponent<PlayerFollow>();
        followActive = playerFollow.enabled;
	}

    public void ResetEnemy() {
        transform.position = startPos;
        playerFollow.enabled = followActive;
    }
}
