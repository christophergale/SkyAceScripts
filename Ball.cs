using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EZCameraShake;

public class Ball : MonoBehaviour {

    public static Ball instance = null;

    // Component references ////////////////////////
    public Rigidbody rb;
    private LineRenderer lineRenderer;

    // Planet ////////////////////////
    [HideInInspector] public GameObject planet;
    [HideInInspector] public PlanetStats planetStats;
    [HideInInspector] public Planet planetClass;
    private Transform center;

    // Values ////////////////////////
    public float force;
    public float catapultForce;

    // Bools ////////////////////////
    [HideInInspector] public bool isOrbiting = false;
    [HideInInspector] public bool levelComplete;
    [HideInInspector] public bool gunner = false;

    // Explorer achievement ////////////////////////
    [HideInInspector] public int visitedIndex = 0;

    // GameObjects ////////////////////////
    public GameObject explosion;
    GameObject explosionObject;

    // UI ////////////////////////

    // Orbiting ////////////////////////
    Vector3 r0;
    float angleSpeed;
    float angle;
    float limitedDistance;
    bool readyToOrbit;
    Vector3 orbitVelocity;
    bool orbitBonus = false;
    Vector3 randomAxis;
    float rotateSpeed = 25f;

    private void Awake()
    {
        #region instance
        if (instance == null)
            instance = this;

        else if (instance == !this)
            Destroy(this.gameObject);
        #endregion
    }

    // Use this for initialization
    void Start () {
        // Get a reference to the Rigidbody component:
        rb = GetComponent<Rigidbody>();
        // Get a reference to the LineRenderer component for aiming:
        lineRenderer = GetComponentInChildren<LineRenderer>();
        // Assign a starting velocity to the Rigidbody:
        rb.velocity = new Vector3(10f, 0f, 0f);

        randomAxis = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.gamePaused)
        {
            // Check if the player has pressed the left mouse button and if they have, call Catapult()
            if (isOrbiting && Input.GetMouseButtonDown(0) && !planetStats.isSatellite)
            {
                Catapult();
            }

            // Check if the player is pressing the right mouse button and they are currently orbiting a satellite and if so, call Aim()
            else if (Input.GetMouseButton(0) && planetStats.isSatellite)
            {
                Aim();
            }

            if (Input.GetMouseButtonUp(0) && planetStats.isSatellite)
            {
                Catapult();
            }

            // When the player stops pressing the right mouse button, disable the aiming line renderer
            if (Input.GetMouseButtonUp(0))
            {
                lineRenderer.enabled = false;
            }
        }
    }

    private void FixedUpdate()
    {
        // Check if isOrbiting is true. If true, call Orbit()
        if (isOrbiting)
        {
            Orbit();
        }
    }

    public void ReCentre()
    {
        // Get a reference to the Planet component of the planet, so that we can access its planetStats
        planetClass = planet.GetComponent<Planet>();
        // Assign the planetStats variable the values for the planetStats component on the current planet:
        planetStats = planetClass.planetStats;
        // Set the orbit center to be the transform of the current planet:
        center = planet.transform;

        //if (planet.GetComponent<Quest>() != null)
        //{
        //    GameManager.instance.activeQuest = planet.GetComponent<Quest>();
        //    GameManager.instance.questLog.text = "Liberate " + planetStats.name;
        //}

        if (planet.GetComponent<QuestRace>())
        {
            planet.GetComponent<QuestRace>().StartRace();
        }

        // Set the force variable according to the speed of the current planet:
        force = force * planetStats.orbitSpeed;

        // Change the planetAudio for the new planet
        AudioManager.instance.CheckPlanetAudio();

        // Set the isOrbiting variable to true so that Orbit() will be called:
        isOrbiting = true;

        // If the planet has not yet been visited, add it to the visited index of the GameManager so as to keep track of which planets the player has visited
        if (planetClass.visited != true && planetStats.isPlanet)
        {
            // visitedIndex defaults to 0, so the first planet visited is stored at visited[0]
            GameManager.instance.visited[visitedIndex] = planetClass;
            // We then change this planet's visited bool to true, so that it cannot be added to the array a second time
            planetClass.visited = true;
            // Then we add 1 to visitedIndex so that the next planet that the player visits will be stored at the next index of the array
            visitedIndex += 1;
        }

        AudioManager.instance.UpdateSoundtrack();
    }

    void Orbit()
    {
        r0 = (center.position - transform.position);
        angleSpeed = 2f;

        // Calculate the angle between the ball's velocity and the vector between ball and planet
        angle = Mathf.Abs(Vector3.Angle(rb.velocity, r0));

        // Calculate the limitedDistance variable, which is a float equal to the size of the planet mutiplied by 2
        limitedDistance = 0.3f * planetStats.size * planetStats.gravityRadiusMultiplier;

        // Calculate the orbit velocity 
        // by calculating the cross product of the ball's velocity and the vector between ball and planet
        // and subsequently finding the cross product of that vector with the vector between ball and planet
        orbitVelocity = Vector3.Cross(r0, Vector3.Cross(rb.velocity, r0));

        // Whilst orbiting, spin the ball on a random axis
        transform.Rotate(randomAxis * rotateSpeed * Time.deltaTime);

        CheckAngle();
    }

    void CheckAngle() {
        // First, we check if the angle between the ball's velocity and the vector between ball and planet is too sharp 
        // and if the vector between ball and planet is less than the size of the planet multiplied by 2
        if (angle >= 10f && angle < 80f && r0.magnitude <= limitedDistance)
        {
            rb.AddForce(-r0 * 10f);
        }

        // If the angle between the ball's velocity and the vector between ball and planet is nearly 90 degress
        // i.e. between 85 and 105 degress
        // then we begin orbiting
        if (angle <= 105f && angle >= 85f)
        {
            // Calculate the range at which the orbit bonus sound effect will play
            float orbitBonusRange = ((planet.transform.localScale.x * planet.GetComponentInChildren<GravitationalRange>().gameObject.transform.localScale.x) * 0.5f) - 0.5f;
            // If the distance between ball and planet is within that range, call the OrbitBonus function of the AudioManager
            if (r0.magnitude > orbitBonusRange && orbitBonus == false)
            {
                Debug.Log("Orbit Bonus!");
                orbitBonus = true;
                AudioManager.instance.OrbitBonus();
            }

            // Turn off gunner
            gunner = false;

            // Check if the planet is a checkpoint and update the spawn point
            if (planetStats.isCheckpoint)
            {
                UpdateSpawn();
            }

            // Add orbit force
            AddOrbitForce();
        }
    }

    void AddOrbitForce() {
        // First we set the ball's velocity to be equal to the normalized orbitVelocity, multiplied by anglespeed (2)
        //which makes the ball's orbit anglespeed a constant
        // this is multiplied by the magnitude of the vector between ball and planet
        // which makes the ball orbit faster the further from the planet it is
        rb.velocity = orbitVelocity.normalized * angleSpeed * r0.magnitude;

        // We add a force to the Rigidbody
        // this is equal to the normalized vector between ball and planet
        // multiplied by the angleSpeed (2) multiplied by the angleSpeed again (2)
        // multiplied by the magnitude of the vector between ball and planet
        // multiplied by the mass of the ball
        // multiplied by 0.5
        rb.AddForce(r0.normalized * angleSpeed * angleSpeed * r0.magnitude * rb.mass * 0.5f);
    }

    void Catapult()
    {
        // If the planetStats for the planet the player is currently orbiting isMilitary, then we change the gunner bool to true
        gunner = planetStats.isMilitary;

        // Cancel orbiting
        isOrbiting = false;

        orbitBonus = false;

        // Play hit sound effects
        AudioManager.instance.HitSFX();

        // Add an impulse force to the Rigidbody in the axis of the player's normalized velocity
        rb.AddForce(rb.velocity.normalized * catapultForce, ForceMode.Impulse);
    }

    void Aim()
    {
        // Set the first position of the LineRenderer to be the player's position
        lineRenderer.SetPosition(0, transform.position);

        // Set the second position of the LineRenderer to be in the axis of the player's position + the player's velocity, multipled by 10
        lineRenderer.SetPosition(1, transform.position + rb.velocity * 10f);

        // Enable the LineRenderer so that it is visible
        lineRenderer.enabled = true;
    }

    public void Crash()
    {
        AudioManager.instance.scream.pitch = Random.Range(0.75f, 1.25f);
        AudioManager.instance.scream.Play();

        // Shake the camera using the CameraShaker script
        CameraShaker.Instance.ShakeOnce(30f, 30f, 0.1f, 1.5f);
        // Instantiate an explosion particle effect prefab at the player's current position
        LoadExplosion(this.transform);

        // Play the explosion sound effect
        AudioManager.instance.explosion.pitch = Random.Range(0.75f, 1.25f);
        AudioManager.instance.explosion.Play();
        gameObject.SetActive(false);
    }

    public void LoadExplosion(Transform target)
    {
        if (explosionObject == null)
        {
            explosionObject = Instantiate(explosion, target.transform.position, Quaternion.identity);
        }
        else
        {
            Destroy(explosionObject);
            explosionObject = Instantiate(explosion, target.transform.position, Quaternion.identity);
        }
    }

    void UpdateSpawn() {
        GameManager.instance.spawnPoint = center.position + Vector3.up * 5f;
    }

    // COLLISIONS ////////////////////////
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Planet")
        {
            Crash();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Portal")
        {
            // SceneController.instance.NewScene(other.GetComponent<Portal>().portalIndex);
        }

        if (other.tag == "Pickup")
        {
            Debug.Log("Pickup");
            Destroy(other.gameObject);
            AudioManager.instance.PickupSFX();
            GameManager.instance.pickups += 1;
        }

        if (other.tag == "PickupWall" && GameManager.instance.pickups >= other.GetComponent<PickupWall>().pickupsRequired)
        {
            LoadExplosion(other.transform);
            AudioManager.instance.explosion.Play();
            Destroy(other.gameObject);
        } else if (other.tag == "PickupWall" && !(GameManager.instance.pickups > other.GetComponent<PickupWall>().pickupsRequired))
        {
            Crash();
        }

        if (other.tag == "Enemy" && !gunner)
        {
            Crash();
        } else if (other.tag == "Enemy" && gunner)
        {
            LoadExplosion(other.transform);
            AudioManager.instance.explosion.Play();
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Background")
        {
            Crash();
        }

        if (other.tag == "ScreamTrigger")
        {
            AudioManager.instance.noNoNoNo.Play();
        }
    }
}
