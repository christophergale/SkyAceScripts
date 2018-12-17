using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour {

    public float speed;
	
	// Update is called once per frame
	void Update () {
        if (!GameManager.instance.gameOver)
        {
            transform.LookAt(Ball.instance.gameObject.transform.position);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
	}
}
