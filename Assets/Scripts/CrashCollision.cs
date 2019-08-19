using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashCollision : MonoBehaviour {


    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Boundary"))
        {
            gameObject.GetComponent<Car>().crashed = true;
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            gameObject.GetComponent<Sensors>().enabled = false;
        }
    }
}
