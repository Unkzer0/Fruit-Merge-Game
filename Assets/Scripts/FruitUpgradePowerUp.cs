using UnityEngine;

public class FruitUpgradePowerUp : MonoBehaviour
{
    private Camera mainCam;
    private bool isActive = false;

    public void Activate()
    {
        isActive = true;
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
            Transform root = col.transform;

            // Traverse up to find the top-level Fruit object
            while (root.parent != null && root.parent.CompareTag("Fruit"))
            {
                root = root.parent;
            }

            if (root.CompareTag("Fruit"))
            {
                // Duplicate the fruit at the same position
                GameObject fruitClone = Instantiate(root.gameObject, root.position, root.rotation, root.parent);
                isActive = false;
                return;
            }
        }
    }
}
