using UnityEngine;
using UnityEngine.Audio;

public class VolumeControlScript : MonoBehaviour
{
    public AudioMixer mainMixer;

    public void SetVolume(float sliderValue)
    {
        mainMixer.SetFloat("Volume", Mathf.Log10(sliderValue) * 20);
    }
}