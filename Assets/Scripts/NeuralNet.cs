using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


[Serializable]
public class NeuralNet : IComparable<NeuralNet>{

    [SerializeField]
    private int[] layers;
    [SerializeField]
    private float[][] neurons;
    [SerializeField]
    private float[][][] weights;

    private float fitness;
    
    public NeuralNet() { }

    public NeuralNet(int[] layers)
    {
        this.layers = new int[layers.Length];
        for(int i = 0; i < layers.Length; i++)
        {
            this.layers[i] = layers[i];
        }
        
        InitNeurons();
        InitWeights();

    }

    public NeuralNet(NeuralNet copyNetwork)
    {
        this.layers = new int[copyNetwork.layers.Length];
        for (int i = 0; i < copyNetwork.layers.Length; i++)
        {
            this.layers[i] = copyNetwork.layers[i];
        }

        InitNeurons();
        InitWeights();
        CopyWeights(copyNetwork.weights);

    }

    private void CopyWeights(float[][][] copyWeights)
    {
        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    weights[i][j][k] = copyWeights[i][j][k];
                }
            }
        }
    }

    private void InitNeurons()
    {
        List<float[]> neuronsList = new List<float[]>();

        for(int i = 0; i < layers.Length; i++)
        {
            neuronsList.Add(new float[layers[i]]);
        }

        neurons = neuronsList.ToArray();
    }
    private void InitWeights()
    {
        List<float[][]> weightsList = new List<float[][]>();

        for(int i = 1; i < layers.Length; i++)
        {
            List<float[]> layerWeightsList = new List<float[]>();

            int neuronsInPreviousLayer = layers[i - 1];

            for (int j = 0; j < neurons[i].Length; j++)
            {
                float[] neuronWeights = new float[neuronsInPreviousLayer];

                for (int k = 0; k < neuronsInPreviousLayer; k++)
                {
                    neuronWeights[k] = UnityEngine.Random.Range(-1f, 1f);  
                }

                layerWeightsList.Add(neuronWeights);
            }

            weightsList.Add(layerWeightsList.ToArray());
        }

        weights = weightsList.ToArray();
    }

    public float[] FeedForward(float[] inputs)
    {
        for (int i = 0; i < inputs.Length; i++)
        {
            neurons[0][i] = inputs[i];
        }
        for (int i = 1; i < layers.Length; i++)
        {
            for (int j = 0; j < neurons[i].Length; j++)
            {
                float value = 0;

                for (int k = 0; k < neurons[i - 1].Length; k++)
                {
                    value += weights[i - 1][j][k] * neurons[i - 1][k];
                }
                
                neurons[i][j] = (float)Math.Tanh(value);
            }
        }

        return neurons[neurons.Length - 1];
    }

    public void Mutate(float mutationRate)
    {
        for(int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    float weight = weights[i][j][k];

                    float randomNumber = UnityEngine.Random.Range(0f, 3 / mutationRate);
                    if (randomNumber <= 1f)
                    {
                        weight *= -1f;
                    }
                    else if (randomNumber <= 2f)
                    {
                        weight = UnityEngine.Random.Range(-1f, 1f);
                    }
                    else if (randomNumber <= 3f)
                    {
                        float factor = UnityEngine.Random.Range(0.5f, 2f);
                        weight *= factor;
                    }

                    weights[i][j][k] = weight;
                }
            }
        }
    }
    public void AddFitness(float fit)
    {
        fitness += fit;
    }

    public void SetFitness(float fit)
    {
        fitness = fit;
    }

    public float GetFitness()
    {
        return fitness;
    }
    public int CompareTo(NeuralNet other)
    {
        if (other == null) return 1;

        if (fitness > other.fitness)
            return 1;
        else if (fitness < other.fitness)
            return -1;
        else
            return 0;
    }
    public void OutputNeurons()
    {
        for(int i = 0; i < neurons.Length; i++)
        {
            String output = "";
            for(int j = 0; j < neurons[i].Length; j++)
            {
                output += neurons[i][j] + "  ";
            }
            Debug.Log(output);
        }
    }

    public static void SaveNN(NeuralNet net,string path)
    {

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(path);
        bf.Serialize(file, net);
        file.Close();

        Debug.Log("Neural Network saved at path : " + path);
    }
    public static NeuralNet LoadNN(string path)
    {
        NeuralNet output = new NeuralNet();
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            output = new NeuralNet((NeuralNet)bf.Deserialize(file));
            file.Close();
        }
        return output;
    }
}
