using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Fruit : MonoBehaviour
{
    public Action onSettled;

    private Rigidbody2D rb;
    private bool hasSettled = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasSettled) return;

        // Optional: You can add a condition to make sure it hit ground or another fruit
        hasSettled = true;

        // Delay to ensure the fruit visually settles
        Invoke(nameof(NotifySettled), 0.2f);
    }

    private void NotifySettled()
    {
        onSettled?.Invoke();
        onSettled = null; // Avoid calling again
    }
}
