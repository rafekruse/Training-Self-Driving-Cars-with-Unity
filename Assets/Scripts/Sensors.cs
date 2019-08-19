using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensors : MonoBehaviour
{

    [SerializeField]
    private LayerMask LayerToSense;

    public float raycastLengths;
    public bool drawRaycasts;

    public RaycastHit2D hitLeft, hitRight, hitFront, hitFrontRight, hitFrontLeft;
    [HideInInspector]
    public float distLeft = 0, distRight = 0, distFront = 0, distFrontRight = 0, distFrontLeft = 0;

    private Vector2 dirLeft, dirRight, dirFront, dirFrontRight, dirFrontLeft;



    void Update()
    {
        SetDirections();

        //Normalizes values to 0-1
        distLeft = CastRay(dirLeft, hitLeft) / raycastLengths;
        distRight = CastRay(dirRight, hitRight) / raycastLengths;
        distFront = CastRay(dirFront, hitFront) / raycastLengths;
        distFrontLeft = CastRay(dirFrontLeft, hitFrontLeft) / raycastLengths;
        distFrontRight = CastRay(dirFrontRight, hitFrontRight) / raycastLengths;
    }


    void SetDirections()
    {
        dirLeft = -transform.right * raycastLengths;
        dirRight = transform.right * raycastLengths;
        dirFront = transform.up * raycastLengths;
        dirFrontRight = (transform.up + transform.right) * raycastLengths * Mathf.Sqrt(2) / 2;
        dirFrontLeft = (transform.up - transform.right) * raycastLengths * Mathf.Sqrt(2) / 2;
    }
    float CastRay(Vector2 dir, RaycastHit2D hit)
    {

        hit = Physics2D.Raycast(transform.position, dir, raycastLengths, LayerToSense);

        Color raycastColor = Color.white;

        if (hit.collider == null)
            hit.distance = raycastLengths;
        else
            raycastColor = Color.red;

        if (hit.distance < .01f)
            hit.distance = .01f;

        if (drawRaycasts)
            Debug.DrawLine(transform.position, new Vector2(transform.position.x + dir.x * raycastLengths, transform.position.y + dir.y * raycastLengths), raycastColor);


        return hit.distance;
    }
}
