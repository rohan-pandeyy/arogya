using UnityEngine;
using System.IO.Ports; // Ensure you have a compatible serial library

public class SerialReader : MonoBehaviour
{
    public string portName = "COM3"; // Change to your Arduino's COM port
    public int baudRate = 115200;
    public GameObject cube; // Reference to the cube in the scene
    public float rotationSpeed = 20f; // Scaling factor for rotation speed

    private SerialPort serialPort;
    private float[] sensorData = new float[6]; // Array to store sensor data

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
                // Handle errors
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
        // Use gyroscope data to rotate the cube
        float rotateX = sensorData[3] * Time.deltaTime * rotationSpeed; // Gyro X (pitch)
        float rotateY = sensorData[4] * Time.deltaTime * rotationSpeed; // Gyro Y (yaw)
        float rotateZ = sensorData[5] * Time.deltaTime * rotationSpeed; // Gyro Z (roll)

        // Apply rotation to the cube
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