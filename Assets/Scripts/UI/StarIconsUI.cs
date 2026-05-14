using System.Collections;
using UnityEngine;

public class StarIconsUI : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(AnimateStars());
    }

    private IEnumerator AnimateStars()
    {
        int filledStars = CalculateStars(out _);
        int index = 0;
        foreach (Transform child in transform)
        {
            if (index == filledStars) break;
            
            child.GetComponent<StarIconSingleUI>().PlayFillAnimation();
            
            yield return new WaitForSeconds(0.4f);
            index++;
        }
    }

    public static int CalculateStars(out int nextStarMoney)
    {
        int totalMoneyEarned = DeliveryManager.Instance.GetTotalMoneyEarned();
        GameModeSO gameModeSO = GameModeSelector.GetGameModeSO();

        if (totalMoneyEarned >= gameModeSO.threeStarThreshold) { nextStarMoney = 0; return 3; }
        if (totalMoneyEarned >= gameModeSO.twoStarThreshold)   { nextStarMoney = gameModeSO.threeStarThreshold; return 2; }
        if (totalMoneyEarned >= gameModeSO.oneStarThreshold)   { nextStarMoney = gameModeSO.twoStarThreshold; return 1; }

        nextStarMoney = gameModeSO.oneStarThreshold;
        return 0;
    }
}
