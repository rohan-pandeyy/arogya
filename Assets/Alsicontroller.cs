using UnityEngine;
using System.IO.Ports;

public class SerialReader : MonoBehaviour
{
    public string portName = "COM3";
    public int baudRate = 115200;
    public GameObject cube;
    public float rotationSpeed = 20f;

    private SerialPort serialPort;
    private float[] sensorData = new float[6];

    void Start()
    {
        serialPort = new SerialPort(portName, baudRate);
        serialPort.Open();
        serialPort.ReadTimeout = 100;
    }

    void Update()
    {
        if (serialPort.IsOpen)
        {
            try
            {
                string data = serialPort.ReadLine();
                ParseSensorData(data);
                RotateCube();
            }
            catch (System.Exception)
            {
                // Optional: Debug.LogWarning("Serial read error");
            }
        }
    }

    void ParseSensorData(string data)
    {
        string[] values = data.Split(',');

        if (values.Length == 6)
        {
            for (int i = 0; i < 6; i++)
            {
                sensorData[i] = float.Parse(values[i]);
            }
        }
    }

    void RotateCube()
    {
        float rotateX = sensorData[3] * Time.deltaTime * rotationSpeed;
        float rotateY = sensorData[4] * Time.deltaTime * rotationSpeed;
        float rotateZ = sensorData[5] * Time.deltaTime * rotationSpeed;

        cube.transform.Rotate(rotateX, rotateY, rotateZ, Space.Self);
    }

    void OnDestroy()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }
}
