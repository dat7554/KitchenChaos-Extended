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
        int filledStars = CalculateStars();
        int index = 0;
        foreach (Transform child in transform)
        {
            if (index == filledStars) break;
            
            child.GetComponent<StarIconSingleUI>().PlayFillAnimation();
            
            yield return new WaitForSeconds(0.4f);
            index++;
        }
    }

    private int CalculateStars() 
    {
        int totalMoneyEarned = DeliveryManager.Instance.GetTotalMoneyEarned();
        GameModeSO gameModeSO = GameModeSelector.GetGameModeSO();

        if (totalMoneyEarned >= gameModeSO.threeStarThreshold) return 3;
        if (totalMoneyEarned >= gameModeSO.twoStarThreshold) return 2;
        if (totalMoneyEarned >= gameModeSO.oneStarThreshold)  return 1;
        return 0;
    }
}
