using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform playerTarget;

    void Start()
    {

    }

    void FixedUpdate()
    {
        Vector3 playerTargetPos = new Vector3(playerTarget.position.x, playerTarget.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, playerTargetPos, 0.2f);
    }
}
