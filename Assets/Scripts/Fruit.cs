using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Fruit : MonoBehaviour
{
    public Action onSettled;
    public FruitData fruitData;

    [Header("Fruit Properties")]
    public int fruitIndex;

    private Rigidbody2D rb;
    private bool hasSettled = false;
    private bool isMerging = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Attempt merge if colliding with another fruit
        if (!isMerging && collision.gameObject.CompareTag("Fruit"))
        {
            Fruit otherFruit = collision.gameObject.GetComponent<Fruit>();
            if (otherFruit != null && otherFruit.fruitIndex == fruitIndex && !otherFruit.isMerging)
            {
                isMerging = true;
                otherFruit.isMerging = true;

                Vector3 mergePos = (transform.position + otherFruit.transform.position) * 0.5f;
                MergeManager.instance?.MergeFruits(fruitIndex, gameObject, otherFruit.gameObject, mergePos);
            }
        }

        // Trigger settle callback after first collision
        if (!hasSettled)
        {
            hasSettled = true;
            Invoke(nameof(NotifySettled), 0.2f);
        }
    }

    private void NotifySettled()
    {
        onSettled?.Invoke();
        onSettled = null; // Ensure callback is not invoked multiple times
    }
}
