using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class Planet : MonoBehaviour {

    Vector3 randomAxis;

    public PlanetStats planetStats;
    public float rotateSpeed = 25f;

    public bool visited = false;

    void Start()
    {
        // Assign a random axis for the planet to spin on:
        randomAxis = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
    }

    // Update is called once per frame
    void Update () {
        // Assign the scale according to the PlanetStats associated with this planet:
        transform.localScale = new Vector3(planetStats.size, planetStats.size, planetStats.size);

        Rotation();
	}

    void Rotation()
    {
        transform.Rotate(randomAxis * rotateSpeed * Time.deltaTime);
    }
}
