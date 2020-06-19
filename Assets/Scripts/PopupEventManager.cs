using System;
using UnityEngine;

/// <summary>
///		PopupEventManager handles the UI popup events.
/// </summary>
public class PopupEventManager : MonoBehaviour
{
    public event Action<PopupEnum> OnPopupOpen;
    public event Action<PopupEnum> OnPopupClose;

    #region Singleton Setup

    /// <summary>
    ///     Singleton Setup.
    /// </summary>
    public static PopupEventManager Instance { get; private set; } = null;
    public void Awake()
    {
        // Setup the singleton instance.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    #endregion

    /// <summary>
    ///     PopupOpen Event.
    /// </summary>
    public void PopupOpen(PopupEnum popupId)
    {
        OnPopupOpen?.Invoke(popupId);
    }

    /// <summary>
    ///     PopupClose Event.
    /// </summary>
    public void PopupClose(PopupEnum popupId)
    {
        OnPopupClose?.Invoke(popupId);
    }
}
