using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeIconUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Transform background;
    [SerializeField] private TextMeshProUGUI infoText;

    private Image _upgradeIconImage;
    
    private void Awake()
    {
        _upgradeIconImage = GetComponent<Image>();
        
        HideInfo();
    }

    public void SetUpgradeSO(UpgradeSO upgradeSO)
    {
        infoText.text = upgradeSO.displayName;
        _upgradeIconImage.sprite = upgradeSO.thumbnailSprite;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowInfo();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideInfo();
    }

    private void ShowInfo()
    {
        background.gameObject.SetActive(true);
        infoText.gameObject.SetActive(true);
    }

    private void HideInfo()
    {
        background.gameObject.SetActive(false);
        infoText.gameObject.SetActive(false);
    }
}
