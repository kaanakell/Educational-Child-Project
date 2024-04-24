using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMove : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of camera movement

    public Vector3 moveDirection = Vector3.right;

    void Update()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }
}
