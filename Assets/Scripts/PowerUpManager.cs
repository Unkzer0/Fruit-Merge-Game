using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager instance;

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

    [Header("UI Panel")]
    public GameObject buyBoomUpPanel;
    public GameObject buyFruitUpgradePanel;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    /// <summary>
    /// Try to use a power-up by name. Returns true if used, otherwise shows Buy panel.
    /// </summary>
    public bool TryUsePowerUp(string powerUpName)
    {
        switch (powerUpName)
        {
            case "Boom":
                if (boomCount > 0)
                {
                    boomCount--;
                    boomPowerUp.Activate();
                    return true;
                }
                break;

            case "FruitUpgrade":
                if (fruitUpgradeCount > 0)
                {
                    fruitUpgradeCount--;
                    fruitUpgradePowerUp.gameObject.SetActive(true); // Ensure script is active
                    fruitUpgradePowerUp.Activate();
                    return true;
                }
                break;

                 case "SmallFruitRemove":
                    if (smallFruitRemoveCount > 0)
                   {
                        smallFruitRemoveCount--;
                        smallFruitRemovePowerUp.Activate();
                        return true;
                   }
                   break;

                 case "CleanUp":
                     if (cleanUpCount > 0)
                     {
                         cleanUpCount--;
                         cleanUpPowerUp.Activate();
                         return true;
                     }
                     break;
        }

        ShowBuyPanel(); // If power-up not available
        return false;
    }

    /// <summary>
    /// UI Button triggers
    /// </summary>
    public void TryUseBoom() => TryUsePowerUp("Boom");
    public void TryUseFruitUpgrade() => TryUsePowerUp("FruitUpgrade");
    public void TryUseSmallFruitRemove() => TryUsePowerUp("SmallFruitRemove");
    public void TryUseCleanUp() => TryUsePowerUp("CleanUp");

    /// <summary>
    /// Show the Buy More Power-Ups panel
    /// </summary>
    private void ShowBuyPanel()
    {
        if (buyBoomUpPanel != null)
            buyFruitUpgradePanel.SetActive(true);
    }
}
