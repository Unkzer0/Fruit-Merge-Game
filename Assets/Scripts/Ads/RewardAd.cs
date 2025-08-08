using GoogleMobileAds.Api;
using System;
using System.Collections;
using UnityEngine;

// Fruit Merge

public class RewardAd : MonoBehaviour
{
    public string _adUnitId;
    public string currentRewardKey = null;
    private RewardedAd _rewardedAd;
    private bool isAdMobInitialized = false;
    //[SerializeField] private DiamondManager diamondManager;

    private void OnEnable()
    {
        if (!isAdMobInitialized)
        {
            Debug.Log("Initializing AdMob...");
            MobileAds.Initialize((InitializationStatus initStatus) =>
            {
                isAdMobInitialized = true;
                Debug.Log("AdMob Initialized. Now loading rewarded ad.");
                LoadRewardedAd();
            });
        }
        else
        {
            Debug.Log("AdMob already initialized. Loading rewarded ad.");
            LoadRewardedAd();
        }
    }

    private void Start()
    {   

#if UNITY_ANDROID || UNITY_IOS
        // Initialize the Google Mobile Ads SDK for Mobile Only
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            LoadRewardedAd();
        });
#else
        Debug.Log("AdMob is disabled in WebGL.");
        rewardAdButton.SetActive(false); // Hide button in WebGL
#endif
    }

    public void LoadRewardedAd()
    {
#if UNITY_ANDROID || UNITY_IOS
        // Clean up old ad before loading a new one.
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad.");

        var adRequest = new AdRequest();

        // Load the ad
        RewardedAd.Load(_adUnitId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load: " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded.");
                _rewardedAd = ad;
                RegisterEventHandlers(_rewardedAd);
            });
#endif
    }

    public void ShowRewardedAd()
    {
#if UNITY_ANDROID || UNITY_IOS
        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            _rewardedAd.Show((Reward reward) =>
            {
                Debug.Log("+25 diamonds to the user");
                StartCoroutine(GrantRewardWithDelay(reward));
            });
        }
#else
        Debug.Log("Rewarded ads are disabled in WebGL.");
#endif
    }

    private IEnumerator GrantRewardWithDelay(Reward reward)
    {
        yield return new WaitForSeconds(0.1f);

        if (!string.IsNullOrEmpty(currentRewardKey) && PowerUpManager.instance != null)
        {
            PowerUpManager.instance.AddPowerUp(currentRewardKey, 1);
            Debug.Log($"Granted +1 {currentRewardKey} power-up after ad.");
        }
        else
        {
            Debug.LogWarning("No power-up reward key was set.");
        }

        // Reset for safety
        currentRewardKey = null;
        Debug.Log($"Rewarded ad rewarded user. Type: {reward.Type}, amount: {reward.Amount}");
    }

    private void RegisterEventHandlers(RewardedAd rewardedAd)
    {
        rewardedAd.OnAdPaid += HandleAdPaid;
        rewardedAd.OnAdImpressionRecorded += HandleAdImpression;
        rewardedAd.OnAdClicked += HandleAdClicked;
        rewardedAd.OnAdFullScreenContentOpened += HandleAdFullScreenOpened;
        rewardedAd.OnAdFullScreenContentClosed += HandleAdClosed;
        rewardedAd.OnAdFullScreenContentFailed += HandleAdFailedToOpen;
    }

    private void HandleAdPaid(AdValue adValue) { Debug.Log($"Rewarded ad paid {adValue.Value} {adValue.CurrencyCode}."); }
    private void HandleAdImpression() { Debug.Log("Rewarded ad impression recorded."); }
    private void HandleAdClicked() { Debug.Log("Rewarded ad clicked."); }
    private void HandleAdFullScreenOpened() { Debug.Log("Rewarded ad opened."); }

    private void HandleAdClosed()
    {
        Debug.Log("Rewarded ad closed.");
        PowerUpManager.instance?.ClosePowerUpPanel();
        DestroyRewardedAd();
        LoadRewardedAd();
    }

    private void HandleAdFailedToOpen(AdError error)
    {
        Debug.LogError($"Rewarded ad failed to open: {error}");
    }

    public void DestroyRewardedAd()
    {
        if (_rewardedAd != null)
        {
            Debug.Log("Destroying rewarded ad.");
            _rewardedAd.OnAdPaid -= HandleAdPaid;
            _rewardedAd.OnAdImpressionRecorded -= HandleAdImpression;
            _rewardedAd.OnAdClicked -= HandleAdClicked;
            _rewardedAd.OnAdFullScreenContentOpened -= HandleAdFullScreenOpened;
            _rewardedAd.OnAdFullScreenContentClosed -= HandleAdClosed;
            _rewardedAd.OnAdFullScreenContentFailed -= HandleAdFailedToOpen;

            _rewardedAd.Destroy();
            _rewardedAd = null;
        }
    }
}
