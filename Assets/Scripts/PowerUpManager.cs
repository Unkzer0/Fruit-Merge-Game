using System.Collections;
using UnityEngine;
using TMPro;
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

    private int boomCount = 1;
    private int fruitUpgradeCount = 1;
    private int smallFruitRemoveCount = 1;
    private int cleanUpCount = 1;

    private bool isPowerUpActive = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

 public void CrossButton()
    {
        PlayClickSound();
        PanelManager.instance?.ShowOnly(null);
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

        switch (powerUpName)
        {
            case "Boom":
                if (boomCount > 0)
                {
                    boomCount--;
                    BoomCredit.text = boomCount.ToString();
                    StartCoroutine(ActivatePowerUpCoroutine(boomPowerUp));
                    return true;
                }
                else
                    PanelManager.instance?.ShowBoompowerUp();
                break;

            case "FruitUpgrade":
                if (fruitUpgradeCount > 0)
                {
                    fruitUpgradeCount--;
                    FruitUpgradeCredits.text = fruitUpgradeCount.ToString();
                    fruitUpgradePowerUp.gameObject.SetActive(true);
                    StartCoroutine(ActivatePowerUpCoroutine(fruitUpgradePowerUp));
                    return true;
                }
                else
                    PanelManager.instance?.ShowFruitUpgradepowerUp();
                break;

            case "SmallFruitRemove":
                if (smallFruitRemoveCount > 0)
                {
                    smallFruitRemoveCount--;
                    SmallFruitUpgradeCredits.text = smallFruitRemoveCount.ToString();
                    StartCoroutine(ActivatePowerUpCoroutine(smallFruitRemovePowerUp));
                    return true;
                }
                else
                    PanelManager.instance?.ShowSmallFruitpowerUp();
                break;

            case "CleanUp":
                if (cleanUpCount > 0)
                {
                    cleanUpCount--;
                    CleanUpPowerUpCredits.text = cleanUpCount.ToString();
                    StartCoroutine(ActivatePowerUpCoroutine(cleanUpPowerUp));
                    return true;
                }
                else
                    PanelManager.instance?.ShowClearFruitpowerUp();
                break;
        }

        return false;
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

        // Wait until power-up finishes if it has a known delay (optional)
        yield return new WaitForSeconds(1f);

        isPowerUpActive = false;
    }

    public void PowerUpDisableElement()
    {
        fruitDropper.SetActive(false);
        scoreBoard.SetActive(false);
        nextFruitBar.SetActive(false);
        noAds.SetActive(false);
        setting.SetActive(false);
        powerup1.SetActive(false);
        powerup2.SetActive(false);  
        powerup3.SetActive(false);
        powerup4.SetActive(false);
        fruitBar.SetActive(false);
    }

    public void PowerUpEnableElement()
    {
        fruitDropper.SetActive(true);
        scoreBoard.SetActive(true);
        nextFruitBar.SetActive(true);
        noAds.SetActive(true);
        setting.SetActive(true);
        powerup1.SetActive(true);
        powerup2.SetActive(true);
        powerup3.SetActive(true);
        powerup4.SetActive(true);
        fruitBar.SetActive(true);
    }

    private void PlayClickSound()
    {
        if (buttonClickSound != null)
        {
            SoundManager.instance?.PlayButtonClick(buttonClickSound);
        }
    }

    // Convenient methods for buttons
    public void TryUseBoom() => TryUsePowerUp("Boom");
    public void TryUseFruitUpgrade() => TryUsePowerUp("FruitUpgrade");
    public void TryUseSmallFruitRemove() => TryUsePowerUp("SmallFruitRemove");
    public void TryUseCleanUp() => TryUsePowerUp("CleanUp");

    public bool IsPowerUpActive => isPowerUpActive; // Expose the flag
}
