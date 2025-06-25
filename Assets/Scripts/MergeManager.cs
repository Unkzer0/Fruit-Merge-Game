using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeManager : MonoBehaviour
{
    public static MergeManager instance;

    [Header("Merge Settings")]
    [SerializeField] private AudioClip mergeSound;
    [SerializeField] private float bounceScale = 1.2f;
    [SerializeField] private float bounceDuration = 0.2f;

    private bool sfxMuted;
    private Queue<MergeRequest> mergeQueue = new Queue<MergeRequest>();
    private bool isMerging = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            sfxMuted = PlayerPrefs.GetInt("SFXMuted", 0) == 1;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ToggleSFX()
    {
        sfxMuted = !sfxMuted;
        PlayerPrefs.SetInt("SFXMuted", sfxMuted ? 1 : 0);
    }

    public bool IsSFXMuted() => sfxMuted;

    // Called from Fruit.cs
    public void QueueMergeRequest(int fruitIndex, GameObject fruit1, GameObject fruit2, Vector3 mergePosition)
    {
        if (fruit1 == null || fruit2 == null) return;

        // Always enqueue the merge
        mergeQueue.Enqueue(new MergeRequest
        {
            index = fruitIndex,
            fruitA = fruit1,
            fruitB = fruit2,
            position = mergePosition
        });

        // Start processing if not already
        if (!isMerging)
        {
            StartCoroutine(ProcessMergeQueue());
        }
    }

    private IEnumerator ProcessMergeQueue()
    {
        isMerging = true;

        while (mergeQueue.Count > 0)
        {
            MergeRequest current = mergeQueue.Dequeue();

            // Skip invalid objects
            if (current.fruitA == null || current.fruitB == null) continue;

            // Score update
            Fruit fruitScript = current.fruitA.GetComponent<Fruit>();
            if (fruitScript?.fruitData != null)
            {
                ScoreManager.instance?.AddScore(fruitScript.fruitData.scoreValue);

                // Unlock in progression bar
                FruitBarUIManager.instance?.UnlockFruit(fruitScript.fruitIndex - FruitBarUIManager.instance.StartIndex);
            }

            Destroy(current.fruitA);
            Destroy(current.fruitB);

            int nextIndex = current.index + 1;
            if (FruitSelector.instance == null || nextIndex >= FruitSelector.instance.Fruits.Length) continue;

            GameObject newFruit = Instantiate(FruitSelector.instance.Fruits[nextIndex], current.position, Quaternion.identity);
            newFruit.transform.localScale = Vector3.one;

            Fruit newFruitScript = newFruit.GetComponent<Fruit>();
            if (newFruitScript != null)
            {
                newFruitScript.fruitIndex = nextIndex;
            }

            if (!sfxMuted && mergeSound != null)
            {
                AudioSource.PlayClipAtPoint(mergeSound, current.position);
            }

            StartCoroutine(BounceEffect(newFruit.transform, bounceScale, bounceDuration));

            // Delay before next merge
            yield return new WaitForSeconds(0.2f);
        }

        isMerging = false;
    }

    private IEnumerator BounceEffect(Transform target, float scaleMultiplier, float duration)
    {
        if (target == null) yield break;

        Vector3 originalScale = Vector3.one;
        Vector3 targetScale = originalScale * scaleMultiplier;

        float timer = 0f;
        while (timer < duration)
        {
            if (target == null) yield break;
            target.localScale = Vector3.Lerp(originalScale, targetScale, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0f;
        while (timer < 0.1f)
        {
            if (target == null) yield break;
            target.localScale = Vector3.Lerp(targetScale, originalScale, timer / 0.1f);
            timer += Time.deltaTime;
            yield return null;
        }

        if (target != null)
            target.localScale = originalScale;
    }

    private class MergeRequest
    {
        public int index;
        public GameObject fruitA;
        public GameObject fruitB;
        public Vector3 position;
    }
}
