using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

    Vector3 randomAxis;

    GameObject planet;
    public Transform center;
    public Vector3 axis = Vector3.back;
    public Vector3 desiredPosition;
    public float radius = 2.0f;
    [HideInInspector] public float radiusSpeed = 1000f;
    public float rotationSpeed = 80.0f;

    public virtual void Start()
    {
        // Assign a random axis for the planet to spin on:
        randomAxis = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);

        if (planet != null)
        {
            center = planet.transform;
            transform.position = (transform.position - center.position).normalized * radius + center.position;
            radius = 2.0f;
        }
    }

    public virtual void Update()
    {
        Rotate();
        if (center)
        {
            transform.RotateAround(center.position, axis, rotationSpeed * Time.deltaTime);
            desiredPosition = (transform.position - center.position).normalized * radius + center.position;
            transform.position = Vector3.MoveTowards(transform.position, desiredPosition, Time.deltaTime * radiusSpeed);
        }
    }

    public void Rotate()
    {
        transform.Rotate(randomAxis * Time.deltaTime * 25f);
    }
}
