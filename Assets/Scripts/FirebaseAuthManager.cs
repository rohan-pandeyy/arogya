using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;  // For UI Elements

public class FirebaseAuthManager : MonoBehaviour
{
    private FirebaseAuth auth;
    private FirebaseUser user;

    public TMP_InputField emailInput;  // Assign in Unity Inspector
    public TMP_InputField passwordInput;  // Assign in Unity Inspector

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
            }
            else
            {
                Debug.LogError("Firebase dependencies are not available.");
            }
        });
    }

    public void SignUpUser()
    {
        if (auth == null)
        {
            Debug.LogError("Firebase Auth is not initialized yet!");
            return;
        }

        string email = emailInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            Debug.LogWarning("Please enter email and password.");
            return;
        }

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("Signup Failed: " + task.Exception?.Message);
                return;
            }

            AuthResult result = task.Result;  // Get AuthResult
            user = result.User;  // Get FirebaseUser from AuthResult

            Debug.Log("Signup Successful! Welcome " + user.Email);
        });
    }
}
