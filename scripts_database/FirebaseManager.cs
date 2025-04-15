using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseAuth auth;
    public static FirebaseUser user;
    public static FirebaseFirestore db;

    // Start is now marked async, and InitializeFirebase is awaited properly
    async void Start()
    {
        await InitializeFirebase();
    }

    // Initialize Firebase and await the async method
    private async Task InitializeFirebase()
    {
        // Ensure Firebase dependencies are checked asynchronously
        var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
        
        if (dependencyStatus != DependencyStatus.Available)
        {
            Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            return;
        }

        // Initialize Firebase services
        FirebaseApp app = FirebaseApp.DefaultInstance;
        auth = FirebaseAuth.DefaultInstance;
        db = FirebaseFirestore.DefaultInstance;

        Debug.Log("Firebase Initialized!");
    }
}
