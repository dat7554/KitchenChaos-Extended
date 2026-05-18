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
    
    public int GetCost(string difficultyKey) => difficultyKey switch
    {
        "Easy"   => easyCost,
        "Normal" => normalCost,
        "Hard"   => hardCost,
        _        => normalCost
    };
}
