using UnityEngine;

public class OneWay : MonoBehaviour {

    private Planet planet;
    private GravityCheck checkGravity;
    private bool oneway=false;
    private void Start()
    {
        planet = GetComponent<Planet>();
        checkGravity = GetComponentInChildren<GravityCheck>();
    }

    // Update is called once per frame
    void Update () {

        if (planet.visited && !oneway)
        {

            checkGravity.enabled = !checkGravity.enabled;
            oneway = true;
        }
	}
}
