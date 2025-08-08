using UnityEngine;

public class FruitUpgradePowerUp : MonoBehaviour
{
    private Camera mainCam;
    private bool isActive = false;
    [SerializeField] GameObject powerUpInfo;

    [Header("Sound")]
    public AudioClip FruitUpgradeSound;

    public void Activate()
    {
        isActive = true;
        PowerUpManager.instance.PowerUpDisableElement();
        powerUpInfo.SetActive(true);
    }
    private void Start()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        if (!isActive) return;

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            TryUpgradeFruit(Input.mousePosition);
        }
#else
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            TryUpgradeFruit(Input.GetTouch(0).position);
        }
#endif
    }

    private void TryUpgradeFruit(Vector2 screenPos)
    {
        Vector2 worldPoint = mainCam.ScreenToWorldPoint(screenPos);
        Collider2D[] hits = Physics2D.OverlapPointAll(worldPoint);

        foreach (Collider2D col in hits)
        {
            // Traverse up the hierarchy to find the root object tagged as "Fruit"
            Transform root = col.transform;
            while (root.parent != null && !root.CompareTag("Fruit"))
            {
                root = root.parent;
            }

            // Check if it's a valid fruit
            if (root.CompareTag("Fruit"))
            {
                Fruit fruitScript = root.GetComponent<Fruit>();
                if (fruitScript == null) return;

                int currentIndex = fruitScript.fruitIndex;
                int nextIndex = currentIndex + 1;

                // Check if it is upgradable
                if (nextIndex >= FruitSelector.instance.Fruits.Length)
                {
                    Debug.Log("This fruit is already at max level.");
                    return;
                }

                // Upgrade logic
                Vector3 spawnPos = root.position;
                Transform parent = root.parent;

                Destroy(root.gameObject); // Destroy current fruit
                Instantiate(FruitSelector.instance.Fruits[nextIndex], spawnPos, Quaternion.identity, parent); // Spawn next level

                SoundManager.instance.PlayButtonClick(FruitUpgradeSound);
                isActive = false;
                PowerUpManager.instance.OnPowerUpComplete();
                powerUpInfo.SetActive(false);
                Invoke(nameof(EnablePowerUpElements), 0.2f);
                return;
            }
        }
    }

    private void EnablePowerUpElements()
    {
        PowerUpManager.instance.PowerUpEnableElement();
    }
}
