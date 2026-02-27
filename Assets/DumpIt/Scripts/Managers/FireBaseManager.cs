using UnityEngine;
using Firebase;
using Firebase.Analytics;
using Firebase.Crashlytics;
using System;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance;
    public static string SessionId { get; private set; }

    bool firebaseReady = false;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SessionId = Guid.NewGuid().ToString();
        InitializeFirebase();
    }

    void InitializeFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
                Crashlytics.IsCrashlyticsCollectionEnabled = true;

                firebaseReady = true;

                LogSessionStart();

                //Debug.Log("[Firebase] Initialized | Session: " + SessionId);
            }
            else
            {
                Debug.LogError("[Firebase] Dependency error: " + task.Result);
            }
        });
    }

    void LogSessionStart()
    {
        if (!firebaseReady) return;

        FirebaseAnalytics.LogEvent(
            "session_start",
            new Parameter("session_id", SessionId)
        );
    }

    public void LogLevelStart(int level)
    {
        if (!firebaseReady) return;

        FirebaseAnalytics.LogEvent(
            "level_start",
            new Parameter("level", level),
            new Parameter("session_id", SessionId)
        );
        Crashlytics.SetCustomKey("level", level.ToString());
        Crashlytics.SetCustomKey("session_id", SessionId);
    }

    public void LogLevelFail(int level, int attempt)
    {
        if (!firebaseReady) return;

        FirebaseAnalytics.LogEvent(
            "level_fail",
            new Parameter("level", level),
            new Parameter("attempt", attempt),
            new Parameter("session_id", SessionId)
        );
    }

    public void LogLevelComplete(int level, int attempt)
    {
        if (!firebaseReady) return;

        FirebaseAnalytics.LogEvent(
            "level_complete",
            new Parameter("level", level),
            new Parameter("attempt", attempt),
            new Parameter("session_id", SessionId)
        );
    }
}
