using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new(0, 0, -10);
    public float smoothTime;
    Vector3 currentVelocity;

    private void FixedUpdate()
    {
        Vector3 vector3 = Vector3.SmoothDamp(
                    transform.position,
                    target.position + offset,
                    ref currentVelocity,
                    smoothTime
                    );
        transform.position = vector3;
    }
}
