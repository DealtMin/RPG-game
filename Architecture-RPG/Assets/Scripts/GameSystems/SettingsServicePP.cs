using UnityEngine;

public class SettingsServicePP : ISettingsService
{
    public void SaveSettingsInt(string key, int value)
    {
        
        PlayerPrefs.SetInt(key, value);
    }

    public void SaveSettingsString(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
    }

    public void SaveSettingsFloat(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }

    public int LoadSettingsInt(string key)
    {
        if (PlayerPrefs.HasKey(key))
            return PlayerPrefs.GetInt(key);
        else
            return 0;
    }

    public string LoadSettingsString(string key)
    {
        if (PlayerPrefs.HasKey(key))
            return PlayerPrefs.GetString(key);
        else
            return "";
    }

    public float LoadSettingsFloat(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            Debug.Log(PlayerPrefs.GetFloat(key));
            return PlayerPrefs.GetFloat(key);
        }
        else
            return 0;
    }
}
