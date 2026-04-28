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

    private int CalculateStars() {
        int totalAttemptsAmount = DeliveryManager.Instance.GetTotalAttemptsAmount();
        int successRecipesAmount = DeliveryManager.Instance.GetSuccessRecipesAmount();
        int totalMoneyEarned = DeliveryManager.Instance.GetTotalMoneyEarned();
        
        if (totalAttemptsAmount == 0) return 3; // TODO: Change to 0 when not testing

        float accuracy = successRecipesAmount / (float)totalAttemptsAmount;

        bool threeStarGate = accuracy >= 0.85f && successRecipesAmount >= 5 && totalMoneyEarned >= 280;
        bool twoStarGate   = accuracy >= 0.65f && successRecipesAmount >= 3 && totalMoneyEarned >= 120;
        bool oneStarGate   = accuracy >= 0.40f && successRecipesAmount >= 1 && totalMoneyEarned >= 20;

        Debug.Log("Accuracy: " + accuracy + "\nSuccess: " + successRecipesAmount + "\nMoney: " + totalMoneyEarned);
        
        if (threeStarGate) return 3;
        if (twoStarGate)   return 2;
        if (oneStarGate)   return 1;
        return 0;
    }
}
