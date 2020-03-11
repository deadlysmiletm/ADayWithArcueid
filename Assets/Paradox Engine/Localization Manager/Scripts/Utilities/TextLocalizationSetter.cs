using UnityEngine;
using UnityEngine.UI;
using ParadoxEngine.Localization;
using System;

public class TextLocalizationSetter : MonoBehaviour
{
    [SerializeField] private LocalizationManager _manager = null;
    [SerializeField] private Text _textComponent = null;
    [SerializeField] [HideInInspector] private byte[] _localizationID;

    private void Awake()
    {
        if (_manager == null)
            _manager = GameObject.FindObjectOfType<LocalizationManager>();

        if (_textComponent == null)
            _textComponent = GetComponent<Text>();

        _manager.OnUpdateTranslation += Translate;
    }
    public Guid GetLocalizationID()
    {
        if (_localizationID.Length == 0)
            _localizationID = Guid.NewGuid().ToByteArray();

        return new Guid(_localizationID);
    }

    public void RebuildLocalizationID() => _localizationID = Guid.NewGuid().ToByteArray();

    public string GetTextValue() => _textComponent.text;

    private void Start() => Translate();

    private void Translate() => _textComponent.text = _manager.GetTextTranslation(GetLocalizationID());
}
