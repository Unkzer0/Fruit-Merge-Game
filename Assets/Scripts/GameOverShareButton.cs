using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GameOverShareButton : MonoBehaviour
{
    [Header("Dependencies")]
    public Button shareButton;
    public ScoreManager scoreManager;

    [Header("Audio")]
    [SerializeField] private AudioClip buttonClickSound;

    private bool isProcessing = false;
    private bool isFocus = false;

    private void Start()
    {
        if (shareButton != null)
            shareButton.onClick.AddListener(() =>
            {
                PlayClickSound();
                ShareText(); 
            }); ;
    }
    private void PlayClickSound()
    {
        if (buttonClickSound != null && Camera.main != null)
        {
            AudioSource.PlayClipAtPoint(buttonClickSound, Camera.main.transform.position);
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        isFocus = focus;
    }

    private void ShareText()
    {
        if (!isProcessing)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            StartCoroutine(ShareTextOnAndroid());
#else
            Debug.Log("Text sharing is only implemented for Android.");
#endif
        }
        Debug.Log($"Highscore: {ScoreManager.instance.GetHighScore()}");
    }

#if UNITY_ANDROID && !UNITY_EDITOR
    private IEnumerator ShareTextOnAndroid()
    {
        isProcessing = true;

        int highScore = scoreManager != null ? scoreManager.GetHighScore() : 0;
        string subject = "I have set a new high score in Fruit Merge!";
        string message = $"Just nailed a solid {highScore} in Fruit Merge. Think you can beat me?\n\n" +
                         "Get the game from the link below:\n" +
                         "https://play.google.com/store/apps/details?id=com.InfiniteSwipeLabs.FruitMerge";

        // Create share intent
        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
        intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
        intentObject.Call<AndroidJavaObject>("setType", "text/plain");

        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), subject);
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), message);

        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        AndroidJavaObject chooser = intentClass.CallStatic<AndroidJavaObject>(
            "createChooser", intentObject, "Share your high score");

        activity.Call("startActivity", chooser);

        // Wait until app regains focus
        yield return new WaitUntil(() => isFocus);
        isProcessing = false;
    }
#endif
}
