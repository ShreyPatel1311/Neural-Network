using System;
using System.Collections.Generic;
using UnityEngine;

public class NNetwork : MonoBehaviour
{
    private List<Layers> layers = new List<Layers>();
    private int max_units;
    private double alpha;

    public NNetwork(double alpha = 0.001) 
    {
        this.alpha = alpha;
    }

    public void Add(int num_units, string activation_function, int input_shape = 0)
    {
        if(input_shape != 0)
        {
            Layers inputLayer = new Layers(input_shape, "None");
            max_units = input_shape;
            layers.Add(inputLayer);
        }
        Layers layer = new Layers(num_units, activation_function, layers[layers.Count - 1].num_perceptrons);
        if(num_units > max_units)
        {
            max_units = num_units;
        }
        layers.Add(layer);
    }

    public void Compile()
    {
        
    }

    public List<double[]> Train(double[][] x_train, double[][] y_train) 
    {
        if(x_train.Length != y_train.Length)
        {
            Debug.LogError("Length of Inputs are not equal to Length of Outputs");
            return null;
        }
        List<double[]> forwardOutput = new List<double[]>
        {
            layers[0].ForwardPass(x_train[0], 0)
        };
        //foreach (var layer in layers)
        //{
        //    Debug.Log(layer.ToString());
        //}
        double[] loss = new double[max_units];
        double[] derivativeValue = new double[max_units];
        int count = 1;
        try
        {
            for (int i = 0; i < x_train.Length; i++)
            {
                for (int j = 1; j < layers.Count; j++)
                {
                    forwardOutput.Add(layers[j].ForwardPass(forwardOutput[forwardOutput.Count - 1], j));
                    count++;
                }
                double[] finalOutput = forwardOutput[forwardOutput.Count - 1];
                for (int k = 0; k < y_train[y_train.Length - 1].Length; k++)
                {
                    loss[i] = DotProduct(finalOutput, y_train[i]);
                }
                for (int l = 0; l < layers.Count; l++)
                {
                    if (l == 0)
                    {
                        derivativeValue = layers[layers.Count - l - 1].BackPropagate(loss, alpha);
                    }
                    else
                    {
                        derivativeValue = layers[layers.Count - l - 1].BackPropagate(derivativeValue, alpha);
                    }
                }
            }
        }
        catch (IndexOutOfRangeException)
        {
            Debug.Log("Normal Error, Index Out of Range. Unity Smoked up a lot!!!!");
        }
        //foreach (var layer in layers)
        //{
        //    Debug.Log(layer.ToString());
        //}
        //foreach (var output in forwardOutput)
        //{
        //    string outputS = "";
        //    foreach (var value in output)
        //    {
        //        outputS += value.ToString() + ",";
        //    }
        //    Debug.Log(outputS);
        //}
        return forwardOutput;
    }

    public double[] Predict(Texture2D image)
    {
        byte[] inputBytes = image.GetRawTextureData();
        double[] inputs = new double[inputBytes.Length];
        double[] output = new double[10];
        for (int i = 0; i < inputs.Length; i++)
        {
            inputs[i] = inputBytes[i];
        }

        List<double[]> forwardOutput = new List<double[]>
        {
            layers[0].ForwardPass(inputs, 0)
        };
        try
        {
            for (int j = 1; j < layers.Count; j++)
            {
                forwardOutput.Add(layers[j].ForwardPass(forwardOutput[forwardOutput.Count - 1], j));
            }
            output = forwardOutput[forwardOutput.Count - 1];
        }
        catch(IndexOutOfRangeException)
        {
            Debug.Log("Unity is High till Heaven");
        }
        return output;
    }

    private void PrintArray(double[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            Debug.Log(array[i]);
        }
    }

    private double DotProduct(double[] forwardOutput, double[] outputs)
    {
        double dot = 0;
        for (int i = 0; i<outputs.Length; i++)
        {
            dot = outputs[i] - forwardOutput[i];
        }
        return dot;
    }
}