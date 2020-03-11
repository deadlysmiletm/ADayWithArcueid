using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;


public class SoundSliderSetter : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer = null;
    [SerializeField] private Slider _slider = null;
    [SerializeField] private string _mixerChannel = string.Empty;


    void Start()
    {
        if (_audioMixer == null)
            SetSliderComponent();

        float audio = 0f;
        _audioMixer.GetFloat(_mixerChannel, out audio);
        _slider.value = audio;
    }

    public void SetVolume(float volume)
    {
        if (_audioMixer == null)
            SetSliderComponent();

        _audioMixer.SetFloat(_mixerChannel, volume);
    }

    public float GetVolume()
    {
        if (_audioMixer == null)
            SetSliderComponent();

        float temp = 0f;
        _audioMixer.GetFloat(_mixerChannel, out temp);

        return temp;
    }


    private void SetSliderComponent() => _slider = GetComponent<Slider>();
}
