using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityCheck : MonoBehaviour {

    public GameObject planet;

    private void Start()
    {
        planet = transform.parent.gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Ball.instance.planet = planet;
            Ball.instance.ReCentre();
        }
    }
}
