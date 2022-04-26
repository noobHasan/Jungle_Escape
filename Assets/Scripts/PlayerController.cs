using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float MainSpeed;
    private float speed;
    private float rotationOffset;

    private Rigidbody rb;

    private Vector2 lastMousePos;
    public bool canMove;

    public Animator anim;
    private static int ANIMATOR_PARAM_WALK_SPEED = Animator.StringToHash("speed");


    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rotationOffset = 270;
        canMove = true;
        
    }


    private void FixedUpdate()
    {
        Vector2 deltaPos = Vector2.zero;

        if (JoyStickController.Instance.Touched && canMove)

        {
            Vector2 currenMousePos = Input.mousePosition;

            if (lastMousePos == Vector2.zero)
            {
                lastMousePos = currenMousePos;
            }

            deltaPos = currenMousePos - lastMousePos;

            float angle = Mathf.Atan2(deltaPos.y, deltaPos.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, -(angle + rotationOffset), 0), 9f * Time.deltaTime);

            Vector3 dir = (currenMousePos - lastMousePos).normalized;

            if (deltaPos.magnitude > 200)
            {
                speed = MainSpeed;
            }
            else
            {
                speed = MainSpeed;

            }

            rb.velocity = new Vector3(dir.x * speed, rb.velocity.y, dir.y * speed);
        }

        else
        {
            lastMousePos = Vector2.zero;
            float VelocityReduce = 1.5f;
            rb.velocity = new Vector3(rb.velocity.x / VelocityReduce, rb.velocity.y / VelocityReduce, rb.velocity.z / VelocityReduce);
        }

        float rush = rb.velocity.magnitude;
        anim.SetFloat(ANIMATOR_PARAM_WALK_SPEED, rush);

    }
}
