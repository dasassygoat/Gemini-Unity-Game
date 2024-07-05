using UnityEngine;

public class ConfigManager : MonoBehaviour
{
    public static ConfigData configData;
    [SerializeField] private TextAsset _configFile;
    private void Awake()
    {
        LoadConfig();
    }

    private void LoadConfig()
    {
         configData = JsonUtility.FromJson<ConfigData>(_configFile.text);
    }
}
