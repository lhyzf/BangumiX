using BangumiX.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BangumiX.Helper
{
    public static class SettingHelper
    {
        private static readonly string _settingPath;
        public static Setting Setting { get; set; }

        static SettingHelper()
        {
            var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal, Environment.SpecialFolderOption.Create);
            _settingPath = Path.Combine(folderPath, "setting.json");
            // 加载缓存
            if (File.Exists(_settingPath))
            {
                try
                {
                    Setting = JsonConvert.DeserializeObject<Setting>(File.ReadAllText(_settingPath));
                }
                catch (Exception)
                {
                    File.Delete(_settingPath);
                }
            }
            if (Setting == null)
            {
                Setting = new Setting();
            }
        }

        public static void Save()
        {
            File.WriteAllText(_settingPath, JsonConvert.SerializeObject(Setting));
        }
    }
}
