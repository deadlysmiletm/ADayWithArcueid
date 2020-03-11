using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ParadoxEngine.Utilities.Parameters;
using ParadoxEngine.Localization;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] List<DropdownLocalizationSetter> _dropdownsData = new List<DropdownLocalizationSetter>();
    [SerializeField] ResolutionDropdownSetter _resolution = null;
    [SerializeField] List<Toggle> _toggles = new List<Toggle>();
    [SerializeField] FullScreenToggleSetter _fullScreen = null;
    [SerializeField] QualityDropdownSetter _quality = null;
    [SerializeField] List<SoundSliderSetter> _slidersData = new List<SoundSliderSetter>();


    public void SaveSettings()
    {
        var data = GetData();

        SaveHelper.SaveSettings(data);
    }

    public void LoadSettings()
    {
        SaveHelper.LoadSettings(x =>
        {
            for (int i = 0; i < _dropdownsData.Count; i++)
                _dropdownsData[i].SetDropdownValue(x.intData[i]);

            _resolution.SetResolution(x.intData[_dropdownsData.Count]);
            _quality.SetQuality(x.intData[_dropdownsData.Count + 1]);

            for (int i = 0; i < _toggles.Count; i++)
                _toggles[i].isOn = x.boolData[i];

            _fullScreen.SetFullscreen(x.boolData[_toggles.Count]);

            for (int i = 0; i < _slidersData.Count; i++)
                _slidersData[i].SetVolume(x.floatData[i]);

        }, this);
    }

    public SaveGlobal GetData()
    {
        SaveGlobal data = new SaveGlobal() { boolData = new List<bool>(), intData = new List<int>(), floatData = new List<float>(), stringData = new List<string>() };

        for (int i = 0; i < _dropdownsData.Count; i++)
            data.intData.Add(_dropdownsData[i].GetDropdownValue());

        if (_resolution == null)
            _resolution = GameObject.FindObjectOfType<ResolutionDropdownSetter>();

        if (_quality == null)
            _quality = GameObject.FindObjectOfType<QualityDropdownSetter>();

        data.intData.Add(_resolution.GetResolution());
        data.intData.Add(_quality.GetQuality());

        for (int i = 0; i < _toggles.Count; i++)
            data.boolData.Add(_toggles[i]);

        data.boolData.Add(_fullScreen.GetFullscreen());

        for (int i = 0; i < _slidersData.Count; i++)
            data.floatData.Add(_slidersData[i].GetVolume());

        return data;
    }
}
