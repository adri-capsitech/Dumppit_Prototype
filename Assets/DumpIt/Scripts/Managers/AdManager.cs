using System;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.UI;

public class AdManager : MonoBehaviour
{
    // test ad unit IDs
#if UNITY_ANDROID
    private string TestrewardedAdUnitId = "ca-app-pub-3940256099942544/5224354917";
    private string TestinterstitialAdUnitId = "ca-app-pub-3940256099942544/1033173712";
#elif UNITY_IPHONE
    private string TestrewardedAdUnitId = "ca-app-pub-3940256099942544/1712485313";
    private string TestinterstitialAdUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
    private string TestrewardedAdUnitId = "unused";
    private string TestinterstitialAdUnitId = "unused";
#endif

    private RewardedAd rewardedAd;
    private InterstitialAd interstitialAd;
    public static AdManager Instance { get; private set; }
    private void Awake()
    {
        CheckInstance();
    }
    private void CheckInstance()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        LoadAd();
        InterstitialLoadAd();
    }

    void LoadAd()
    {
        // [START load_ad]
        // Create our request used to load the ad.
        var adRequest = new AdRequest();

        // Send the request to load the ad.
        RewardedAd.Load(TestrewardedAdUnitId, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null)
            {
                // The ad failed to load.
                return;
            }
            rewardedAd = ad;
            // The ad loaded successfully.
        });
        // [END load_ad]
    }
    void InterstitialLoadAd()
    {
        // Create our request used to load the ad.
        var adRequest = new AdRequest();

        // Send the request to load the ad.
        InterstitialAd.Load(TestinterstitialAdUnitId, adRequest, (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null)
            {
                // The ad failed to load.
                Debug.LogError("Interstitial load failed: " + error);
                return;
            }
            // The ad loaded successfully.
            interstitialAd = ad;
            //Debug.Log("Interstitial Loaded");
        });
    }


    public void ShowAd(Action onAdFinished)
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                // User watched ad fully
                onAdFinished?.Invoke();
                InterstitialReloadAd();
            });

            // Also handle when ad is closed (even without reward)
            rewardedAd.OnAdFullScreenContentClosed += () =>
            {
                onAdFinished?.Invoke();
                InterstitialReloadAd();
            };
        }
        else
        {
            //Debug.Log("Ad not ready, skipping ad...");
            onAdFinished?.Invoke(); // Fallback: restart game anyway
            LoadAd();
        }
    }
    // public void InterstitialShowAd()
    // {
    //     if (interstitialAd != null && interstitialAd.CanShowAd())
    //     {
    //         interstitialAd.Show();
    //     }
    // }
    // public void ShowInterstitial(System.Action onAdClosed)
    // {
    //     if (interstitialAd != null && interstitialAd.CanShowAd())
    //     {
    //         InterstitialLoadAd(); // reload
    //         onAdClosed?.Invoke();

    //         interstitialAd.Show();
    //     }
    //     else
    //     {
    //         //Debug.Log("Interstitial not ready, skipping");
    //         onAdClosed?.Invoke();
    //         InterstitialLoadAd();
    //     }

    // }
    public void ShowInterstitial(System.Action onAdClosed)
    {
        if (interstitialAd == null || !interstitialAd.CanShowAd())
        {
            Debug.Log("Interstitial not ready");
            return;
        }


        Time.timeScale = 1f;
        interstitialAd.Show();
        onAdClosed?.Invoke();
        InterstitialLoadAd();

    }

    void ListenToAdEvents()
    {
        // [START ad_events]
        rewardedAd.OnAdPaid += (AdValue adValue) =>
        {
            // Raised when the ad is estimated to have earned money.
        };
        rewardedAd.OnAdImpressionRecorded += () =>
        {
            // Raised when an impression is recorded for an ad.
        };
        rewardedAd.OnAdClicked += () =>
        {
            // Raised when a click is recorded for an ad.
        };
        rewardedAd.OnAdFullScreenContentOpened += () =>
        {
            // Raised when the ad opened full screen content.
        };
        rewardedAd.OnAdFullScreenContentClosed += () =>
        {
            // Raised when the ad closed full screen content.
        };
        rewardedAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            // Raised when the ad failed to open full screen content.
        };
        // [END ad_events]]
    }
    void InterstitialListenToAdEvents()
    {
        interstitialAd.OnAdPaid += (AdValue adValue) =>
        {
            // Raised when the ad is estimated to have earned money.
        };
        interstitialAd.OnAdImpressionRecorded += () =>
        {
            // Raised when an impression is recorded for an ad.
        };
        interstitialAd.OnAdClicked += () =>
        {
            // Raised when a click is recorded for an ad.
        };
        interstitialAd.OnAdFullScreenContentOpened += () =>
        {
            // Raised when the ad opened full screen content.
        };
        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            // Raised when the ad closed full screen content.
        };
        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            // Raised when the ad failed to open full screen content.
        };
    }


    void DestroyAd(RewardedAd rewardedAd)
    {
        // [START destroy_ad]
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
        }
        // [END destroy_ad]]
    }
    void InterstitialDestroyAd(InterstitialAd interstitialAd)
    {
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
        }
    }

    // public void ReloadAd()
    // {
    //     // [START reload_ad]
    //     rewardedAd.OnAdFullScreenContentClosed += () =>
    //     {
    //         // Reload the ad so that we can show another as soon as possible.
    //         var adRequest = new AdRequest();
    //         RewardedAd.Load(AD_UNIT_ID, adRequest, (RewardedAd ad, LoadAdError error) =>
    //         {
    //             rewardedAd = ad;
    //             // Handle ad loading here.
    //         });
    //     };
    //     // [END reload_ad]]
    // }
    public void ReloadAd()
    {
        var adRequest = new AdRequest();
        RewardedAd.Load(TestrewardedAdUnitId, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null)
            {
                Debug.LogError("Failed to reload ad: " + error);
                return;
            }

            rewardedAd = ad;
            //Debug.Log("Ad reloaded");
        });
    }
    public void InterstitialReloadAd()
    {
        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            // Reload the ad so that we can show another as soon as possible.
            var adRequest = new AdRequest();
            InterstitialAd.Load(TestinterstitialAdUnitId, adRequest, (InterstitialAd ad, LoadAdError error) =>
            {
                // Handle ad loading here.
                if (error != null)
                {
                    Debug.LogError("Failed to reload ad: " + error);
                    return;
                }

                interstitialAd = ad;
                //Debug.Log("Ad reloaded");
            });
        };
    }


}