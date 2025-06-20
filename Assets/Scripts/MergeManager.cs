using System.Collections;
using UnityEngine;

public class MergeManager : MonoBehaviour
{
    public static MergeManager instance;

    [Header("Merge Settings")]
    [SerializeField] private AudioClip mergeSound;
    [SerializeField] private float bounceScale = 1.2f;
    [SerializeField] private float bounceDuration = 0.2f;

    private bool sfxMuted;

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

    public void MergeFruits(int currentIndex, GameObject fruit1, GameObject fruit2, Vector3 spawnPos)
    {
        if (fruit1 == null || fruit2 == null) return;

        // Award score
        Fruit fruitScript1 = fruit1.GetComponent<Fruit>();
        if (fruitScript1?.fruitData != null)
        {
            ScoreManager.instance?.AddScore(fruitScript1.fruitData.scoreValue);
        }

        // Destroy merged fruits
        Destroy(fruit1);
        Destroy(fruit2);

        // Spawn new merged fruit
        int nextIndex = currentIndex + 1;
        if (FruitSelector.instance == null || nextIndex >= FruitSelector.instance.Fruits.Length) return;

        GameObject newFruit = Instantiate(FruitSelector.instance.Fruits[nextIndex], spawnPos, Quaternion.identity);
        newFruit.transform.localScale = Vector3.one;

        // Unlock in fruit progression UI
        FruitBarUIManager.instance?.UnlockFruit(fruitScript1.fruitIndex - FruitBarUIManager.instance.StartIndex);

        // Play sound
        if (!sfxMuted && mergeSound != null)
        {
            AudioSource.PlayClipAtPoint(mergeSound, spawnPos);
        }

        // Bounce animation
        StartCoroutine(BounceEffect(newFruit.transform, bounceScale, bounceDuration));

        // Assign index and onSettled callback
        Fruit fruitScript = newFruit.GetComponent<Fruit>();
        if (fruitScript != null)
        {
            fruitScript.fruitIndex = nextIndex;
            fruitScript.onSettled = () =>
            {
                FruitDropperController dropper = FindObjectOfType<FruitDropperController>();
                dropper?.StartCoroutine(dropper.EnableDropAfterDelay());
            };
        }
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
}
