using UnityEngine;
using UnityEngine.Audio;

public class LoadConfig : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private string mixerParameter;

    void Start() {
        float value = PlayerPrefs.GetFloat(mixerParameter, 0.5f);
        mixer.SetFloat(mixerParameter, Mathf.Log10(value) * 20f);
    }
}
