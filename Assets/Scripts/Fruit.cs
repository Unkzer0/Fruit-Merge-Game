using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Fruit : MonoBehaviour
{
    public Action onSettled;
    public FruitData fruitData;
    [Header("Fruit Properties")]
    public int fruitIndex; // Assigned when spawned

    private Rigidbody2D rb;
    private bool hasSettled = false;
    private bool isMerging = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Handle merge logic
        if (!isMerging && collision.gameObject.CompareTag("Fruit"))
        {
            Fruit otherFruit = collision.gameObject.GetComponent<Fruit>();

            if (otherFruit != null && otherFruit.fruitIndex == fruitIndex && !otherFruit.isMerging)
            {
                isMerging = true;
                otherFruit.isMerging = true;

                Vector3 mergePos = (transform.position + otherFruit.transform.position) / 2f;

                // Call MergeManager to handle the merge
                MergeManager.instance.MergeFruits(fruitIndex, transform.gameObject, otherFruit.gameObject, mergePos);
            }
        }

        if (!hasSettled)
        {
            hasSettled = true;
            Invoke(nameof(NotifySettled), 0.2f);
        }
    }

    private void NotifySettled()
    {
        onSettled?.Invoke();
        onSettled = null;
    }
}
