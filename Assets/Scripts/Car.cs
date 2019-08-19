using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {

    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float rotationSpeed;
    public float distanceCovered;


    private bool initilized = false;
    [HideInInspector]
    public  bool crashed = false;  
    [HideInInspector]
    public float internalDistance;  
    [HideInInspector]
    public float distanceUpdateFrequency;


    private NeuralNet net;
    private Rigidbody2D rb;
    [HideInInspector]
    public Sensors sensors;
    private Vector2 prevLocation;
    private Vector2 trackerPrevLocation;
    
    
    void Start()
    {
        StartCoroutine(CarInit());
    }

    IEnumerator CarInit()
    {
        rb = GetComponent<Rigidbody2D>();
        sensors = gameObject.GetComponent<Sensors>();

        while (!initilized)
        {
            yield return null;
        }

        StartCoroutine(TrackFit(distanceUpdateFrequency));

        while (!crashed)
        {
            internalDistance += Vector2.Distance(transform.position, trackerPrevLocation);            
            trackerPrevLocation = transform.position;
            yield return StartCoroutine(MovementLoop());
        }
    }

    IEnumerator TrackFit(float delay)
    {
        yield return new WaitForSeconds(delay);
        distanceCovered += Vector2.Distance(prevLocation, transform.position);
        prevLocation = transform.position;
        net.SetFitness(distanceCovered);
        StartCoroutine(TrackFit(delay));
    }

    IEnumerator MovementLoop()
    {

        float[] inputs = new float[5];
        inputs[0] = sensors.distFront / sensors.raycastLengths;
        inputs[1] = sensors.distLeft / sensors.raycastLengths;
        inputs[2] = sensors.distRight / sensors.raycastLengths;
        inputs[3] = sensors.distFrontLeft / sensors.raycastLengths;
        inputs[4] = sensors.distFrontRight / sensors.raycastLengths;

        float[] output = net.FeedForward(inputs);
        
        //1 : right h
        //2 : left -h

        float h = 0;
        if (output[0] > output[1])
        {
            h = 1;
        }
        else if (output[0] < output[1])
        {
            h = -1;
        }
        
        MoveCar(h);
        yield return new WaitForFixedUpdate();
    }

    public void Init(NeuralNet net, float updateFrequency, float speed, float rotation, float raycastLengths)
    {
        this.net = net;
        initilized = true;
        prevLocation = transform.position;
        distanceCovered = 0;
        distanceUpdateFrequency = updateFrequency;
        maxSpeed = speed;
        rotationSpeed = rotation;
        sensors.raycastLengths = raycastLengths;
    }     

    void MoveCar(float h)
    {

        float speed = maxSpeed;
        rb.velocity = transform.up * speed;

        rb.rotation += h * rotationSpeed;

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    

    public NeuralNet GetNet()
    {
        return net;
    }
}
