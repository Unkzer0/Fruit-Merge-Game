using UnityEngine;

public class BoomPowerUp : MonoBehaviour
{
    private Camera mainCam;
    private bool isActive = false;

    [Header("Sound")]
    public AudioClip BoomSound;

    public void Activate()
    {
        if (isActive) return;

        isActive = true;
        mainCam = Camera.main;
        PowerUpManager.instance.PowerUpDisableElement();
    }

    private void Update()
    {
        if (!isActive) return;

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            TryDestroyFruit(Input.mousePosition);
        }
#else
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            TryDestroyFruit(Input.GetTouch(0).position);
        }
#endif
    }

    private void TryDestroyFruit(Vector2 screenPos)
    {
        Vector2 worldPoint = mainCam.ScreenToWorldPoint(screenPos);

        Collider2D[] hits = Physics2D.OverlapPointAll(worldPoint);

        foreach (Collider2D col in hits)
        {
            Transform root = col.transform;

            while (root.parent != null && root.parent.CompareTag("Fruit"))
            {
                root = root.parent;
            }

            if (root.CompareTag("Fruit"))
            {
                SoundManager.instance.PlayButtonClick(BoomSound);
                Destroy(root.gameObject);
                isActive = false;
                PowerUpManager.instance.OnPowerUpComplete(); // Inform manager

                // Use a wrapper method to call PowerUpEnableElement
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


