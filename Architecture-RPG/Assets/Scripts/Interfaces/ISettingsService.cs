public interface ISettingsService

{
    void SaveSettingsInt (string key, int value);
    void SaveSettingsString (string key, string value);
    void SaveSettingsFloat (string key, float value);
    int LoadSettingsInt (string key);
    string LoadSettingsString (string key);
    float LoadSettingsFloat (string key);
}
