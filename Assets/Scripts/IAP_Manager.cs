using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Purchasing;
using TMPro;

// Fruit Merge

[Serializable]
public class ConsumableItem
{
    public string Name;
    public string Id;
    public string description;
    public float price;
    public int diamondAmount;
}

[Serializable]
public class StarterBundleConsumableItem
{
    public string Name;
    public string Id;
    public string description;
    public float price;
    public int powerUpAmount;
}

[Serializable]
public class NonConsumableItem
{
    public string Name;
    public string Id;
    public string description;
    public float price;
}

[Serializable]
public class ComboBundleNonConsumableItem
{
    public string Name;
    public string Id;
    public string description;
    public float price;
    public int diamondAmount;
}

public class IAP_Manager : MonoBehaviour, IStoreListener
{
    IStoreController m_StoreController;

    public List<ConsumableItem> cItems;
    public StarterBundleConsumableItem sbItem;
    public NonConsumableItem ncItem;
    public ComboBundleNonConsumableItem cbItem;

    public DiamondManager diamondManager;
    public BannerAd bannerAd;
    public Interstitial_Ad interstitial_Ad;
    public List<TextMeshProUGUI> priceTexts;

    // Start is called before the first frame update
    void Start()
    {
        SetupBuilder();
    }

    private void SetupBuilder()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        foreach (var item in cItems)
        {
            builder.AddProduct(item.Id, ProductType.Consumable);
        }

        builder.AddProduct(sbItem.Id, ProductType.Consumable);

        builder.AddProduct(ncItem.Id, ProductType.NonConsumable);

        builder.AddProduct(cbItem.Id, ProductType.NonConsumable);

        UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        print("Success");
        m_StoreController = controller;
        CheckNonConsumable(ncItem.Id);
        CheckNonConsumable(cbItem.Id);
        UpdateLocalizedPrices();
    }

    public void PurchaseById(string productId)
    {
        m_StoreController.InitiatePurchase(productId);
    }

    public void NonConsumableButtonPressed()
    {
        m_StoreController.InitiatePurchase(ncItem.Id);
    }

    public void ComboBundleNonConsumableButtonPressed()
    {
        m_StoreController.InitiatePurchase(cbItem.Id);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        var product = purchaseEvent.purchasedProduct;

        print("Purchase Complete " + product.definition.id);

        foreach (var item in cItems)
        {
            if (product.definition.id == item.Id)
            {
                diamondManager.AddDiamonds(item.diamondAmount);
                break;
            }
        }

        if (product.definition.id == sbItem.Id)
        {
             PowerUpManager.instance?.AddPowerUp("PowerUp_Boom", sbItem.powerUpAmount);
             PowerUpManager.instance?.AddPowerUp("PowerUp_FruitUpgrade", sbItem.powerUpAmount);
             PowerUpManager.instance?.AddPowerUp("PowerUp_SmallFruitRemove", sbItem.powerUpAmount);
             PowerUpManager.instance?.AddPowerUp("PowerUp_CleanUp", sbItem.powerUpAmount);
        }

        if (product.definition.id == ncItem.Id)
        {
            RemoveAds();
        }

        if (product.definition.id == cbItem.Id)
        {
            RemoveAds();
            diamondManager.AddDiamonds(cbItem.diamondAmount);
        }

        return PurchaseProcessingResult.Complete;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        print("Initialize Failed: " + error);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        print("Initialize Failed: " + error + " " + message);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        print("Purchasing Failed: " + failureReason);
    }

    private void CheckNonConsumable(string id)
    {
        if (m_StoreController != null)
        {
            var product = m_StoreController.products.WithID(id);
            if (product != null)
            {
                if (product.hasReceipt)
                {
                    RemoveAds();
                }

                else
                {
                    ShowAds();
                }

                /*if (product.hasReceipt)
                {
                    if (id.Equals(ncItem.Id))
                    {
                        RemoveAds();
                    }
                    else if (id.Equals(cbItem.Id))
                    {
                        RemoveAds();
                        //diamondManager.UpdateDiamonds(cbItem.diamondAmount);
                    }
                }

                else
                {
                    ShowAds();
                }*/
            }
        }
    }

    private void RemoveAds()
    {
        PlayerPrefs.SetInt("NoAds", 1);
        PlayerPrefs.Save();
        Debug.Log("No Ads activated via receipt check.");

        if (bannerAd != null)
        {
            bannerAd.HideAd();
            bannerAd.DestroyAd();
        }

        if (interstitial_Ad != null)
        {
            interstitial_Ad.DestroyInterstitialAd();
        }
    }

    private void ShowAds()
    {
        PlayerPrefs.SetInt("NoAds", 0);
        PlayerPrefs.Save();

        /*if (bannerAd != null)
        {
           bannerAd.LoadAd();
        }

        if (interstitial_Ad != null)
        {
           interstitial_Ad.LoadInterstitialAd();
        }*/

        Debug.Log("Ads have been re-enabled.");
    }

    private void UpdateLocalizedPrices()
    {
        // First update all cItems
        for (int i = 0; i < cItems.Count; i++)
        {
            var item = cItems[i];
            Product product = m_StoreController.products.WithID(item.Id);

            if (product != null && i < priceTexts.Count && priceTexts[i] != null)
            {
                string localizedPrice = product.metadata.localizedPriceString;
                priceTexts[i].text = localizedPrice;
            }
        }

        // Now handle scItem separately (use priceTexts[4] in this case)
        if (sbItem != null && priceTexts.Count > cItems.Count && priceTexts[cItems.Count] != null)
        {
            Product sbProduct = m_StoreController.products.WithID(sbItem.Id);

            if (sbProduct != null)
            {
                string localizedPrice = sbProduct.metadata.localizedPriceString;
                priceTexts[cItems.Count].text = localizedPrice; // 👉 This will use index 4 when cItems.Count = 4
            }
        }

        if (cbItem != null && priceTexts.Count > cItems.Count + 1 && priceTexts[cItems.Count + 1] != null)
        {
            Product cbProduct = m_StoreController.products.WithID(cbItem.Id);

            if (cbProduct != null)
            {
                string localizedPrice = cbProduct.metadata.localizedPriceString;
                priceTexts[cItems.Count + 1].text = localizedPrice;
            }
        }

        if (ncItem != null && priceTexts.Count > cItems.Count + 2 && priceTexts[cItems.Count + 2] != null)
        {
            Product ncProduct = m_StoreController.products.WithID(ncItem.Id);

            if (ncProduct != null)
            {
                string localizedPrice = ncProduct.metadata.localizedPriceString;
                priceTexts[cItems.Count + 2].text = localizedPrice;
            }
        }
    }
}
