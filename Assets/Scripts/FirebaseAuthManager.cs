using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using System.Threading.Tasks; // 🔧 Required for Task<T>

public class FirebaseAuthManager : MonoBehaviour
{
    private FirebaseAuth auth;
    private FirebaseUser user;
    private DatabaseReference dbReference;
    private DependencyStatus dependencyStatus;
    private bool firebaseInitialized = false;

    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_InputField firstNameInput;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;

                // ✅ Set the Realtime Database URL
                //app.Options.DatabaseUrl = new System.Uri("https://arogya-4327f-default-rtdb.firebaseio.com/");

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

    public void SignUpUser()
    {
        if (!firebaseInitialized || auth == null)
        {
            Debug.LogError("Firebase is not initialized yet!");
            return;
        }

        string email = emailInput.text;
        string password = passwordInput.text;
        string firstName = firstNameInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(firstName))
        {
            Debug.LogWarning("Please enter email, password, and first name.");
            return;
        }

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("Signup Failed: " + task.Exception?.Flatten().InnerException?.Message);
                return;
            }

            var authResultTask = task as Task<Firebase.Auth.AuthResult>;
            if (authResultTask == null)
            {
                Debug.LogError("AuthResult task is null.");
                return;
            }

            Firebase.Auth.AuthResult authResult = authResultTask.Result;
            user = authResult.User;

            Debug.Log("Signup Successful! Storing name...");

            dbReference.Child("users").Child(user.UserId).Child("firstName").SetValueAsync(firstName).ContinueWith(setTask =>
            {
                if (setTask.IsCompletedSuccessfully)
                {
                    Debug.Log("First name stored successfully for " + user.Email);
                }
                else
                {
                    Debug.LogError("Failed to store first name: " + setTask.Exception?.Flatten().InnerException?.Message);
                }
            });
        });
    }
}
