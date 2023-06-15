using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingScript : MonoBehaviour
{
    public TMP_Dropdown GraphicDropdown;
    public AudioMixer musicMixer;
    public void Start()
    {
        string[] graphicsOptions = QualitySettings.names;
        GraphicDropdown.AddOptions(new List<string>(graphicsOptions));
        GraphicDropdown.value = QualitySettings.GetQualityLevel();
    }
    public void SetAudioVolume(float volume)
    {
        musicMixer.SetFloat("audioVolume", volume);
        print(volume);
    }
    public void SetMusicVolume(float volume)
    {
        musicMixer.SetFloat("musicVolume", volume);
        print(volume);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = !Screen.fullScreen;
        print(Screen.fullScreen);
        
    }

    public void SetGraphic (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
}
