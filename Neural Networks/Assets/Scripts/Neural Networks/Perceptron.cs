using UnityEngine;
using Random = System.Random;

public class Perceptron : MonoBehaviour 
{
    private double[] weight;
    private double bias;
    public double[] Weight { get => weight; }

    public Perceptron(int input_size)
    {
        weight = new double[input_size];
        Random random = new Random();
        for (int i = 0; i < weight.Length; i++)
        {
            weight[i] = random.NextDouble() * 2.0 - 1.0;
        }
        bias = random.NextDouble() * 2.0 - 1.0;
    }

    public double generateOutput(double[] input, string activationFunction, int index)
    {
        double sum = 0;
        Debug.Log("Input Size : "+input.Length + "Weight : " + weight.Length + "Acti : " + activationFunction + "Index : " + index);
        for(int i=0; i<weight.Length; i++)
        {
            sum += input[i] * weight[i];
        }
        return sum + bias;
    }

    public void UpdateWeightsAndBiases(double[] dw, double db, double alpha)
    {
        for(int i=0;i<weight.Length;i++)
        {
            weight[i] -= alpha * dw[i];
        }
        bias -= alpha * db;
    }

    public override string ToString()
    {
        string weightS = "";
        foreach(var w in weight)
        {
            weightS += w.ToString() + ",";
        }
        return "Input Size : " + Weight.Length + "\n" + "Weights : " + weightS + "\n" + "Bias : " + bias + "\n";
    }
}
