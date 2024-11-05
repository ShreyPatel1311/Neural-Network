using System.Collections.Generic;
using System.Text;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DoodleCNN : MonoBehaviour
{
    [Header("Neural Networks")]
    [SerializeField] private int[] num_units;
    [SerializeField] private string[] layer_activation;
    [SerializeField] private double learningRate = 0.01;

    [Header("Data")]
    [SerializeField] private TextAsset ImageBytes;
    [SerializeField] private TextAsset LabelBytes;

    [Header("Interactors")]
    [SerializeField] private TextMeshProUGUI[] labelTextMesh;
    [SerializeField] private RawImage doodleImage;

    private NNetwork cnn;
    private List<double[]> imageInputs;
    private List<double[]> labels;
    private TextMeshProUGUI[] labelsOriginal;

    private double[] getLabel(byte[] labelBytes, int labelIndex)
    {
        double[] labelArray = new double[10];
        labelArray[labelIndex] = 1;
        return labelArray;
    }

    private double[] getImage(byte[] imageBytes, int start)
    {
        double[] imageIntensity = new double[784];
        for (int i = 0; i < 784; i++)
        {
            imageIntensity[i] = imageBytes[start + i]/256;
        }

        return imageIntensity;
    }

    private void LoadData(byte[] imageBytes, byte[] labelBytes)
    {
        imageInputs = new List<double[]>(labelBytes.Length);
        labels = new List<double[]>(labelBytes.Length);

        for (int i = 0; i < labelBytes.Length; i++)
        {
            labels.Add(getLabel(labelBytes, labelBytes[i]));
            imageInputs.Add(getImage(imageBytes, i * 784));
        }
    }

    public void Train()
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            cnn.Train(imageInputs.ToArray(), labels.ToArray());
        });
    }

    public void Predict()
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            double[] output = cnn.Predict(doodleImage.texture as Texture2D);
            int i = 0;
            foreach(var label in labelTextMesh)
            {
                string newString = labelsOriginal[i].text + (output[i] * 100f).ToString();
                label.text = newString;
                i++;
            }
            Debug.Log(labelsOriginal[0].text);
            doodleImage.texture = new Texture2D(28, 28, TextureFormat.Alpha8, false);
        });
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Ensure the UnityMainThreadDispatcher is initialized
        UnityMainThreadDispatcher.Instance();
        labelsOriginal = labelTextMesh;

        byte[] imageBytes = ImageBytes.bytes;  
        byte[] labelBytes = LabelBytes.bytes;

        cnn = new NNetwork(learningRate);
        cnn.Add(num_units[1], layer_activation[0], num_units[0]);
        for (int i = 2; i < num_units.Length; i++)
        {
            cnn.Add(num_units[i], layer_activation[i - 1]);
        }

        // Start the data loading thread
        Thread dataLoadingThread = new Thread(() => LoadData(imageBytes, labelBytes));
        dataLoadingThread.Start();
    }
}
