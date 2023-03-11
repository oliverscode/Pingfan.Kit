using System.Collections.Generic;
using System.IO;

namespace Pingfan.Kit
{
    /// <summary>
    /// 根据组名键名的方式读取配置文件
    /// </summary>
    public class ConfigFile
    {
        private readonly string filePath; // 配置文件路径
        private readonly Dictionary<string, Dictionary<string, string>> data; // 配置数据

        public ConfigFile(string filePath)
        {
            this.filePath = filePath;
            this.data = new Dictionary<string, Dictionary<string, string>>();
            ReLoad();
        }

        // 读取配置值
        public string GetValue(string section, string key, string defaultValue = "")
        {
            if (data.ContainsKey(section) && data[section].ContainsKey(key))
            {
                return data[section][key];
            }
            else
            {
                return defaultValue;
            }
        }

        // 设置配置值
        public void SetValue(string section, string key, string value)
        {
            if (!data.ContainsKey(section))
            {
                data[section] = new Dictionary<string, string>();
            }

            data[section][key] = value;
            Save();
        }

        // 加载配置文件
        public void ReLoad()
        {
            if (!File.Exists(filePath))
            {
                return;
            }

            data.Clear();
            // 读取配置文件数据
            string[] lines = File.ReadAllLines(filePath);
            string currentSection = "";
            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();
                if (trimmedLine.StartsWith("[") && trimmedLine.EndsWith("]"))
                {
                    currentSection = trimmedLine.Substring(1, trimmedLine.Length - 2);
                    continue;
                }

                int index = trimmedLine.IndexOf("=");
                if (index >= 0)
                {
                    string key = trimmedLine.Substring(0, index).Trim();
                    string value = trimmedLine.Substring(index + 1).Trim();
                    if (!data.ContainsKey(currentSection))
                    {
                        data[currentSection] = new Dictionary<string, string>();
                    }

                    data[currentSection][key] = value;
                }
            }
        }

        // 保存配置文件
        private void Save()
        {
            List<string> lines = new List<string>();
            foreach (KeyValuePair<string, Dictionary<string, string>> section in data)
            {
                lines.Add($"[{section.Key}]");
                foreach (KeyValuePair<string, string> kv in section.Value)
                {
                    lines.Add($"{kv.Key}={kv.Value}");
                }
            }

            File.WriteAllLines(filePath, lines);
        }
    }
}