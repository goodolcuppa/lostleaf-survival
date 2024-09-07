using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private string mixerParameter;

    private Slider mixerSlider;

    public void SetLevel(float value) {
        mixer.SetFloat(mixerParameter, Mathf.Log10(value) * 20f);
        PlayerPrefs.SetFloat(mixerParameter, value);
    }

    void OnEnable() {
        mixerSlider = GetComponent<Slider>();
        float value = PlayerPrefs.GetFloat(mixerParameter, 0.5f);
        mixer.SetFloat(mixerParameter, Mathf.Log10(value) * 20f);
        mixerSlider.value = value;
    }
}
