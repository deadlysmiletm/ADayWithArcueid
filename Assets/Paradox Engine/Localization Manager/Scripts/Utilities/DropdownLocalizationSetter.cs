using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ParadoxEngine.Localization;
using System;
using System.Linq;

public class DropdownLocalizationSetter : MonoBehaviour
{
    [SerializeField] protected LocalizationManager _manager = null;
    [SerializeField] protected Dropdown _dropdown = null;
    [SerializeField] protected Text _dropdownLabel = null;
    [SerializeField] [HideInInspector] private byte[] _localizationID;

    protected void Awake()
    {
        if (_manager == null)
            _manager = GameObject.FindObjectOfType<LocalizationManager>();

        if (_dropdown == null)
            GetDropdownComponent();

        if (_dropdownLabel == null)
            _dropdownLabel = GetComponentInChildren<Text>();

        _manager.OnUpdateTranslation += Translate;
    }


    public int GetDropdownValue()
    {
        if (_dropdown == null)
            GetDropdownComponent();

        return _dropdown.value;
    }

    public void SetDropdownValue(int index)
    {
        if (_dropdown == null)
            GetDropdownComponent();

        _dropdown.value = index;
    }

    public IEnumerable<string> GetDropdownOptions()
    {
        if (_dropdown == null)
            GetDropdownComponent();

        return _dropdown.options.Select(x => x.text);
    }

    public Guid GetLocalizationID()
    {
        if (_localizationID.Length == 0)
            _localizationID = Guid.NewGuid().ToByteArray();

        return new Guid(_localizationID);
    }

    public void RebuildLocalizationID() => _localizationID = Guid.NewGuid().ToByteArray();

    protected void GetDropdownComponent() => _dropdown = GetComponent<Dropdown>();

    protected virtual void Start() => Translate();

    protected virtual void Translate()
    {
        var data = _manager.GetDropdownTranslation(GetLocalizationID());

        for (int i = 0; i < _dropdown.options.Count; i++)
            _dropdown.options[i].text = data[i];

        _dropdownLabel.text = _dropdown.options[_dropdown.value].text;
    }
}