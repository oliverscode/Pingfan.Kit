using System.Collections.Generic;
using System.IO;

namespace Pingfan.Kit
{
    /// <summary>
    /// 根据组名键名的方式读取配置文件
    /// </summary>
    public class IniFile
    {
        private readonly string filePath; // 配置文件路径
        private readonly Dictionary<string, Dictionary<string, string>> data; // 配置数据

        /// <summary>
        /// 获取所有Sections
        /// </summary>
        public List<string> Sections => new List<string>(data.Keys);

        public IniFile(string filePath)
        {
            this.filePath = filePath;
            this.data = new Dictionary<string, Dictionary<string, string>>();
            ReLoad();
        }

        /// <summary>
        /// 读取配置
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 设置指定值
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetValue(string section, string key, string value, bool isSave = true)
        {
            if (!data.ContainsKey(section))
            {
                data[section] = new Dictionary<string, string>();
            }

            data[section][key] = value;
            if (isSave)
                Save();
        }

        /// <summary>
        /// 读取所有section
        /// </summary>
        /// <returns></returns>
        public List<string> GetSections()
        {
            return new List<string>(data.Keys);
        }

        /// <summary>
        /// 获取section下所有键
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        public List<string> GetKeys(string section)
        {
            if (data.ContainsKey(section))
            {
                return new List<string>(data[section].Keys);
            }
            else
            {
                return new List<string>();
            }
        }

        /// <summary>
        /// 刷新配置文件
        /// </summary>
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


        public void Save()
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

            // 判断目录是否存在, 不存在的话, 创建目录
            string dir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }


            File.WriteAllLines(filePath, lines);
        }
    }
}