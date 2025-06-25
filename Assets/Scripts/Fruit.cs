using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Fruit : MonoBehaviour
{
    public FruitData fruitData;

    [Header("Fruit Properties")]
    public int fruitIndex;

    private Rigidbody2D rb;
    public bool isMerging = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isMerging && collision.gameObject.CompareTag("Fruit"))
        {
            Fruit otherFruit = collision.gameObject.GetComponent<Fruit>();

            if (otherFruit != null &&
                otherFruit.fruitIndex == fruitIndex &&
                !otherFruit.isMerging)
            {
                isMerging = true;
                otherFruit.isMerging = true;

                Vector3 mergePos = (transform.position + otherFruit.transform.position) * 0.5f;
                MergeManager.instance?.QueueMergeRequest(fruitIndex, gameObject, otherFruit.gameObject, mergePos);
            }
        }
    }
}
