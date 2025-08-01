using GoogleMobileAds.Api;
using System;
using System.Collections;
using UnityEngine;

// Fruit Merge

public class BannerAd : MonoBehaviour
{
    public string _adUnitId;
    private BannerView _bannerView;
    private bool isAdLoading = false;

    private void OnEnable()
    {
        if (PlayerPrefs.GetInt("NoAds", 0) == 1)
        {
            Debug.Log("No Ads purchased — not loading banner ad.");
            return;
        }

        LoadAd();
    }

    void Start()
    {
#if UNITY_ANDROID || UNITY_IOS
        // Initialize the Google Mobile Ads SDK for Mobile Only
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            LoadAd();
        });
#else
        Debug.Log("AdMob is disabled in WebGL.");
#endif
    }

    public void CreateBannerView()
    {
#if UNITY_ANDROID || UNITY_IOS
        Debug.Log("Creating banner view");

        if (_bannerView != null)
        {
            DestroyAd();
        }

        _bannerView = new BannerView(_adUnitId, AdSize.Banner, AdPosition.Bottom);
#endif
    }

    public void LoadAd()
    {
#if UNITY_ANDROID || UNITY_IOS
        if (isAdLoading)
        {
            Debug.Log("Ad is already being loaded.");
            return;
        }

        isAdLoading = true;

        if (_bannerView == null)
        {
            CreateBannerView();
        }

        var adRequest = new AdRequest();
        Debug.Log("Loading banner ad.");
        _bannerView.LoadAd(adRequest);

        ListenToAdEvents();
#else
        Debug.Log("Banner ads are disabled in WebGL.");
#endif
    }

    private void ListenToAdEvents()
    {
#if UNITY_ANDROID || UNITY_IOS
        _bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log("Banner view loaded an ad.");
            isAdLoading = false;
        };

        _bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.LogError("Banner view failed to load an ad: " + error);
            isAdLoading = false;
            StartCoroutine(RetryLoadAd());
        };

        _bannerView.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log($"Banner view paid {adValue.Value} {adValue.CurrencyCode}.");
        };

        _bannerView.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Banner view recorded an impression.");
        };

        _bannerView.OnAdClicked += () =>
        {
            Debug.Log("Banner view was clicked.");
        };

        _bannerView.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Banner view full screen content opened.");
        };

        _bannerView.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Banner view full screen content closed.");
            StartCoroutine(DelayLoadAd());
        };
#endif
    }

    private IEnumerator RetryLoadAd()
    {
#if UNITY_ANDROID || UNITY_IOS
        yield return new WaitForSeconds(2);
        LoadAd();
#else
        yield break; 
#endif
    }

    private IEnumerator DelayLoadAd()
    {
#if UNITY_ANDROID || UNITY_IOS
        yield return new WaitForSeconds(2);
        LoadAd();
#else
        yield break; 
#endif
    }

    public void DestroyAd()
    {
#if UNITY_ANDROID || UNITY_IOS
        if (_bannerView != null)
        {
            Debug.Log("Destroying banner view.");

            _bannerView.OnBannerAdLoaded -= null;
            _bannerView.OnBannerAdLoadFailed -= null;
            _bannerView.OnAdPaid -= null;
            _bannerView.OnAdImpressionRecorded -= null;
            _bannerView.OnAdClicked -= null;
            _bannerView.OnAdFullScreenContentOpened -= null;
            _bannerView.OnAdFullScreenContentClosed -= null;

            _bannerView.Destroy();
            _bannerView = null;
        }
#endif
    }

    public void HideAd()
    {
        if (_bannerView != null)
        {
            _bannerView.Hide();
        }
    }
}
