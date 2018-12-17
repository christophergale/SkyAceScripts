using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour {

    public static BackgroundScroll instance;

    public float smoothTime;
    public float rangeMultiply;
    Vector3 targetPosition;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else if (instance != null)
        {
            Destroy(this.gameObject);
        }
        
    }

    // Update is called once per frame
    void Update () {
        targetPosition = Camera.main.transform.position * rangeMultiply;
        targetPosition.z = 5f;
        Vector3 velocity = Vector3.zero;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime / 1.5f);
    }

    public void ResetBackground()
    {
        targetPosition = GameManager.instance.spawnPoint;
        targetPosition.z = 5f;
        transform.position = targetPosition;
    }
}
