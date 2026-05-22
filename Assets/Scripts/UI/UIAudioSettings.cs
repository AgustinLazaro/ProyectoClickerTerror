using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UIAudioSettings : MonoBehaviour
{
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [SerializeField] AudioMixer masterMixer;

    private void Awake()
    {
        masterSlider.onValueChanged.AddListener(delegate { SetVolume("masterVolume", masterSlider.value); });
        musicSlider.onValueChanged.AddListener(delegate { SetVolume("musicVolume", musicSlider.value); });
        sfxSlider.onValueChanged.AddListener(delegate { SetVolume("sfxVolume", sfxSlider.value); });
    }

    private void SetVolume(string parameterName, float _value)
    {
        if (_value < 1) _value = 0.001f;

        PlayerPrefs.SetFloat(parameterName, _value);

        //Conversion a decibelios
        masterMixer.SetFloat(parameterName, Mathf.Log10(_value / 100) * 20f);
    }

    private void Start()
    {
        SetVolume("masterVolume", PlayerPrefs.GetFloat("masterVolume", 100f));
        SetVolume("musicVolume", PlayerPrefs.GetFloat("musicVolume", 100f));
        SetVolume("sfxVolume", PlayerPrefs.GetFloat("sfxVolume", 100f));
    }

    private void OnDestroy()
    {
        masterSlider.onValueChanged.RemoveAllListeners();
        musicSlider.onValueChanged.RemoveAllListeners();
        sfxSlider.onValueChanged.RemoveAllListeners();
    }
}
