using System.Linq;
using System;
using UnityEngine;

public class Layers : MonoBehaviour 
{
    private Perceptron[] perceptron;
    private string activationFunction;

    public int num_perceptrons;

    public Perceptron[] Perceptron { get => perceptron; set => perceptron = value; }

    public Layers(int num_perceptrons, string activation_function, int prev_layer_units = 0)
    {
        activationFunction = activation_function;
        this.num_perceptrons = num_perceptrons;
        Perceptron = new Perceptron[num_perceptrons];
        if(prev_layer_units != 0)
        {
            for(int i=0; i < Perceptron.Length; i++)
            {
                Perceptron[i] = new Perceptron(prev_layer_units);
            }
        }
        else
        {
            for (int i = 0; i < Perceptron.Length; i++)
            {
                Perceptron[i] = new Perceptron(1);
            }
        }
    }

    public double[] ForwardPass(double[] Inputs, int index)
    {
        double[] output = new double[num_perceptrons];
        if (perceptron[0].Weight.Length == 1)
        {
            for (int i = 0; i < Perceptron.Length; i++)
            {
                double[] Input = new double[1] { Inputs[i] };
                output[i] = Perceptron[i].generateOutput(Input, activationFunction, index);
            }
        }
        else
        {
            for (int i = 0; i < Perceptron.Length; i++)
            {
                output[i] = Perceptron[i].generateOutput(Inputs, activationFunction, index);
            }
        }
        return ApplyActivationFunction(output);
    }

    private void PrintArray(double[] array)
    {
        array = ApplyActivationFunction(array);
        for (int i = 0; i < array.Length; i++)
        {
            Debug.Log(array[i]);
        }
    }

    public double[] BackPropagate(double[] loss, double learningRate)
    {
        double[] derivativeValue = CalculateDerivate(loss);
        foreach(Perceptron perceptron in Perceptron)
        {
            perceptron.UpdateWeightsAndBiases(derivativeValue, derivativeValue.Sum()/derivativeValue.Length, learningRate);
        }
        return derivativeValue;
    }

    private double[] ApplyActivationFunction(double[] input)
    {
        double[] output = new double[input.Length];
        double e = Math.E;
        switch (activationFunction)
        {
            case "Sigmoid":
                for (int i = 0; i < input.Length; i++)
                {
                    output[i] = 1 / (1 + Math.Pow(e, input[i]));
                }
                return output;
            case "Relu":
                for (int i=0; i<input.Length; i++)
                {
                    output[i] = Math.Max(0, input[i]);
                }
                return output;
            case "Tanh":
                for (int i=0; i<input.Length; i++)
                {
                    output[i] = 2 * (1 / (1 + Math.Exp(-input[i]))) - 1;
                }
                return output;
            case "Swiss":
                for (int i = 0; i < input.Length; i++)
                {
                    output[i] = input[i] * (1 / (1 + Math.Exp(-input[i])));
                }
                return output;
            case "Softmax":
                double[] exp = new double[input.Length];
                for (int i = 0; i < input.Length; i++)
                {
                    exp[i] = Math.Exp(input[i]);
                }
                for (int i = 0; i < input.Length; i++)
                {
                    output[i] = Math.Exp(input[i]) / exp.Sum();
                }
                return output;
            default:
                return input;
        }
    }

    private double[] CalculateDerivate(double[] input)
    {
        double e = Math.E;
        double[] derivatives = new double[input.Length];
        switch (activationFunction)
        {
            case "Sigmoid":
                for (int i = 0; i < input.Length; i++)
                {
                    derivatives[i] = (1 / (1 + Math.Pow(e, input[i]))) * (1 - (1 / (1 + Math.Pow(e, input[i]))));
                }
                return derivatives;
            case "Relu":
                for (int i = 0; i < input.Length; i++)
                {
                    derivatives[i] = (input[i] > 0) ? 1 : 0;
                }
                return derivatives;
            case "Tanh":
                for (int i = 0; i < input.Length; i++)
                {
                    derivatives[i] = 1 - Math.Pow(2 * (1 / (1 + Math.Exp(-input[i]))) - 1, 2);
                }
                return derivatives;
            case "Swiss":
                for (int i = 0; i < input.Length; i++)
                {
                    derivatives[i] = (1 / (1 + Math.Pow(e, input[i]))) * (input[i] * (1 - (1 / (1 + Math.Pow(e, input[i])))) + 1);
                }
                return derivatives;
            case "Softmax":
                double exp = 0;
                for (int i = 0; i < input.Length; i++)
                {
                    exp += Math.Exp(input[i]);
                }
                for (int i = 0; i < input.Length; i++)
                {
                    for (int j = 0; j < input.Length; j++)
                    {
                        derivatives[i] += (Math.Exp(input[i]) / exp) * (i == j ? 1 - (Math.Exp(input[i]) / exp) : Math.Exp(input[j]) / exp);
                    }
                }
                return derivatives;
            default:
                for (int i = 0; i < input.Length; i++)
                {
                    derivatives[i] = 1;
                }
                return derivatives;
        }
    }

    public override string ToString()
    {
        string mainString = "";
        foreach(Perceptron unit in Perceptron)
        {
            mainString += unit.ToString();
        }
        return "No. of Units : " + num_perceptrons + "\nActivation Function : " + activationFunction + "\n" + mainString;
    }
}
