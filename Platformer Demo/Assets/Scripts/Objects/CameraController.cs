using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("References")]
    public GameObject target;

    [Header("Camera Settings")]
    public float cameraSpeed;
    public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        followTarget();
    }

    void followTarget(){
        // Vector2 targetPosition = new Vector2(target.transform.position.x+offset.x, target.transform.position.y+offset.y);
        Vector3 targetPosition = target.transform.position + offset;

        transform.position = Vector3.Lerp(transform.position, targetPosition, cameraSpeed);
    }
}
