using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBoundary : MonoBehaviour {

    public Vector2[] wayPoints;
    public float moveSpeed;
    public int nextWaypoint;

    private void Start()
    {
        transform.position = wayPoints[0];
        nextWaypoint = 1;
    }

    private void FixedUpdate()
    {
        Vector2 directionToNext = (wayPoints[nextWaypoint] - (Vector2)transform.position).normalized;
        transform.position = (Vector2)transform.position + (directionToNext * moveSpeed * Time.deltaTime);
        if (Vector2.Distance((Vector2)transform.position, wayPoints[nextWaypoint]) < 0.05f)
        {
            nextWaypoint++;
            if(nextWaypoint > wayPoints.Length - 1)
            {
                nextWaypoint = 0;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < wayPoints.Length; i++)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(wayPoints[i], 0.25f);
        }
    }

}
