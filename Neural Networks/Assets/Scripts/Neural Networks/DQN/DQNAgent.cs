using UnityEngine;

public class DQNAgent : MonoBehaviour
{
    public GameState gameState;
    private NNetwork model;
    [SerializeField] private int[] num_units;
    [SerializeField] private string[] layer_activation;
    [SerializeField] private double learningRate = 0.001;

    public void PredictAction(int state)
    {

    }

    private void Start()
    {
        model = new NNetwork(learningRate);
        model.Add(num_units[1], layer_activation[0], num_units[0]);
        for (int i = 2; i < num_units.Length; i++)
        {
            model.Add(num_units[i], layer_activation[i-1]);
        }
        double[][] train_inputs = new double[][] { new double[]{ 1, 0, 0, 1, 1 } };
        double[][] train_outputs = new double[][] { new double[] { 0, 1 } };
        model.Train(train_inputs, train_outputs);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
