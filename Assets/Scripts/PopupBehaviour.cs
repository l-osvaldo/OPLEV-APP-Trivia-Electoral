using UnityEngine;

/// <summary>
///     Handles Popup Scale-and-Fade In and Out animations through LeanTween package.
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
public class PopupBehaviour : MonoBehaviour
{
    /// <summary>
    ///     UI Canvas or parent gameObject that wraps the Popup gameObject and its possible parent objects.
    ///     This gameObject is to be disabled when all animations of HidePopup() are completed.
    /// </summary>
    public GameObject PopupCanvas;

    /// <summary>
    ///     UI Panel or parent gameObject that wraps all the objects that need alpha animation.
    ///     This gameObject ensures it and all objects within will have an alpha animation while not affecting
    ///     the child gameObjects' colors.
    /// </summary>
    public CanvasGroup PopupPanel;

    /// <summary>
    ///     PopupId is used to call the corresponding Popup's actions when events are fired.
    /// </summary>
    public PopupEnum PopupId;

    /// <summary>
    ///     Set to true to not disable the object on Start.
    /// </summary>
    public bool StartEnabled;

    /// <summary>
    ///     UI Panel or parent gameObject that wraps all the objects that need alpha animation.
    ///     This gameObject ensures it and all objects within will have an alpha animation while not affecting
    ///     the child gameObjects' colors.
    /// </summary>
    private CanvasGroup _canvasGroup;

    /// <summary>
    ///     Visible scale
    /// </summary>
    private Vector3 _visibleScale = Vector3.one;

    /// <summary>
    ///     Invisible scale
    /// </summary>
    private Vector3 _invisibleScale = Vector3.zero;

    /// <summary>
    ///     Calls ShowPopup when gameObject is enabled.
    /// </summary>
    void OnEnable()
    {
        OpenPopup();
    }

    void Start()
    {
        PopupEventManager.Instance.OnPopupClose += OnClosePopup;
        _canvasGroup = gameObject.GetComponent<CanvasGroup>();

        if (StartEnabled)
        {
            return;
        }
        PopupCanvas.SetActive(false);
    }

    /// <summary>
    ///     Scales up gameobject from 0 to 1 in a spring-like bounce animation curve of 0.35 seconds.
    ///     At the same time it increases opacity or alpha value of the canvas group from 0 to 1 in
    ///     an exponential curve in 0.15 seconds.
    /// </summary>
    public void OpenPopup()
    {
        LeanTween.alphaCanvas(PopupPanel, 1, 0.15f).setFrom(0).setEaseInExpo();
        LeanTween.scale(gameObject, _visibleScale, 0.35f).setFrom(_invisibleScale).setEaseSpring();

        if (_canvasGroup)
        {
            LeanTween.alphaCanvas(_canvasGroup, 1, 0.15f).setFrom(0).setEaseInExpo();
        }
    }

    /// <summary>
    ///     Calls ClosePopup if the event's popupId matches the gameObject's PopupId
    /// </summary>
    public void OnClosePopup(PopupEnum popupId)
    {
        if (PopupId != popupId)
        {
            return;
        }

        ClosePopup();
    }

    /// <summary>
    ///     Scales up gameobject from 1 to 1 to 1.1, to simulate a reverse spring-like bounce animation,
    ///     in 0.35 seconds. Meanwhile, it decreases opacity or alpha value of the canvas group to zero 
    ///     in an exponential curve in 0.25 seconds. Finally it calls DisableObject after 0.26 seconds to
    ///     make sure the object will not disappear while still visible.
    /// </summary>
    public void ClosePopup()
    {
        LeanTween.scale(gameObject, _visibleScale * 1.1f, 0.10f).setEaseInBack();
        LeanTween.scale(gameObject, _invisibleScale, 0.25f).setFrom(_visibleScale * 1.1f).setEaseInBack();
        if (_canvasGroup)
        {
            LeanTween.alphaCanvas(_canvasGroup, 0, 0.25f).setEaseInBack();
        }
        LeanTween.alphaCanvas(PopupPanel, 0, 0.35f).setEaseOutBack().setOnComplete(DisableOjbect);
    }

    /// <summary>
    ///     Disables the PopupWrapper object.
    /// </summary>
    void DisableOjbect()
    {
        PopupCanvas.SetActive(false);
    }

    private void OnDestroy()
    {
        PopupEventManager.Instance.OnPopupClose -= OnClosePopup;
    }
}
