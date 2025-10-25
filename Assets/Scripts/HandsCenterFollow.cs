using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsCenterFollow : MonoBehaviour
{
    public Transform leftHand;
    public Transform rightHand;

    void Update()
    {
        if (leftHand && rightHand)
        {
            transform.position = (leftHand.position + rightHand.position) / 2;
            // Rotation: look from left hand to right hand
            Vector3 direction = rightHand.position - leftHand.position;
            if (direction != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}

