using UnityEngine;
using UnityEngine.UI;

public class RotationDisplay : MonoBehaviour
{
    public GameObject targetObject;      // The object to track
    public Text rotationText;            // UI Text to show rotation

    private Quaternion initialRotation;

    void Start()
    {
        if (targetObject != null)
        {
            // Store the initial rotation
            initialRotation = targetObject.transform.rotation;
        }
    }

    void Update()
    {
        if (targetObject != null && rotationText != null)
        {
            // Calculate relative rotation from the initial state
            Quaternion relativeRotation = Quaternion.Inverse(initialRotation) * targetObject.transform.rotation;
            Vector3 euler = relativeRotation.eulerAngles;

            // Display the relative rotation in degrees
            rotationText.text = $"X: {euler.x:F1}° Y: {euler.y:F1}° Z: {euler.z:F1}°";
        }
    }
}
