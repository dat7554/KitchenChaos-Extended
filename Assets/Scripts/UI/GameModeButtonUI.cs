using UnityEngine;
using UnityEngine.EventSystems;

public class GameModeButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] private LevelSelectorUI levelSelectorUI;
    [SerializeField] private GameModeSO gameModeSO;

    public GameModeSO GetGameModeSO()
    {
        return gameModeSO;
    }

    public void OnPointerEnter(PointerEventData eventData) => levelSelectorUI.OnButtonHovered(this);
    public void OnPointerExit(PointerEventData eventData) => levelSelectorUI.OnButtonUnhovered(this);
    public void OnSelect(BaseEventData eventData) => levelSelectorUI.OnButtonSelected(this);
    public void OnDeselect(BaseEventData eventData) => levelSelectorUI.OnButtonDeselected(this);
}
