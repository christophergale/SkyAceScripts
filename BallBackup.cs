//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using EZCameraShake;

//public class Ball : MonoBehaviour {

//    public static Ball instance = null;

//    // Component references ////////////////////////
//    public Rigidbody rb;
//    private LineRenderer lineRenderer;

//    // Planet ////////////////////////
//    [HideInInspector] public GameObject planet;
//    [HideInInspector] public PlanetStats planetStats;
//    [HideInInspector] public Planet planetClass;
//    private Transform center;

//    // Values ////////////////////////
//    public float force;
//    public float catapultForce;

//    // Bools ////////////////////////
//    [HideInInspector] public bool isOrbiting = false;
//    [HideInInspector] public bool inOrbit = false;
//    [HideInInspector] public bool levelComplete;
//    [HideInInspector] public bool gunner = false;

//    // Explorer achievement ////////////////////////
//    [HideInInspector] public int visitedIndex = 0;

//    // GameObjects ////////////////////////
//    public GameObject explosion;
//    GameObject explosionObject;

//    // UI ////////////////////////

//    // Orbiting ////////////////////////
//    Vector3 r0;
//    float anglespeed;
//    float angle;
//    float limiteddist;


//    // public bool teeOff = true;
//    // public Slider powerSlider;
//    // float teeOffPower;

//    private void Awake()
//    {
//        #region instance
//        if (instance == null)
//            instance = this;

//        else if (instance == !this)
//            Destroy(this.gameObject);
//        #endregion
//    }

//    // Use this for initialization
//    void Start () {
//        // Get a reference to the Rigidbody component:
//        rb = GetComponent<Rigidbody>();
//        // Get a reference to the LineRenderer component for aiming:
//        lineRenderer = GetComponentInChildren<LineRenderer>();
//        // Assign a starting velocity to the Rigidbody:
//        rb.velocity = new Vector3(10f, 0f, 0f);
//    }

//    // Update is called once per frame
//    void Update()
//    {   
//        // Check if the player has pressed the left mouse button and if they have, call Catapult()
//        if (isOrbiting && Input.GetMouseButtonDown(0) && !planetStats.isSatellite)
//        {
//            Catapult();
//        }
//        // Check if the player is pressing the right mouse button and they are currently orbiting a satellite and if so, call Aim()
//        else if (Input.GetMouseButton(0) && planetStats.isSatellite)
//        {
//            Aim();
//        }

//        if (Input.GetMouseButtonUp(0) && planetStats.isSatellite)
//        {
//            Catapult();
//        }
//        // When the player stops pressing the right mouse button, disable the aiming line renderer
//        if (Input.GetMouseButtonUp(0))
//        {
//            lineRenderer.enabled = false;
//        }

//        //if (teeOff)
//        //{
//        //    TeeOff();
//        //}
//    }

//    private void FixedUpdate()
//    {
//        // Check if isOrbiting is true. If true, call Orbit()
//        if (isOrbiting)
//        {
//            Orbit();
//        }
//    }

//    public void ReCentre()
//    {
//        // Get a reference to the Planet component of the planet, so that we can access its planetStats
//        planetClass = planet.GetComponent<Planet>();
//        // Assign the planetStats variable the values for the planetStats component on the current planet:
//        planetStats = planetClass.planetStats;
//        // Set the orbit center to be the transform of the current planet:
//        center = planet.transform;

//        if (planet.GetComponent<QuestRace>())
//        {
//            planet.GetComponent<QuestRace>().StartRace();
//        }

//        // Set the force variable according to the speed of the current planet:
//        force = force * planetStats.orbitSpeed;

//        // Change the planetAudio for the new planet
//        AudioManager.instance.CheckPlanetAudio();

//        // Set the isOrbiting variable to true so that Orbit() will be called:
//        isOrbiting = true;

//        // If the planet has not yet been visited, add it to the visited index of the GameManager so as to keep track of which planets the player has visited
//        if (planetClass.visited != true && planetStats.isPlanet)
//        {
//            // visitedIndex defaults to 0, so the first planet visited is stored at visited[0]
//            GameManager.instance.visited[visitedIndex] = planetClass;
//            // We then change this planet's visited bool to true, so that it cannot be added to the array a second time
//            planetClass.visited = true;
//            // Then we add 1 to visitedIndex so that the next planet that the player visits will be stored at the next index of the array
//            visitedIndex += 1;
//        }
//    }

//    void Orbit()
//    {
//        r0 = (center.position - transform.position);
//        anglespeed = 2f;
        
//        //float velocity1 = Mathf.Sqrt(force / r0.magnitude);
//        //float velocity2 = Mathf.Sqrt(2.0f) * velocity1;
//        //get the basic status of the ball and calculate the first cosmic velocity(which make the ball have the circle orbit around a planet)
//        angle = Mathf.Abs(Vector3.Angle(rb.velocity, r0));
//        limiteddist = 2f* planetStats.size;

//        Vector3 orbitvelocity = Vector3.Cross(r0, Vector3.Cross(rb.velocity, r0));
//        //change the velocity of the ball has the vertical direction with the position vector
//        //also a condition of circle orbit
//        if(angle >=5 && angle < 80 && r0.magnitude<=limiteddist)
//        {
//            rb.AddForce(-r0 * 10);
//        }

//        //use the angle of velocity and position to evaluate if the ball is in the right position and start orbiting
//        if (angle <= 95f && angle >= 85f )
//        {
//            gunner = false;

//            if (planetStats.isCheckpoint)
//            {
//                GameManager.instance.spawnPoint = center.position + Vector3.up * 5f;
//            }

//            //rb.velocity = orbitvelocity.normalized * 10f;
//            rb.velocity = orbitvelocity.normalized * anglespeed * r0.magnitude;

//            inOrbit = true;
//            //rb.AddForce(r0.normalized * rb.velocity.magnitude * rb.velocity.magnitude *rb.mass/(2f*r0.magnitude));
//            rb.AddForce(r0.normalized * anglespeed * anglespeed * r0.magnitude * rb.mass * 0.5f);

//            //limitation of the orbit
//            //if (r0.magnitude < 2f * limiteddist)
//            //{
//            //    rb.velocity *= 1.5f;
//            //}

//            //if (planetStats.isGoal)
//            //{
//            //    rb.AddForce(r0.normalized * force * rb.mass * (1f + Time.deltaTime) / (r0.magnitude * r0.magnitude));
//            //}
//        }
//        else if(angle <= 95f && angle >= 85f && r0.magnitude < limiteddist)
//        {
//            //rb.velocity = orbitvelocity.normalized * 10f;
//            rb.velocity = orbitvelocity.normalized * anglespeed * r0.magnitude;

//            inOrbit = true;
//            //rb.AddForce(r0.normalized * rb.velocity.magnitude * rb.velocity.magnitude *rb.mass / (5f * r0.magnitude));
//            rb.AddForce(r0.normalized * anglespeed * anglespeed * r0.magnitude * rb.mass * 0.5f);
//        }
//        //add a force to keep the ball in the circle orbit
        
//        transform.Rotate(transform.position - planet.transform.position, 200f * Time.deltaTime);

//        // Debug.Log(r0.magnitude);
//    }

//    void Catapult()
//    {
//        // If the planetStats for the planet the player is currently orbiting isMilitary, then we change the gunner bool to true
//        if (planetStats.isMilitary)
//        {
//            gunner = true;
//        }

//        // Cancel orbiting
//        isOrbiting = false;
//        inOrbit = false;

//        // Play hit sound effects
//        AudioManager.instance.HitSFX();

//        // Add an impulse force to the Rigidbody in the axis of the player's normalized velocity
//        rb.AddForce(rb.velocity.normalized * catapultForce, ForceMode.Impulse);
//    }

//    void Aim()
//    {
//        // Set the first position of the LineRenderer to be the player's position
//        lineRenderer.SetPosition(0, transform.position);
//        // Set the second position of the LineRenderer to be in the axis of the player's position + the player's velocity, multipled by 10
//        lineRenderer.SetPosition(1, transform.position + rb.velocity * 10f);

//        // Enable the LineRenderer so that it is visible
//        lineRenderer.enabled = true;
//    }

//    public void Crash()
//    {
//        // Shake the camera using the CameraShaker script
//        CameraShaker.Instance.ShakeOnce(30f, 30f, 0.1f, 1.5f);
//        // Instantiate an explosion particle effect prefab at the player's current position
//        LoadExplosion(this.transform);

//        // Play the explosion sound effect
//        AudioManager.instance.explosion.Play();
//        gameObject.SetActive(false);
//    }

//    public void LoadExplosion(Transform target)
//    {
//        if (explosionObject == null)
//        {
//            explosionObject = Instantiate(explosion, target.transform.position, Quaternion.identity);
//        }
//        else
//        {
//            Destroy(explosionObject);
//            explosionObject = Instantiate(explosion, target.transform.position, Quaternion.identity);
//        }
//    }

//    // COLLISIONS ////////////////////////
//    private void OnCollisionEnter(Collision collision)
//    {
//        if (collision.gameObject.tag == "Planet")
//        {
//            Crash();
//        }
//    }

//    private void OnTriggerEnter(Collider other)
//    {
//        if (other.tag == "Portal")
//        {
//            SceneController.instance.NewScene(other.GetComponent<Portal>().portalIndex);
//            AudioManager.instance.BlackHole();
//            GetComponent<Animator>().SetTrigger("Shrink");
//        }

//        if (other.tag == "Pickup")
//        {
//            Debug.Log("Pickup");
//            Destroy(other.gameObject);
//            AudioManager.instance.PickupSFX();
//            GameManager.instance.pickups += 1;
//        }

//        if (other.tag == "PickupWall" && GameManager.instance.pickups >= other.GetComponent<PickupWall>().pickupsRequired)
//        {
//            LoadExplosion(other.transform);
//            AudioManager.instance.explosion.Play();
//            Destroy(other.gameObject);
//        } else if (other.tag == "PickupWall" && !(GameManager.instance.pickups > other.GetComponent<PickupWall>().pickupsRequired))
//        {
//            Crash();
//        }

//        if (other.tag == "Enemy" && !gunner)
//        {
//            Crash();
//        } else if (other.tag == "Enemy" && gunner)
//        {
//            LoadExplosion(other.transform);
//            AudioManager.instance.explosion.Play();
//            Destroy(other.gameObject);
//        }
//    }

//    private void OnTriggerExit(Collider other)
//    {
//        if (other.tag == "Background")
//        {
//            gameObject.SetActive(false);
//        }
//    }

//    //void TeeOff()
//    //{
//    //    powerSlider.value += 0.5f * Time.deltaTime;
//    //    if (Input.GetMouseButtonDown(0))
//    //    {
//    //        teeOff = false;
//    //        teeOffPower = powerSlider.value;
//    //        ReCentre();
//    //    }
//    //}

//    //void Putt()
//    //{
//    //    isOrbiting = false;
//    //    levelComplete = true;

//    //    rb.velocity = Vector3.zero;

//    //    GetComponent<SphereCollider>().enabled = false;
//    //    planet.GetComponent<SphereCollider>().enabled = false;
//    //    planet.GetComponent<Planet>().enabled = false;

//    //    transform.position = Vector3.MoveTowards(transform.position, planet.transform.position, 3f);

//    //    //Instantiate(explosion, planet.transform.position, Quaternion.identity);
//    //    //Destroy(planet.gameObject);
//    //    //Destroy(this.gameObject);
//    //}
//}
