using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

// Fruit Merge

public class Leaderboard : MonoBehaviour
{
    //[SerializeField] private ScoreManager scoreManager;

    public void ShowLeaderboardUI()
    {
        if (Social.localUser.authenticated)
        {
            PlayGamesPlatform.Instance.ShowLeaderboardUI();
        }
        else
        {
            Debug.Log("User not signed in!");
        }
    }

    public void ReportScoreInLeaderboard(int score)
    {
        if (Social.localUser.authenticated)
        {
            Social.ReportScore(score, GPGSIds.leaderboard_best_score, success =>
            {
                Debug.Log(success ? "Success" : "No Success");
            });
        }
        else
        {
            Debug.Log("User not signed in! Score not submitted.");
        }
    }

    public void PostLeaderboard()
    {
        //ReportScoreInLeaderboard(scoreManager.GetHighScore());
    }
}
