using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Settings: MonoBehaviour
{
  [SerializeField] private TMP_Dropdown _resolutionDropdown;
  [SerializeField] private TMP_Dropdown _qualityDropdown;

  private Resolution[] _resolutions;

  private void Start()
  {
    _resolutionDropdown.ClearOptions();
    List<string> options = new();

    _resolutions = Screen.resolutions;

    int currentResolutionIndex = 0;

    for (int i = 0; i < _resolutions.Length; i++)
    {
      string option = _resolutions[i].width + "x" + _resolutions[i].height + " " + _resolutions[i].refreshRateRatio + "Hz";
      options.Add(option);

      if (_resolutions[i].width == Screen.currentResolution.width && _resolutions[i].height == Screen.currentResolution.height)
      {
        currentResolutionIndex = i;
      }
    }

    _resolutionDropdown.AddOptions(options);
    _resolutionDropdown.RefreshShownValue();
    LoadSettings(currentResolutionIndex);
  }

  public void SetFullscreen(bool isFullscreen)
  {
    Screen.fullScreen = isFullscreen;
  }

  public void SetResolution(int resolutionIndex)
  {
    Resolution resolution = _resolutions[resolutionIndex];
    Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
  }

  public void SetQuality(int qualityIndex)
  {
    QualitySettings.SetQualityLevel(qualityIndex);
  }

  public void ExitSettings()
  {
    SceneManager.LoadScene("GravityAttractor");
  }

  public void SaveSettings()
  {
    PlayerPrefs.SetInt("QualitySettingsPreference", _qualityDropdown.value);
    PlayerPrefs.SetInt("ResolutionPreference", _resolutionDropdown.value);
    PlayerPrefs.SetInt("FullscreenPreference", System.Convert.ToInt32(Screen.fullScreen));
  }

  public void LoadSettings(int currentResolutionIndex)
  {
    _qualityDropdown.value = PlayerPrefs.HasKey("QualitySettingsPreference")
      ? PlayerPrefs.GetInt("QualitySettingsPreference")
      : 3;

    _resolutionDropdown.value = PlayerPrefs.HasKey("ResolutionPreference")
      ? PlayerPrefs.GetInt("ResolutionPreference")
      : currentResolutionIndex;

    Screen.fullScreen = PlayerPrefs.HasKey("FullscreenPreference")
      ? System.Convert.ToBoolean(PlayerPrefs.GetInt("FullscreenPreference"))
      : true;
  }
}