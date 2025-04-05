using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class FirebaseAuthManager : MonoBehaviour
{
    private FirebaseAuth auth;
    private FirebaseUser user;
    private DatabaseReference dbReference;
    private DependencyStatus dependencyStatus;
    private bool firebaseInitialized = false;

    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;

                auth = FirebaseAuth.DefaultInstance;
                dbReference = FirebaseDatabase.GetInstance("https://arogya-4327f-default-rtdb.firebaseio.com/").RootReference;
                firebaseInitialized = true;

                Debug.Log("Firebase Initialized Successfully!");
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    public async void SignUpUser()
    {
        if (!firebaseInitialized || auth == null)
        {
            Debug.LogError("Firebase is not initialized yet!");
            return;
        }

        string email = emailInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            Debug.LogWarning("Please enter email and password.");
            return;
        }

        try
        {
            var authResult = await auth.CreateUserWithEmailAndPasswordAsync(email, password);
            user = authResult.User;

            Debug.Log("Signup Successful for: " + user.Email);

            // ✅ Scene switch directly after success
            SceneManager.LoadScene("SignInScene");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Signup Failed: " + ex.Message);
        }
    }
}
