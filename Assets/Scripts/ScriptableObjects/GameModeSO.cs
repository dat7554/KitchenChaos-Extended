using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class GameModeSO : ScriptableObject
{
    [Serializable]
    public struct WeightedRecipe
    {
        public RecipeSO recipeSO;
        [Range(1, 10)] public int weight;
    }
    
    [Header("Spawn Pressure")]
    public float spawnOrderInterval = 4f;
    public int maxOrders = 4;
    public float orderTimeMultiplier = 1f;   // passed to each Order

    [Header("Recipe Pool")]
    public List<WeightedRecipe> recipePool;      // assign different SO list per difficulty

    [Header("Scoring")]
    public int oneStarThreshold = 20;
    public int twoStarThreshold = 120;
    public int threeStarThreshold = 280;
}
