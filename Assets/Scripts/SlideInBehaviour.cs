using UnityEngine;

public class SlideInBehaviour : MonoBehaviour
{
    public CanvasGroup _canvasGroup;

    private GameObject _parent;
    private RectTransform _rectTransform;
    private Vector3 _visiblePosition;
    private Vector3 _hiddenPosition;
    private float _width;
    private bool _initialized = false;

    void OnEnable()
    {
        if (!_initialized)
        {
            return;
        }

        EnablePanel();
    }

    // Start is called before the first frame update
    void Start()
    {
        _parent = gameObject.transform.parent.gameObject;
        _rectTransform = gameObject.GetComponent<RectTransform>();
        _width = _rectTransform.sizeDelta.x;
        _visiblePosition = Vector3.zero;
        _hiddenPosition = new Vector3(-_width, 0, 0);
        _initialized = true;

        EnablePanel();
    }


    public void EnablePanel()
    {
        LeanTween.alphaCanvas(_canvasGroup, 1, 0.25f).setFrom(0).setEaseInCubic();
        LeanTween.move(_rectTransform, _visiblePosition, 0.25f).setFrom(_hiddenPosition).setEaseInSine();
    }

    public void DisablePanel()
    {
        LeanTween.alphaCanvas(_canvasGroup, 0, 0.25f).setFrom(1).setEaseInCubic();
        LeanTween.move(_rectTransform, _hiddenPosition, 0.25f).setFrom(_visiblePosition).setEaseInSine().setOnComplete(DisableParent);
    }

    private void DisableParent()
    {
        _parent.SetActive(false);
    }
}
