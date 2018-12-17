using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform startPlanet;

    public Vector3 offset;
    public float smoothTime;

    Transform planet;

    private Camera cam;

    public float currentZoom = 25f;
    float targetZoom = 25f;
    float minZoom = 5f;
    float maxZoom = 25f;

    Vector3 velocity = Vector3.zero;
    float smooth;

    public float mouseMultiplier;

    void Start()
    {
        cam = GetComponent<Camera>();
        //transform.position = startPlanet.position + offset;
    }

    // Update is called once per frame
    void Update () {
        if (Camera.main.GetComponent<CameraFollow>().currentZoom < 10f)
        {
            Follow();
        } else
        {
            FollowBall();
        }

        // The following code controls camera zooming with the scroll wheel:
        float scroll = Input.GetAxisRaw("Mouse ScrollWheel") * 10f;

        if (Mathf.Abs(scroll) > 0f)
        {
            targetZoom = Mathf.Clamp(targetZoom - scroll, minZoom, maxZoom);
        }

        currentZoom = Mathf.SmoothDamp(currentZoom, targetZoom, ref smooth, .15f);

        cam.orthographicSize = currentZoom;

        //if (Ball.instance.isOrbiting)
        //    MouseControl();

    }

    void Follow()
    {
        planet = Ball.instance.planet.transform;
        Vector3 targetPosition = planet.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    void FollowBall()
    {
        Vector3 targetPosition = Ball.instance.transform.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime / 1.5f);
    }

    //void MouseControl()
    //{
    //    float x = Input.GetAxis("Mouse X") * mouseMultiplier;
    //    float y = Input.GetAxis("Mouse Y") * mouseMultiplier;

    //    Vector3 targetPosition = transform.position + new Vector3(x, y, 0f);

    //    transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    //}
}
