using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    private Transform target;

    [Header("Settings")]
    public bool lockCursor;
    public float mouseSensitivity = 10;
    public float distFromTarget = 10;
    public Vector2 pitchMinMax = new Vector2(-60, 85);

    public float rotationSoothTime = .8f;
    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;

    float yaw;
    float pitch;

    [Header("Collision Vars")]

    [Header("Transparicy")]
    public bool changeTransparency = true;
    public MeshRenderer targetRenderer;

    [Header("Speeds")]
    public float collisionSpeed = 5;
    public float returnSpeed = 9;
    public float wallPush = 0.7f;

    [Header("Distances")]
    public float closeDistanceToPlayer = 2;
    public float closestDistanceToPlayer = 1;

    [Header("Mask")]
    public LayerMask collisionMask;

    void Start()
    {
        //Finding Player and Pivot
        target = GameObject.FindGameObjectWithTag("Player").transform;
        targetRenderer = target.GetComponent<MeshRenderer>();

        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void LateUpdate()
    {
        CollisionCheck(target.position - transform.forward * distFromTarget);
        //Rotating camera with mouse
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        //target.Rotate(0, yaw, 0);
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

        currentRotation = Vector3.Lerp(currentRotation, new Vector3(pitch, yaw), rotationSoothTime * Time.deltaTime);

        transform.eulerAngles = currentRotation;

        Vector3 e = transform.eulerAngles;
        e.x = 0;

        if(target.GetComponent<CharacterController>().velocity == new Vector3(0,0,0))
        {
            return;
        }
        target.eulerAngles = e;
    }

    private void CollisionCheck(Vector3 retPoint)
    {
        RaycastHit hit;

        if(Physics.Linecast(target.position,retPoint,out hit, collisionMask))
        {
            Vector3 norm = hit.normal * wallPush;
            Vector3 p = hit.point + norm;

            TransparencyCheck();

            if(Vector3.Distance (Vector3.Lerp(transform.position, p, collisionSpeed * Time.deltaTime), target.position) < closestDistanceToPlayer)
            {
                transform.position = p;
            }
            else
            {
                transform.position = p;
            }
            return;
        }

       FullTransparency();

       transform.position = target.position - transform.forward * distFromTarget;
    }

    private void TransparencyCheck()
    {
        if (changeTransparency)
        {
            if(Vector3.Distance (transform.position, target.position) <= closestDistanceToPlayer)
            {
                Color temp = targetRenderer.sharedMaterial.color;
                temp.a = Mathf.Lerp(temp.a, 0.2f, collisionSpeed * Time.deltaTime);

                targetRenderer.sharedMaterial.color = temp;
            }
            else
            {
                if (targetRenderer.sharedMaterial.color.a <= 0.99f)
                {
                    Color temp = targetRenderer.sharedMaterial.color;
                    temp.a = Mathf.Lerp(temp.a, 1, collisionSpeed * Time.deltaTime);

                    targetRenderer.sharedMaterial.color = temp;
                }
            }
        }
    }

    private void FullTransparency()
    {
        if (changeTransparency)
        {
            if (targetRenderer.sharedMaterial.color.a <= 0.99f)
            {
                Color temp = targetRenderer.sharedMaterial.color;
                temp.a = Mathf.Lerp(temp.a, 1, collisionSpeed * Time.deltaTime);

                targetRenderer.sharedMaterial.color = temp;
            }
        }
    }
}