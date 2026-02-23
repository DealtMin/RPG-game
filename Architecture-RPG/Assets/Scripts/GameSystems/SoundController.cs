using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundController : MonoBehaviour
{
    [SerializeField] private AudioMixer masterMixer;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private float noVolumeValue;

    private void Awake()
    {
        masterMixer.GetFloat("musicVolume", out float currVolume);
        if (currVolume==noVolumeValue) volumeSlider.value = 0;
        else volumeSlider.value =  Mathf.Lerp(1, 0, currVolume/-20);

    }

    public void SetMasterVolume (Slider slider)
    {
        if (slider.value==0)
            masterMixer.SetFloat("musicVolume", noVolumeValue);
        else    
            masterMixer.SetFloat("musicVolume", Mathf.Lerp(-20, 0, slider.value));
    }

}
