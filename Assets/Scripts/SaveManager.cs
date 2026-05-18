using UnityEngine;

public static class SaveManager
{
    private static string ScoreKey(string difficulty) => $"{difficulty}_bestScore";
    private static string BudgetKey(string difficulty) => $"{difficulty}_budget";

    public static void SaveResult(string difficulty, int score)
    {
        int currentBestScore = PlayerPrefs.GetInt(ScoreKey(difficulty), 0);
        
        if (score > currentBestScore)
        {
            PlayerPrefs.SetInt(ScoreKey(difficulty), score); 
        }
        
        PlayerPrefs.Save();
    }
    
    public static void SaveBudget(string difficulty, int amount)
    {
        PlayerPrefs.SetInt(BudgetKey(difficulty), amount);
        PlayerPrefs.Save();
    }

    public static int GetBestScore(string difficulty) => PlayerPrefs.GetInt(ScoreKey(difficulty), 0);
    public static int GetBudget(string difficulty) => PlayerPrefs.GetInt(BudgetKey(difficulty), 0);
}
