using Firebase;
using Firebase.Database;
using UnityEngine;

public class DatabaseManager: MonoBehaviour
{
    void start()
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
    }
}
