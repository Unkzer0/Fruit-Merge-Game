using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeManager : MonoBehaviour
{
    public static MergeManager instance;

    [SerializeField] private AudioClip mergeSound;
    [SerializeField] private float bounceScale = 1.2f;
    [SerializeField] private float bounceDuration = 0.2f;
    private bool sfxMuted = false;

    private void Awake()
    {
   
        sfxMuted = PlayerPrefs.GetInt("SFXMuted", 0) == 1;

        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void ToggleSFX()
    {
        sfxMuted = !sfxMuted;
        PlayerPrefs.SetInt("SFXMuted", sfxMuted ? 1 : 0);
    }

    public bool IsSFXMuted() => sfxMuted;

    public void MergeFruits(int currentIndex, GameObject fruit1, GameObject fruit2, Vector3 spawnPos)
    {
        Fruit fruitScript1 = fruit1.GetComponent<Fruit>();

        if (fruitScript1 != null && fruitScript1.fruitData != null)
        {
            int points = fruitScript1.fruitData.scoreValue;
            ScoreManager.instance.AddScore(points);
        }

        Destroy(fruit1);
        Destroy(fruit2);

        int nextIndex = currentIndex + 1;
        if (nextIndex < FruitSelector.instance.Fruits.Length)
        {
            GameObject newFruit = Instantiate(FruitSelector.instance.Fruits[nextIndex], spawnPos, Quaternion.identity);
            newFruit.transform.localScale = Vector3.one; // Always scale to 1,1,1

            //  Unlock in UI if not already unlocked
            FruitBarUIManager.instance?.UnlockFruit(fruitScript1.fruitIndex - FruitBarUIManager.instance.StartIndex);


            //  Play merge sound
            if (!sfxMuted && mergeSound != null)
            {
                AudioSource.PlayClipAtPoint(mergeSound, spawnPos);
            }

            StartCoroutine(BounceEffect(newFruit.transform, bounceScale, bounceDuration));

            // Setup merged fruit
            Fruit fruitScript = newFruit.GetComponent<Fruit>();
            if (fruitScript != null)
            {
                fruitScript.fruitIndex = nextIndex;

                fruitScript.onSettled = () =>
                {
                    FruitDropperController dropper = FindObjectOfType<FruitDropperController>();
                    if (dropper != null)
                    {
                        dropper.StartCoroutine(dropper.EnableDropAfterDelay());
                    }
                };
            }
        }
    }

    private IEnumerator BounceEffect(Transform target, float scaleMultiplier, float duration)
    {
        if (target == null) yield break;

        Vector3 originalScale = Vector3.one;
        Vector3 targetScale = originalScale * scaleMultiplier;

        float timer = 0f;

        // Scale up
        while (timer < duration)
        {
            if (target == null) yield break;

            float t = timer / duration;
            target.localScale = Vector3.Lerp(originalScale, targetScale, t);
            timer += Time.deltaTime;
            yield return null;
        }

        if (target != null)
            target.localScale = targetScale;

        timer = 0f;

        // Scale down
        while (timer < 0.1f)
        {
            if (target == null) yield break;

            float t = timer / 0.1f;
            target.localScale = Vector3.Lerp(targetScale, originalScale, t);
            timer += Time.deltaTime;
            yield return null;
        }

        if (target != null)
            target.localScale = originalScale;
    }
}
