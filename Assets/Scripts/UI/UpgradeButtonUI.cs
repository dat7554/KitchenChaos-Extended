using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] private ShopUI shopUI;
    [SerializeField] private UpgradeSO upgradeSO;

    [SerializeField] private Image iconImage;
    [SerializeField] private Image lockImage;
    
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        
        _button.onClick.AddListener(() => shopUI.OnButtonClicked(this));
        
        iconImage.sprite = upgradeSO.thumbnailSprite;
    }

    public UpgradeSO GetUpgradeSO()
    {
        return upgradeSO;
    }
    
    public void OnPointerEnter(PointerEventData eventData) => shopUI.OnButtonHovered(this);
    public void OnPointerExit(PointerEventData eventData) => shopUI.OnButtonUnhovered(this);
    public void OnSelect(BaseEventData eventData) => shopUI.OnButtonSelected(this);
    public void OnDeselect(BaseEventData eventData) => shopUI.OnButtonDeselected(this);
    
    public void UpdateState(int remainingBudget, bool alreadyOwned)
    {
        bool canAfford = remainingBudget >= upgradeSO.GetCost(GameModeSelector.GetGameModeSO().difficultyKey);
        _button.interactable = canAfford && !alreadyOwned;
        iconImage.color = (canAfford && !alreadyOwned) ? Color.white : Color.gray;
        lockImage.gameObject.SetActive(!canAfford && !alreadyOwned);
    }
}
