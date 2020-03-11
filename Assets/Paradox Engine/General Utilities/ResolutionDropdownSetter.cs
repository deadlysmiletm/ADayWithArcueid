using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionDropdownSetter : MonoBehaviour
{
    [SerializeField] Dropdown _resolutionDropdown = null;
    private Resolution[] _resolutions = System.Array.Empty<Resolution>();


    private void Start()
    {
        if (_resolutionDropdown == null)
            _resolutionDropdown = GetComponent<Dropdown>();

        if (_resolutions.Length == 0)
            CalculateResolutions();
    }

    public void SetResolution(int index)
    {
        if (_resolutionDropdown == null)
            SetDropdownComponent();

        if (_resolutions.Length == 0)
            CalculateResolutions();

        Resolution resolution = _resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public int GetResolution()
    {
        if (_resolutionDropdown == null)
            SetDropdownComponent();

        if (_resolutions.Length == 0)
            CalculateResolutions();

        return _resolutionDropdown.value;
    }

    private void CalculateResolutions()
    {
        _resolutions = Screen.resolutions;
        _resolutionDropdown.ClearOptions();

        int currentResolution = 0;
        Resolution current = Screen.currentResolution;
        List<string> options = new List<string>();

        for (int i = 0; i < _resolutions.Length; i++)
        {
            options.Add($"{_resolutions[i].width} x {_resolutions[i].height}");

            if (_resolutions[i].width == current.width && _resolutions[i].height == current.height)
                currentResolution = i;
        }

        _resolutionDropdown.AddOptions(options);
        _resolutionDropdown.value = currentResolution;
        _resolutionDropdown.RefreshShownValue();
    }

    private void SetDropdownComponent() => _resolutionDropdown = GetComponent<Dropdown>();
}
