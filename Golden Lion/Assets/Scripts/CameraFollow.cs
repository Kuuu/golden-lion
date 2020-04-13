using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform target;
    public float smoothTime = 0.15f;

    public float offsetY = -10f;
    private float currentOffset;

    private Vector3 velocity;

    // Use this for initialization
    void Start()
    {
        currentOffset = offsetY;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target)
            transform.position = Vector3.SmoothDamp(transform.position, new Vector3(target.position.x, target.position.y + currentOffset, transform.position.z), ref velocity, smoothTime);
    }
}