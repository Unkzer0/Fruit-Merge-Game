using System.Collections;
using UnityEngine;
using GoogleMobileAds.Api;

// Fruit Merge

public class Interstitial_Ad : MonoBehaviour
{
    private InterstitialAd _interstitialAd;
    public string _adUnitId;
    private bool isAdMobInitialized = false;

    private void OnEnable()
    {
        if (PlayerPrefs.GetInt("NoAds", 0) == 1)
        {
            Debug.Log("No Ads purchased — not loading interstitial ad.");
            return;
        }

        if (!isAdMobInitialized)
        {
            Debug.Log("Initializing AdMob...");
            MobileAds.Initialize((InitializationStatus initStatus) =>
            {
                isAdMobInitialized = true;
                Debug.Log("AdMob Initialized. Now loading interstitial ad.");
                LoadInterstitialAd();
            });
        }
        else
        {
            Debug.Log("AdMob already initialized. Loading interstitial ad.");
            LoadInterstitialAd();
        }
    }


    void Start()
    {
#if UNITY_ANDROID || UNITY_IOS
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            LoadInterstitialAd();
        });
#else
        Debug.Log("AdMob is disabled in WebGL.");
#endif
    }

    public void LoadInterstitialAd()
    {
#if UNITY_ANDROID || UNITY_IOS
        if (_interstitialAd != null)
        {
            DestroyInterstitialAd();
        }

        Debug.Log("Loading interstitial ad.");

        var adRequest = new AdRequest();

        InterstitialAd.Load(_adUnitId, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.LogError("Interstitial ad failed to load: " + error);
                    return;
                }

                Debug.Log("Interstitial ad loaded.");
                _interstitialAd = ad;
                RegisterEventHandlers(_interstitialAd);
            });
#endif
    }

    public void ShowInterstitialAd()
    {
#if UNITY_ANDROID || UNITY_IOS
        if (PlayerPrefs.GetInt("NoAds", 0) == 1)
        {
            Debug.Log("No Ads purchased — not showing interstitial ad.");
            return;
        }

        if (_interstitialAd != null && _interstitialAd.CanShowAd())
        {
            Debug.Log("Showing interstitial ad.");
            _interstitialAd.Show();
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready yet.");
        }
#else
    Debug.Log("Interstitial ads are disabled in WebGL.");
#endif
    }


    private void RegisterEventHandlers(InterstitialAd interstitialAd)
    {
#if UNITY_ANDROID || UNITY_IOS
        interstitialAd.OnAdPaid += HandleAdPaid;
        interstitialAd.OnAdImpressionRecorded += HandleAdImpression;
        interstitialAd.OnAdClicked += HandleAdClicked;
        interstitialAd.OnAdFullScreenContentOpened += HandleAdFullScreenOpened;
        interstitialAd.OnAdFullScreenContentClosed += HandleAdClosed;
        interstitialAd.OnAdFullScreenContentFailed += HandleAdFailedToOpen;
#endif
    }

    private void HandleAdPaid(AdValue adValue) { Debug.Log($"Interstitial ad paid {adValue.Value} {adValue.CurrencyCode}."); }
    private void HandleAdImpression() { Debug.Log("Interstitial ad recorded an impression."); }
    private void HandleAdClicked() { Debug.Log("Interstitial ad was clicked."); }
    private void HandleAdFullScreenOpened() { Debug.Log("Interstitial ad opened."); }

    public void HandleAdClosed()
    {
#if UNITY_ANDROID || UNITY_IOS
        Debug.Log("Interstitial ad closed.");
        DestroyInterstitialAd();
#endif
    }

    private void HandleAdFailedToOpen(AdError error)
    {
#if UNITY_ANDROID || UNITY_IOS
        Debug.LogError($"Interstitial ad failed to open: {error}");
#endif
    }

    public void DestroyInterstitialAd()
    {
#if UNITY_ANDROID || UNITY_IOS
        if (_interstitialAd != null)
        {
            Debug.Log("Destroying interstitial ad.");

            _interstitialAd.OnAdPaid -= HandleAdPaid;
            _interstitialAd.OnAdImpressionRecorded -= HandleAdImpression;
            _interstitialAd.OnAdClicked -= HandleAdClicked;
            _interstitialAd.OnAdFullScreenContentOpened -= HandleAdFullScreenOpened;
            _interstitialAd.OnAdFullScreenContentClosed -= HandleAdClosed;
            _interstitialAd.OnAdFullScreenContentFailed -= HandleAdFailedToOpen;

            _interstitialAd.Destroy();
            _interstitialAd = null;
        }
#endif
    }

    private void OnDestroy()
    {
#if UNITY_ANDROID || UNITY_IOS
        DestroyInterstitialAd();
#endif
    }
}
