using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // This method can be linked to a button
    public void LoadMapScene()
    {
        Debug.Log("Knee button clicked. Loading Map scene...");
        SceneManager.LoadScene("Map");
    }
    public void LoadShoulderScene()
    {
        Debug.Log("Shoulder button clicked. Loading Map scene...");
        SceneManager.LoadScene("Shoulder");
    }
}
