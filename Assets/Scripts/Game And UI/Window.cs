using UnityEngine;
using UnityEngine.UI;

public abstract class Window : MonoBehaviour
{
    [SerializeField] private CanvasGroup _windowGroup;
    [SerializeField] private Button _actionButton;

    private float _closeAlpha = 0f;
    private float _openAlpha = 1f;

    public CanvasGroup WindowGroup => _windowGroup;
    protected Button ActionButton => _actionButton;

    private void OnEnable()
    {
        _actionButton.onClick.AddListener(OnButtonClick);
    }

    private void OnDisable()
    {
        _actionButton.onClick.RemoveListener(OnButtonClick);
    }

    public virtual void Close()
    {
        _windowGroup.alpha = _closeAlpha;
        _windowGroup.interactable = false;
        _windowGroup.blocksRaycasts = false;
        _actionButton.interactable = false;
    }

    public virtual void Open()
    {
        gameObject.SetActive(true);
        _windowGroup.alpha = _openAlpha;
        _windowGroup.interactable = true;
        _windowGroup.blocksRaycasts = true;
        _actionButton.interactable = true;
    }

    public abstract void OnButtonClick();    
}
