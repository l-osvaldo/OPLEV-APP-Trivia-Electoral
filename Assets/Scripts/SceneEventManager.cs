using System;
using UnityEngine;

/// <summary>
///		PopupEventManager handles the UI popup events.
/// </summary>
public class SceneEventManager : MonoBehaviour
{
    public event Action OnEnableButtons;
    public event Action OnDisableButtons;

    #region Singleton Setup

    /// <summary>
    ///     Singleton Setup.
    /// </summary>
    public static SceneEventManager Instance { get; private set; } = null;
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
    public void EnableButtons()
    {
        OnEnableButtons?.Invoke();
    }

    /// <summary>
    ///     PopupClose Event.
    /// </summary>
    public void DisableButtons()
    {
        OnDisableButtons?.Invoke();
    }
}
