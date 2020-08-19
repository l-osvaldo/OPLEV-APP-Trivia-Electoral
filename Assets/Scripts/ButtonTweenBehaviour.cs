using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTweenBehaviour : MonoBehaviour
{
    public int DisplayOrder;

    private CanvasGroup _canvasGroup;
    private Vector3 _startScale = new Vector3(0.1f,0.1f,0.1f);
    private Vector3 _endScale = new Vector3(1, 1, 1);
    private bool _initialized = false;
    private float _delay;

    void OnEnable()
    {
        if (!_initialized)
        {
            return;
        }

        EnableButton();
    }

    // Start is called before the first frame update
    void Start()
    {
        SceneEventManager.Instance.OnEnableButtons += EnableButton;
        SceneEventManager.Instance.OnDisableButtons += DisableButton;

        _canvasGroup = gameObject.GetComponent<CanvasGroup>();
        _delay = (float)DisplayOrder / 15;
        _initialized = true;

        EnableButton();
    }


    void EnableButton()
    {
        LeanTween.alphaCanvas(_canvasGroup, 0, 0);
        LeanTween.alphaCanvas(_canvasGroup, 1, 0.15f).setFrom(0).setEaseInExpo().setDelay(_delay);
        LeanTween.scale(gameObject, _startScale, 0);
        LeanTween.scale(gameObject, _endScale, 0.35f).setFrom(_startScale).setEaseSpring().setDelay(_delay);
    }

    void DisableButton()
    {
        LeanTween.alphaCanvas(_canvasGroup, 0, 0.15f).setFrom(1).setEaseInExpo().setDelay(_delay/2);
        LeanTween.scale(gameObject, _startScale, 0.35f).setFrom(_endScale).setEaseSpring().setDelay(_delay/2);
    }
}
