using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;
using System;
using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class VersionChecker : MonoBehaviour
{

    FirebaseFirestore db;

    public string currentVersion;


    private void Awake()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                db = FirebaseFirestore.DefaultInstance;
                Debug.Log("Firestore Ready");
                CheckVersion();
            }
            else
            {
                Debug.LogError("Firebase not ready");
            }
        });
    }
    public void CheckVersion()
    {
         UpdatePanelScript.instance.setLoading(false);
         currentVersion = Application.version;

        StartCoroutine(SafeFirestoreCall<DocumentSnapshot>(
            () => db.Collection("AppConfig").Document("version").GetSnapshotAsync(),
            snapshot =>
            {
                UpdatePanelScript.instance.setLoading(false);

                if (snapshot.Exists)
                {
                    string latestVersion;
                   #if UNITY_ANDROID
                        latestVersion = snapshot.GetValue<string>("latestVersion");
                        string androidlink = snapshot.GetValue<string>("AndroidLink");
                        UpdatePanelScript.instance.playStoreUrl = androidlink;
                   #elif UNITY_IOS
                        latestVersion = snapshot.GetValue<string>("latestVersionIos");
                        string iosLink = snapshot.GetValue<string>("IosLink");
                        UpdatePanelScript.instance.appStoreUrl = iosLink;
                     #endif
                    bool forceUpdate = snapshot.GetValue<bool>("forceUpdate");

                    CompareVersions(currentVersion, latestVersion, forceUpdate);
                }
                else
                {
                    AndroidToast.ShowToast("Can`t fetch the latest version");
                    GoToHome();
                }
            },

            () =>
            {
                UpdatePanelScript.instance.setLoading(false);
                AndroidToast.ShowToast("Can`t fetch the latest version");
                GoToHome();
            }
        ));

    }

    void CompareVersions(string current, string latest, bool force)
    {
        if (current != latest)
        {
            if (force)
            {
                if (UpdatePanelScript.instance != null)
                {
                    UpdatePanelScript.instance.setLoading(false);
                    UpdatePanelScript.instance.ShowForceUpdatePopup();
                }

            }
            else
            {
                if (UpdatePanelScript.instance != null)
                {
                    UpdatePanelScript.instance.setLoading(false);
                    UpdatePanelScript.instance.ShowOptionalUpdatePopup();
                }

            }
        }
        else
        {
            if (UpdatePanelScript.instance != null)
            {
                UpdatePanelScript.instance.setLoading(false);
                GoToHome();
            }
        }
    }

    public void GoToHome()
    {
        Debug.Log("Go Home Called");
            AppStateManager.Instance.SetHome();
            // AppStateManager.Instance.SetGameplay();
        // write your logic according to the game.
    }

    IEnumerator SafeFirestoreCall<T>(
    Func<Task<T>> firestoreTask,
    Action<T> onSuccess,
    Action onFail,
    float timeout = 10f)
    {
        bool isCompleted = false;
        bool isSuccess = false;
        T result = default;

        var task = firestoreTask();

        task.ContinueWithOnMainThread(t =>
        {
            isCompleted = true;

            if (t.IsCompleted && !t.IsFaulted && !t.IsCanceled)
            {
                result = t.Result;
                isSuccess = true;
            }
        });

        float timer = 0f;

        while (!isCompleted && timer < timeout)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        if (!isCompleted || !isSuccess)
        {
            onFail?.Invoke();
        }
        else
        {
            onSuccess?.Invoke(result);
        }
    }
}
