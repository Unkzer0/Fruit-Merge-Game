using UnityEngine;

public class FruitUpgradePowerUp : MonoBehaviour
{
    private Camera mainCam;
    private bool isActive = false;

    [Header("Sound")]
    public AudioClip FruitUpgradeSound;

    public void Activate()
    {
        isActive = true;
        PowerUpManager.instance.PowerUpDisableElement();
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
            // Only upgrade the exact fruit that was clicked/touched
            if (col.CompareTag("Fruit"))
            {
                Transform fruitTransform = col.transform;
                SoundManager.instance.PlayButtonClick(FruitUpgradeSound);
                GameObject fruitClone = Instantiate(fruitTransform.gameObject, fruitTransform.position, fruitTransform.rotation, fruitTransform.parent);
                isActive = false;
                PowerUpManager.instance.OnPowerUpComplete();
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
