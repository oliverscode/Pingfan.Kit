using System;
using System.Collections.Generic;
using System.IO;

namespace Pingfan.Kit
{
    /// <summary>
    /// 提供根据组名键名的方式读取配置文件的功能。
    /// </summary>
    public class IniFile
    {
        private readonly string _filePath; // 配置文件路径
        private readonly Dictionary<string, Dictionary<string, string>> _data; // 配置数据

        /// <summary>
        /// IniFile的构造函数
        /// </summary>
        /// <param name="filePath">需要加后缀</param>
        public IniFile(string filePath)
        {
            this._filePath = filePath;
            this._data = new Dictionary<string, Dictionary<string, string>>();
            ReLoad();
        }

        /// <summary>
        /// 从配置文件中获取指定的值。
        /// </summary>
        public string GetValue(string section, string key, string defaultValue = "")
        {
            if (_data.TryGetValue(section, out var sectionData) && sectionData.TryGetValue(key, out var value))
            {
                return value;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 将指定的值设置到配置文件中。
        /// </summary>
        public void SetValue(string section, string key, string value, bool isSave = true)
        {
            if (!_data.ContainsKey(section))
            {
                _data[section] = new Dictionary<string, string>();
            }

            _data[section][key] = value;
            if (isSave)
                Save();
        }

        /// <summary>
        /// 获取配置文件的所有sections。
        /// </summary>
        public List<string> GetSections()
        {
            return new List<string>(_data.Keys);
        }

        /// <summary>
        /// 获取指定section下的所有keys。
        /// </summary>
        public List<string> GetKeys(string section)
        {
            if (_data.TryGetValue(section, out var sectionData))
            {
                return new List<string>(sectionData.Keys);
            }
            else
            {
                return new List<string>();
            }
        }

        /// <summary>
        /// 重新加载配置文件。
        /// </summary>
        public void ReLoad()
        {
            if (!File.Exists(_filePath))
            {
                return;
            }

            _data.Clear();

            // 读取配置文件数据
            var lines = File.ReadAllLines(_filePath);
            var currentSection = "";
            foreach (var line in lines)
            {
                var trimmedLine = line.Trim();
                if (trimmedLine.StartsWith("[") && trimmedLine.EndsWith("]"))
                {
                    currentSection = trimmedLine.Substring(1, trimmedLine.Length - 2);
                    continue;
                }

                var index = trimmedLine.IndexOf("=", StringComparison.Ordinal);
                if (index >= 0)
                {
                    var key = trimmedLine.Substring(0, index).Trim();
                    var value = trimmedLine.Substring(index + 1).Trim();
                    if (!_data.ContainsKey(currentSection))
                    {
                        _data[currentSection] = new Dictionary<string, string>();
                    }

                    _data[currentSection][key] = value;
                }
            }
        }

        /// <summary>
        /// 保存配置文件。
        /// </summary>
        public void Save()
        {
            var lines = new List<string>();
            foreach (var section in _data)
            {
                lines.Add($"[{section.Key}]");
                foreach (var kv in section.Value)
                {
                    lines.Add($"{kv.Key}={kv.Value}");
                }
            }

            // 判断目录是否存在, 不存在的话, 创建目录
            PathEx.CreateDirectoryIfNotExists(_filePath);
            File.WriteAllLines(_filePath, lines);
        }
    }
}