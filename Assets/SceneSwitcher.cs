using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public void GoToSignUpScene()
    {
        SceneManager.LoadScene("SignUpScene");
    }

    public void GoToSignInScene()
    {
        SceneManager.LoadScene("SignInScene");
    }
}
