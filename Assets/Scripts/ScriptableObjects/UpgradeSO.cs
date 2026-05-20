using UnityEngine;

[CreateAssetMenu()]
public class UpgradeSO : ScriptableObject
{
    public string displayName;
    [TextArea]
    public string description;
    public Sprite thumbnailSprite;
    
    [Header("Cost Per Difficulty")]
    public int easyCost;
    public int normalCost;
    public int hardCost;
    
    [Header("Upgrade Values")]
    public UpgradeManager.UpgradeType upgradeType;
    [Range(0f, 2f)] public float speedMultiplier = 1f;   // Sharp Knives, Hot Stoves
    [Range(0f, 2f)] public float timeMultiplier = 1f;    // Warming Lamps
    [Range(0f, 1f)] public float tipPercentage = 0f;     // Generous Tips
    public int bonusOrderSlots = 0;                      // Bigger Board
    
    public int GetCost(string difficultyKey) => difficultyKey switch
    {
        "Easy"   => easyCost,
        "Normal" => normalCost,
        "Hard"   => hardCost,
        _        => normalCost
    };
}
