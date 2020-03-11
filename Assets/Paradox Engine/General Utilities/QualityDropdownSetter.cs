using UnityEngine;
using UnityEngine.UI;


public class QualityDropdownSetter : MonoBehaviour
{
    [SerializeField] private Dropdown _qualityDropdown;


    void Start()
    {
        if (_qualityDropdown == null)
            SetDropdownCompoent();

        _qualityDropdown.value = QualitySettings.GetQualityLevel();
    }


    public void SetQuality(int index)
    {
        if (_qualityDropdown == null)
            SetDropdownCompoent();

        QualitySettings.SetQualityLevel(index);
    }

    public int GetQuality()
    {
        if (_qualityDropdown == null)
            SetDropdownCompoent();

        return QualitySettings.GetQualityLevel();
    }

    private void SetDropdownCompoent() => _qualityDropdown = GetComponent<Dropdown>();
}
