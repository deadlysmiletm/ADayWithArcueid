using System.Collections.Generic;
using UnityEngine;

public class LanguageDropdownSetter : DropdownLocalizationSetter
{
    [Tooltip("Describe all the language you use with is filter name, in the same order in the options of the dropdown.")]
    public List<string> Languages = new List<string>();


    protected override void Start()
    {
        _dropdown.value = Languages.IndexOf(_manager.currentLanguage);
        base.Start();
    }


    public void SetLanguage(int value) => _manager.UpdateLanguage(Languages[value]);
}
