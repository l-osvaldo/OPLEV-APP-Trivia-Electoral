using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideProgressBarBehaviour : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    private RectTransform _rectTransform;
    private Vector3 _visiblePosition;
    private Vector3 _hiddenPosition;
    private bool _initialized = false;

    void OnEnable()
    {
        if (!_initialized)
        {
            return;
        }

        EnableProgressBar();
    }

    // Start is called before the first frame update
    void Start()
    {
        _canvasGroup = gameObject.GetComponent<CanvasGroup>();
        _rectTransform = gameObject.GetComponent<RectTransform>();
        _visiblePosition = new Vector3(20, 20, 0);
        _hiddenPosition = new Vector3(20,-10,0);
        _initialized = true;
    }


    public void EnableProgressBar()
    {
        //LeanTween.alphaCanvas(_canvasGroup, 1, 0.5f).setFrom(0).setEaseInCubic();
        //LeanTween.move(_rectTransform, _visiblePosition, 0.5f).setFrom(_hiddenPosition).setEaseInOutBack();
    }

    public void DisableProgressBar()
    {
        //LeanTween.alphaCanvas(_canvasGroup, 0, 0.5f).setFrom(1).setEaseInCubic();
        //LeanTween.move(_rectTransform, _hiddenPosition, 0.5f).setFrom(_visiblePosition).setEaseInOutBack();
    }
}
