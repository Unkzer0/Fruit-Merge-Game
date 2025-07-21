using System.Collections;
using UnityEngine;

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

    [Header("Power-Up Counts")]
    public int boomCount = 3;
    public int fruitUpgradeCount = 3;
    public int smallFruitRemoveCount = 3;
    public int cleanUpCount = 3;

    private bool isPowerUpActive = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
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

    // Convenient methods for buttons
    public void TryUseBoom() => TryUsePowerUp("Boom");
    public void TryUseFruitUpgrade() => TryUsePowerUp("FruitUpgrade");
    public void TryUseSmallFruitRemove() => TryUsePowerUp("SmallFruitRemove");
    public void TryUseCleanUp() => TryUsePowerUp("CleanUp");
}
