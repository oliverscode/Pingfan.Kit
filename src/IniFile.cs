using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace Pingfan.Kit
{
    /// <summary>
    /// 配置文件读写, 线程安全
    /// </summary>
    public class IniFile
    {
        private readonly Dictionary<string, Section> _data;
        private readonly string _path;

        /// <summary>
        /// 当前配置文件目录
        /// </summary>
        /// <param name="path"></param>
        public IniFile(string path)
        {
            _data = new Dictionary<string, Section>(StringComparer.OrdinalIgnoreCase);
            _path = path;
            Load();
        }

        /// <summary>
        /// 获取配置值
        /// </summary>
        public string GetValue(string section, string key, string defaultValue = "")
        {
            lock (_data)
            {
                if (_data.TryGetValue(section, out var sectionData))
                {
                    if (sectionData.Entries.TryGetValue(key, out var entry))
                    {
                        return entry.Value;
                    }
                }

                return defaultValue;
            }
        }

        
        /// <summary>
        /// 设置配置值
        /// </summary>
        public void SetValue(string section, string key, string value)
        {
            lock (_data)
            {
                if (!_data.ContainsKey(section))
                {
                    _data[section] = new Section();
                }

                _data[section].Entries[key] = new Entry { Value = value };
            }
        }

        /// <summary>
        /// 获取所有配置值
        /// </summary>
        /// <returns></returns>
        public string[] GetSections()
        {
            lock (_data)
            {
                var names = new string[_data.Count];
                _data.Keys.CopyTo(names, 0);
                return names;
            }
        }

        /// <summary>
        /// 载入配置
        /// </summary>
        public void Load()
        {
            lock (_data)
            {
                if (!File.Exists(_path)) return;
                var lines = File.ReadAllLines(_path);
                var sectionRegex = new Regex(@"^\s*\[\s*(.+?)\s*\]\s*(;.*)?$");
                var keyValueRegex = new Regex(@"^\s*([^#;]+?)\s*=\s*(.*?)\s*(;.*)?$");
                var commentRegex = new Regex(@"^\s*[;#].*");

                Section? currentSection = null;

                foreach (var line in lines)
                {
                    if (commentRegex.IsMatch(line))
                    {
                        if (currentSection != null)
                        {
                            currentSection.Comments.Add(line);
                        }

                        continue;
                    }

                    var sectionMatch = sectionRegex.Match(line);
                    if (sectionMatch.Success)
                    {
                        var sectionName = sectionMatch.Groups[1].Value;
                        currentSection = new Section();
                        _data[sectionName] = currentSection;
                        continue;
                    }

                    var keyValueMatch = keyValueRegex.Match(line);
                    if (keyValueMatch.Success && currentSection != null)
                    {
                        var key = keyValueMatch.Groups[1].Value.Trim();
                        var value = keyValueMatch.Groups[2].Value.Trim();
                        var comment = keyValueMatch.Groups[3].Value;
                        currentSection.Entries[key] = new Entry { Value = value, Comment = comment };
                    }
                }
            }
        }

        /// <summary>
        /// 清空配置
        /// </summary>
        public void Clear()
        {
            lock (_data)
            {
                _data.Clear();
            }
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        public void Save()
        {
            lock (_data)
            {
                using var writer = new StreamWriter(_path);
                foreach (var sectionPair in _data)
                {
                    writer.WriteLine("[{0}]", sectionPair.Key);
                    foreach (var comment in sectionPair.Value.Comments)
                    {
                        writer.WriteLine(comment);
                    }

                    foreach (var entryPair in sectionPair.Value.Entries)
                    {
                        writer.Write("{0}={1}", entryPair.Key, entryPair.Value.Value);
                        if (!string.IsNullOrWhiteSpace(entryPair.Value.Comment))
                        {
                            writer.Write(" {0}", entryPair.Value.Comment);
                        }

                        writer.WriteLine();
                    }

                    writer.WriteLine();
                }
            }
        }

        private class Section
        {
            public Dictionary<string, Entry> Entries { get; } = new(StringComparer.OrdinalIgnoreCase);

            public List<string> Comments { get; } = new();
        }

        private class Entry
        {
            public string Value { get; set; }
            public string Comment { get; set; }
        }
    }
}