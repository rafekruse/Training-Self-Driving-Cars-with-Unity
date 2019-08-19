using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public bool hide;
    public float maxSpeed;
    public float rotationSpeed;
    public float sensorLengths;
    public bool drawSensors;
    public float mutationRate;
    public float populationDirectParentPortion;

    public GameObject carPrefab;
    public Material aliveMat;
    public Material crashedMat;
    public Material leadMat;

    public float generationTime;
    private float timePassed;

    public float obstacleMoveSpeed;
    public GameObject[] movingObstacles;
    public float timeScale;

    public int populationSize;
    public int generationNumber = 0;
    public float fitnessUpdateFrequency;
    public int[] layers = new int[] { 5, 8, 8, 2 };
    [HideInInspector]
    public Car currentFittest;
    public float delayBetweenGenerations;
    public string carSaveName;
    public string carLoadName;
    private List<NeuralNet> nets;
    private List<Car> cars = null;

    [HideInInspector]

    private void Awake()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Car"), LayerMask.NameToLayer("Car"));
        cars = new List<Car>();
        
    }

    private void Update()
    {
        Time.timeScale = timeScale;
        layers[layers.Length - 1] = 2;
        layers[0] = 5;
    }

    public void RunPopulation()
    {
        RunPopulation(new NeuralNet(layers));
    }

    public void RunPopulation(NeuralNet net)
    {
        if (generationNumber > 0)
        {
            generationNumber = 0;
            StopAllCoroutines();
            currentFittest = (Instantiate(carPrefab, gameObject.transform.position, gameObject.transform.rotation).GetComponent<Car>());
            currentFittest.Init(net, fitnessUpdateFrequency, maxSpeed, rotationSpeed, sensorLengths);
            cars.Add(currentFittest);
        }
        InitCarNeuralNets(net);
        StartCoroutine(SimulationLoop());
    }

    IEnumerator SimulationLoop()
    {
        while (true)
        {
            StartCoroutine(Init());

            yield return StartCoroutine(Training());
        }
    }

    IEnumerator Init()
    {
        ResetMovingObstacles();

        nets.Sort();
        nets[0] = new NeuralNet(currentFittest.GetNet());
        for (int i = 1; i < populationSize; i++)
        {
            if (i < populationDirectParentPortion * populationSize)
            {
                nets[i] = new NeuralNet(currentFittest.GetNet());
                nets[i].Mutate(mutationRate);
            }
            else
            {
                nets[i] = new NeuralNet(layers);
                nets[i].Mutate(mutationRate);
            }
            nets[i].SetFitness(0f);
        }
        Destroy(currentFittest);

        CreateCars();
        generationNumber++;
        Debug.Log("Training for Generation " + generationNumber + " Started.");
        yield break;
    }

    IEnumerator Training()
    {
        while (StillAlive() && timePassed < generationTime)
        {
            timePassed += Time.deltaTime;

            UpdateStatus();

            yield return new WaitForFixedUpdate();
        }
        Debug.Log("Training ended for Generation " + generationNumber + " after " + timePassed + " seconds");
        timePassed = 0;
        yield return new WaitForSecondsRealtime(delayBetweenGenerations);
    }

    void InitCarNeuralNets(NeuralNet net)
    {
        nets = new List<NeuralNet>();

        for (int i = 0; i < populationSize; i++)
        {
            NeuralNet network = new NeuralNet(net);
            nets.Add(network);
        }
    }


    private bool StillAlive()
    {
        for (int i = 0; i < cars.Count; i++)
        {
            if (!cars[i].crashed)
            {
                return true;
            }
        }
        return false;
    }

    private void UpdateStatus()
    {
        float highestDist = 0;
        int index = 0;
        for (int i = 0; i < cars.Count; i++)
        {
            cars[i].sensors.drawRaycasts = drawSensors;
            SpriteRenderer sr = cars[i].GetComponent<SpriteRenderer>();

            sr.material = cars[i].crashed ? crashedMat : aliveMat;
            if (hide && cars[i].crashed)
            {
                sr.enabled = false;
            }

            if (cars[i].internalDistance > highestDist)
            {
                highestDist = cars[i].internalDistance;
                index = i;
            }
        }
        currentFittest = cars[index];
        cars[index].GetComponent<SpriteRenderer>().material = leadMat;
    }

    private void CreateCars()
    {
        if (cars != null)
        {
            for (int i = 0; i < cars.Count; i++)
            {
                GameObject.Destroy(cars[i].gameObject);
            }
        }
        cars = new List<Car>();

        for (int i = 0; i < populationSize; i++)
        {
            Car car = (Instantiate(carPrefab, gameObject.transform.position, gameObject.transform.rotation).GetComponent<Car>());
            car.Init(nets[i], fitnessUpdateFrequency, maxSpeed, rotationSpeed, sensorLengths);
            cars.Add(car);
        }
    }

    void ResetMovingObstacles()
    {
        for (int i = 0; i < movingObstacles.Length; i++)
        {
            movingObstacles[i].transform.position = movingObstacles[i].GetComponent<MoveBoundary>().wayPoints[0];
            movingObstacles[i].GetComponent<MoveBoundary>().nextWaypoint = 1;
            movingObstacles[i].GetComponent<MoveBoundary>().moveSpeed = obstacleMoveSpeed;
        }
        
    }

    public void RunSoloCar(NeuralNet nn)
    {
        StopAllCoroutines();
        ResetMovingObstacles();
        if (cars != null)
        {
            for (int i = 0; i < cars.Count; i++)
            {
                GameObject.Destroy(cars[i].gameObject);
            }
        }
        cars = new List<Car>();
        Car car = (Instantiate(carPrefab, gameObject.transform.position, gameObject.transform.rotation).GetComponent<Car>());
        car.Init(nn, fitnessUpdateFrequency, maxSpeed, rotationSpeed, sensorLengths);
        cars.Add(car);
        currentFittest = car;
    }
}
