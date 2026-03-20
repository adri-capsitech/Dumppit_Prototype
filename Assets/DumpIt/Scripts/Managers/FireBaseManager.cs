using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    public static bool IsFirebaseReady { get; private set; }

    private static bool initialized = false;
    [SerializeField] private GameObject versionChecker;

    private void Awake()
    {
        if (initialized)
        {
            Destroy(gameObject);
            return;
        }

        initialized = true;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.Result == DependencyStatus.Available)
                {
                    FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
                    IsFirebaseReady = true;
                    versionChecker.SetActive(true);
                    Debug.Log(" Firebase initialized");
                }
                else
                {
                    Debug.LogError(" Firebase dependency error: " + task.Result);
                }
            });
    }
}
