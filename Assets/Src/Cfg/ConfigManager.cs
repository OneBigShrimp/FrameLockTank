using UnityEngine;

class ConfigManager
{
    public static readonly ConfigManager Intance = new ConfigManager();
    public T GetConfig<T>(int cfgId) where T: ScriptableObject
    {
        string resPath = string.Format("Config/{0}/{0}{1}", typeof(T).Name, cfgId);
        return Resources.Load<T>(resPath);
    }
}
