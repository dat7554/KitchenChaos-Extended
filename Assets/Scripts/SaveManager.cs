using UnityEngine;

public static class SaveManager
{
    private static string ScoreKey(string difficulty) => $"{difficulty}_bestScore";

    public static void SaveResult(string difficulty, int score)
    {
        int currentBestScore = PlayerPrefs.GetInt(ScoreKey(difficulty), 0);
        
        if (score > currentBestScore)
        {
            PlayerPrefs.SetInt(ScoreKey(difficulty), score); 
        }
        
        PlayerPrefs.Save();
    }

    public static int GetBestScore(string difficulty) => PlayerPrefs.GetInt(ScoreKey(difficulty), 0);
}
