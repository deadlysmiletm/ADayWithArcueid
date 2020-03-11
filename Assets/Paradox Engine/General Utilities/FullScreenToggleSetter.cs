using UnityEngine;
using UnityEngine.UI;

public class FullScreenToggleSetter : MonoBehaviour
{
    [SerializeField] private Toggle _fullscreenToggle = null;


    private void Start()
    {
        if (_fullscreenToggle == null)
            SetToggleComponent();

        _fullscreenToggle.isOn = Screen.fullScreen;
    }

    public void SetFullscreen(bool isFullScreen)
    {
        if (_fullscreenToggle == null)
            SetToggleComponent();

        Screen.fullScreen = isFullScreen;
    }

    public bool GetFullscreen()
    {
        if (_fullscreenToggle == null)
            SetToggleComponent();

        return Screen.fullScreen;
    }


    private void SetToggleComponent() => _fullscreenToggle = GetComponent<Toggle>();
}
