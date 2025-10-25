using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketballController : MonoBehaviour
{
    public float MoveSpeed = 10;
    public Transform Ball;
    public Transform PosDribble;
    public Transform PosOverHead;
    public Transform Arms;
    public Transform Target;

    private bool IsBallInHands = true;
    private bool IsBallFlying = false;
    private float T = 0;

    void Update()
    {
        // walking
        Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        if (direction.magnitude > 0.1f)
        {
            transform.position += direction * MoveSpeed * Time.deltaTime;
            transform.forward = direction; 
        }

        // ball in hands
        if (IsBallInHands)
        {
            if (Input.GetKey(KeyCode.Space)) 
            {
                Ball.position = PosOverHead.position;

                // Rotate arms to face the target
                if (Target != null)
                {
                    Vector3 lookDir = (Target.position - Arms.position).normalized;
                    Arms.rotation = Quaternion.LookRotation(lookDir);
                }
            }
            else // dribbling
            {
                Ball.position = PosDribble.position + Vector3.up * Mathf.Abs(Mathf.Sin(Time.time * 5));
                Arms.localRotation = Quaternion.identity; // reset arms rotation
            }

            // throw ball
            if (Input.GetKeyUp(KeyCode.Space))
            {
                IsBallInHands = false;
                IsBallFlying = true;
                T = 0;
            }
        }

        // ball in the air : shoot
        if (IsBallFlying)
        {
            T += Time.deltaTime;
            float duration = 0.66f;
            float t01 = T / duration;

            Vector3 A = PosOverHead.position;
            Vector3 B = Target.position;
            Vector3 pos = Vector3.Lerp(A, B, t01);

            Vector3 arc = Vector3.up * 5 * Mathf.Sin(t01 * Mathf.PI);
            Ball.position = pos + arc;

            if (t01 >= 1)
            {
                IsBallFlying = false;
                Ball.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsBallInHands && !IsBallFlying)
        {
            IsBallInHands = true;
            Ball.GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}
