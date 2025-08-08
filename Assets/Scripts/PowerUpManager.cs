using System.Collections;
using TMPro;
using UnityEngine;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager instance;

    [Header("Container Settings")]
    public Collider2D containerCollider;
    public GameObject containerEmptyText;

    [Header("Power-Up Scripts")]
    [SerializeField] private BoomPowerUp boomPowerUp;
    [SerializeField] private FruitUpgradePowerUp fruitUpgradePowerUp;
    [SerializeField] private SmallFruitRemovePowerUp smallFruitRemovePowerUp;
    [SerializeField] private CleanUpPowerUp cleanUpPowerUp;

    [Header("Power-Up Credits")]
    [SerializeField] TextMeshProUGUI BoomCredit;
    [SerializeField] TextMeshProUGUI FruitUpgradeCredits;
    [SerializeField] TextMeshProUGUI SmallFruitUpgradeCredits;
    [SerializeField] TextMeshProUGUI CleanUpPowerUpCredits;

    [Header("Power-Up Effected Elements")]
    [SerializeField] GameObject fruitDropper;
    [SerializeField] GameObject scoreBoard;
    [SerializeField] GameObject nextFruitBar;
    [SerializeField] GameObject noAds;
    [SerializeField] GameObject setting;
    [SerializeField] GameObject powerup1;
    [SerializeField] GameObject powerup2;
    [SerializeField] GameObject powerup3;
    [SerializeField] GameObject powerup4;
    [SerializeField] GameObject fruitBar;

    [SerializeField] private AudioClip buttonClickSound;

    // Power-up keys
    private const string BoomKey = "PowerUp_Boom";
    private const string FruitUpgradeKey = "PowerUp_FruitUpgrade";
    private const string SmallFruitRemoveKey = "PowerUp_SmallFruitRemove";
    private const string CleanUpKey = "PowerUp_CleanUp";

    [Header("Power-Up Diamond Costs")]
    [SerializeField] private int boomCost = 5;
    [SerializeField] private int fruitUpgradeCost = 5;
    [SerializeField] private int smallFruitRemoveCost = 5;
    [SerializeField] private int cleanUpCost = 5;


    private bool isPowerUpActive = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        BoomCredit.text = GetPowerUpCount(BoomKey).ToString();
        FruitUpgradeCredits.text = GetPowerUpCount(FruitUpgradeKey).ToString();
        SmallFruitUpgradeCredits.text = GetPowerUpCount(SmallFruitRemoveKey).ToString();
        CleanUpPowerUpCredits.text = GetPowerUpCount(CleanUpKey).ToString();
    }

    // PlayerPrefs utility
    public int GetPowerUpCount(string key)
    {
        return PlayerPrefs.GetInt(key, 2);
    }

    public void SetPowerUpCount(string key, int count)
    {
        PlayerPrefs.SetInt(key, count);
        PlayerPrefs.Save();
        UpdatePowerUpUI(key, count);
    }

    public void AddPowerUp(string key, int amount)
    {
        int newCount = GetPowerUpCount(key) + amount;
        SetPowerUpCount(key, newCount);
    }

    private void UpdatePowerUpUI(string key, int count)
    {
        if (key == BoomKey && BoomCredit != null)
            BoomCredit.text = count.ToString();
        else if (key == FruitUpgradeKey && FruitUpgradeCredits != null)
            FruitUpgradeCredits.text = count.ToString();
        else if (key == SmallFruitRemoveKey && SmallFruitUpgradeCredits != null)
            SmallFruitUpgradeCredits.text = count.ToString();
        else if (key == CleanUpKey && CleanUpPowerUpCredits != null)
            CleanUpPowerUpCredits.text = count.ToString();
    }

    public void OnPowerUpComplete()
    {
        isPowerUpActive = false;
    }

    public bool TryUsePowerUp(string powerUpName)
    {
        if (isPowerUpActive)
            return false;

        if (IsContainerEmpty())
        {
            StartCoroutine(ShowContainerEmptyText());
            return false;
        }

        string key = null;
        MonoBehaviour powerUpScript = null;

        switch (powerUpName)
        {
            case "Boom":
                key = BoomKey;
                powerUpScript = boomPowerUp;
                break;
            case "FruitUpgrade":
                key = FruitUpgradeKey;
                powerUpScript = fruitUpgradePowerUp;
                break;
            case "SmallFruitRemove":
                key = SmallFruitRemoveKey;
                powerUpScript = smallFruitRemovePowerUp;
                break;
            case "CleanUp":
                key = CleanUpKey;
                powerUpScript = cleanUpPowerUp;
                break;
        }

        if (key != null && GetPowerUpCount(key) > 0)
        {
            SetPowerUpCount(key, GetPowerUpCount(key) - 1);
            StartCoroutine(ActivatePowerUpCoroutine(powerUpScript));
            return true;
        }
        else
        {
            if (PanelManager.instance != null)
            {
                switch (powerUpName)
                {
                    case "Boom": PanelManager.instance.ShowBoompowerUp(); WatchAdForPowerUp(powerUpName); break;
                    case "FruitUpgrade": PanelManager.instance.ShowFruitUpgradepowerUp(); WatchAdForPowerUp(powerUpName); break;
                    case "SmallFruitRemove": PanelManager.instance.ShowSmallFruitpowerUp(); WatchAdForPowerUp(powerUpName); break;
                    case "CleanUp": PanelManager.instance.ShowClearFruitpowerUp(); WatchAdForPowerUp(powerUpName); break;
                }
            }
        }

        return false;
    }

    private bool IsContainerEmpty()
    {
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(LayerMask.GetMask("Fruit"));

        Collider2D[] results = new Collider2D[10];
        int count = containerCollider.OverlapCollider(filter, results);

        return count == 0;
    }

    private IEnumerator ShowContainerEmptyText()
    {
        containerEmptyText.SetActive(true);
        yield return new WaitForSeconds(1f);
        containerEmptyText.SetActive(false);
    }

    private IEnumerator ActivatePowerUpCoroutine(MonoBehaviour powerUp)
    {
        isPowerUpActive = true;

        if (powerUp is BoomPowerUp boom)
            boom.Activate();
        else if (powerUp is FruitUpgradePowerUp upgrade)
            upgrade.Activate();
        else if (powerUp is SmallFruitRemovePowerUp remove)
            remove.Activate();
        else if (powerUp is CleanUpPowerUp clean)
            clean.Activate();

        yield return new WaitForSeconds(1f);

        isPowerUpActive = false;
    }

    public void PowerUpDisableElement()
    {
        fruitDropper.SetActive(false); scoreBoard.SetActive(false);
        nextFruitBar.SetActive(false); noAds.SetActive(false);
        setting.SetActive(false); powerup1.SetActive(false);
        powerup2.SetActive(false); powerup3.SetActive(false);
        powerup4.SetActive(false); fruitBar.SetActive(false);
    }

    public void PowerUpEnableElement()
    {
        fruitDropper.SetActive(true); scoreBoard.SetActive(true);
        nextFruitBar.SetActive(true); noAds.SetActive(true);
        setting.SetActive(true); powerup1.SetActive(true);
        powerup2.SetActive(true); powerup3.SetActive(true);
        powerup4.SetActive(true); fruitBar.SetActive(true);
    }

    private void PlayClickSound()
    {
        if (buttonClickSound != null)
        {
            SoundManager.instance?.PlayButtonClick(buttonClickSound);
        }
    }

    // Button methods
    public void TryUseBoom() => TryUsePowerUp("Boom");
    public void TryUseFruitUpgrade() => TryUsePowerUp("FruitUpgrade");
    public void TryUseSmallFruitRemove() => TryUsePowerUp("SmallFruitRemove");
    public void TryUseCleanUp() => TryUsePowerUp("CleanUp");

    public void ClosePowerUpPanel()
    {
        PlayClickSound();
        PanelManager.instance?.ShowOnly(null);
        PowerUpEnableElement();
    }

    public bool IsPowerUpActive => isPowerUpActive;

    public void WatchAdForPowerUp(string powerUpName)
    {
        var rewardAd = FindObjectOfType<RewardAd>();
        if (rewardAd == null) return;

        switch (powerUpName)
        {
            case "Boom":
                rewardAd.currentRewardKey = BoomKey;
                break;
            case "FruitUpgrade":
                rewardAd.currentRewardKey = FruitUpgradeKey;
                break;
            case "SmallFruitRemove":
                rewardAd.currentRewardKey = SmallFruitRemoveKey;
                break;
            case "CleanUp":
                rewardAd.currentRewardKey = CleanUpKey;
                break;
        }
    }

    public void BuyPowerUpWithDiamonds(string powerUpName)
    {
        string key = null;
        int cost = 0;

        switch (powerUpName)
        {
            case "Boom":
                key = BoomKey;
                cost = boomCost;
                break;
            case "FruitUpgrade":
                key = FruitUpgradeKey;
                cost = fruitUpgradeCost;
                break;
            case "SmallFruitRemove":
                key = SmallFruitRemoveKey;
                cost = smallFruitRemoveCost;
                break;
            case "CleanUp":
                key = CleanUpKey;
                cost = cleanUpCost;
                break;
        }

        if (key != null)
        {
            if (DiamondManager.Instance.SpendDiamonds(cost))
            {
                AddPowerUp(key, 1); // Grant 1 power-up
                UpdatePowerUpUI(key, GetPowerUpCount(key));
                Debug.Log($"Bought {powerUpName} power-up with {cost} diamonds.");
            }
            else
            {
                PanelManager.instance.ShowShop();
                Debug.Log("Not enough diamonds to buy power-up.");
            }
        }
    }

    public void BuyBoomWithDiamonds() => BuyPowerUpWithDiamonds("Boom");
    public void BuyFruitUpgradeWithDiamonds() => BuyPowerUpWithDiamonds("FruitUpgrade");
    public void BuySmallFruitRemoveWithDiamonds() => BuyPowerUpWithDiamonds("SmallFruitRemove");
    public void BuyCleanUpWithDiamonds() => BuyPowerUpWithDiamonds("CleanUp");


}
