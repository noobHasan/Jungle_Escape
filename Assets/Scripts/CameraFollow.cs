using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smootSpeed;
    public Vector3 offset;
    public bool isBusy;
    private void Start()
    {
        offset = transform.position - target.position;
    }
    private void FixedUpdate()
    {
        if (isBusy)
        {
            return;
        }
        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, 0.5f);
    }
}