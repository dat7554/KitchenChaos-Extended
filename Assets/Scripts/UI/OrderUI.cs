using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderUI : MonoBehaviour
{
    private static readonly int Shake = Animator.StringToHash("Shake");
    
    [SerializeField] private TextMeshProUGUI orderNameText;
    [SerializeField] private Transform iconContainer;
    [SerializeField] private Transform iconTemplate;
    [SerializeField] private Slider fillSlider;
    [SerializeField] private Image fillImage;

    private Order _order;
    private Animator _animator;
    private bool _isShaking;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        iconTemplate.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (_order != null)
        {
            _order.OnProgressChanged -= Order_OnProgressChanged;
            _order.OnDestroyed -= Order_OnDestroyed;
        }
    }

    public void SetOrder(Order order)
    {
        _order = order;
        SetRecipeSO(order.GetRecipeSO());
        
        _order.OnProgressChanged += Order_OnProgressChanged;
        _order.OnDestroyed += Order_OnDestroyed;
    }
    
    private void SetRecipeSO(RecipeSO recipeSO)
    {
        orderNameText.text = recipeSO.recipeName;

        foreach (Transform child in iconContainer)
        {
            if (child == iconTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (var kitchenObjectSO in recipeSO.KitchenObjectSOList)
        {
            Transform iconTransform = Instantiate(iconTemplate, iconContainer);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<Image>().sprite = kitchenObjectSO.Sprite;
        }
    }
    
    private void Order_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        fillSlider.value = e.NormalizedProgress;
        fillImage.color = GetSliderColor(e.NormalizedProgress);

        if (e.NormalizedProgress <= 0.25f && !_isShaking)
        {
            _animator.SetTrigger(Shake);
            _isShaking = true;
        }
    }
    
    private void Order_OnDestroyed(object sender, EventArgs e)
    {
        Destroy(gameObject);
    }
    
    private Color GetSliderColor(float progress)
    {
        if (progress >= 0.5f)
        {
            // remap 0.5–1.0 → 0.0–1.0
            float t = (progress - 0.5f) / 0.5f;
            return Color.Lerp(Color.yellow, Color.darkGreen, t);
        }
        else
        {
            // remap 0.0–0.5 → 0.0–1.0
            float t = progress / 0.5f;
            return Color.Lerp(Color.red, Color.yellow, t);
        }
    }
}
