using Bangumi.Data;
using BangumiX.Helper;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BangumiX.ViewModels
{
    public class SettingViewModel : BaseViewModel
    {
        public bool OrderByAirTime
        {
            get => SettingHelper.Setting.OrderByAirTime;
            set
            {
                if (SettingHelper.Setting.OrderByAirTime != value)
                {
                    SettingHelper.Setting.OrderByAirTime = value;
                    SettingHelper.Save();
                    OnPropertyChanged();
                }
            }
        }

        public bool UseBangumiData
        {
            get => SettingHelper.Setting.UseBangumiData;
            set
            {
                if (SettingHelper.Setting.UseBangumiData != value)
                {
                    SettingHelper.Setting.UseBangumiData = value;
                    if (value)
                    {
                        // 获取数据版本
                        var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal, Environment.SpecialFolderOption.Create);
                        BangumiData.Init(Path.Combine(folderPath, "bangumi-data"));
                    }
                    else
                    {
                        BangumiData.AutoCheck = false;
                    }
                    SettingHelper.Save();
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(BangumiDataVersion));
                    OnPropertyChanged(nameof(UseBangumiDataAirSites));
                    OnPropertyChanged(nameof(UseBangumiDataAirTime));
                    OnPropertyChanged(nameof(BangumiDataAutoCheck));
                    OnPropertyChanged(nameof(BangumiDataCheckInterval));
                    OnPropertyChanged(nameof(BangumiDataAutoUpdate));
                }
            }
        }

        public bool BangumiDataAutoCheck
        {
            get => BangumiData.AutoCheck;
            set
            {
                BangumiData.AutoCheck = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(BangumiDataCheckInterval));
                OnPropertyChanged(nameof(BangumiDataAutoUpdate));
            }
        }

        public int BangumiDataCheckInterval
        {
            get => BangumiData.CheckInterval;
            set
            {
                BangumiData.CheckInterval = value;
                OnPropertyChanged();
            }
        }

        public bool BangumiDataAutoUpdate
        {
            get => BangumiData.AutoUpdate;
            set
            {
                BangumiData.AutoUpdate = value;
                OnPropertyChanged();
            }
        }


        public bool UseBangumiDataAirSites
        {
            get => SettingHelper.Setting.UseBangumiDataAirSites;
            set
            {
                SettingHelper.Setting.UseBangumiDataAirSites = value;
                SettingHelper.Save();
                OnPropertyChanged();
            }
        }

        public bool UseBangumiDataAirTime
        {
            get => SettingHelper.Setting.UseBangumiDataAirTime;
            set
            {
                SettingHelper.Setting.UseBangumiDataAirTime = value;
                SettingHelper.Save();
                OnPropertyChanged();
            }
        }
        private string _bangumiDataStatus = "检查更新";
        public string BangumiDataStatus
        {
            get => _bangumiDataStatus;
            set
            {
                SetProperty(ref _bangumiDataStatus, value);
                OnPropertyChanged(nameof(BangumiDataVersion));
            }
        }

        private bool _bangumiDataVersionChecking;
        public bool BangumiDataVersionChecking
        {
            get => _bangumiDataVersionChecking;
            set => SetProperty(ref _bangumiDataVersionChecking, value);
        }

        public string BangumiDataVersion
        {
            get
            {
                // 显示当前数据版本及更新后版本
                var version = string.IsNullOrEmpty(BangumiData.Version) ?
                       "无数据" : BangumiData.Version;
                version += ((string.IsNullOrEmpty(BangumiData.LatestVersion) || BangumiData.Version == BangumiData.LatestVersion) ?
                       string.Empty :
                       $" -> {BangumiData.LatestVersion}");
                return version;
            }
        }

        /// <summary>
        /// 检查更新并下载最新版本
        /// </summary>
        public async Task UpdateBangumiData()
        {
            BangumiDataStatus = "正在检查更新";
            BangumiDataVersionChecking = true;
            bool hasNew = false;
            var startDownloadAction = new Action(() =>
            {
                hasNew = true;
                BangumiDataStatus = "正在下载数据";
            });
            if (await BangumiData.DownloadLatestBangumiData(startDownloadAction))
            {
                if (hasNew)
                {
                    NotificationHelper.Notify("数据下载成功！");
                }
                else
                {
                    NotificationHelper.Notify("已是最新版本！");
                }
            }
            else
            {
                if (hasNew)
                {
                    NotificationHelper.Notify("数据下载失败，请重试或稍后再试！");
                }
                else
                {
                    NotificationHelper.Notify("获取最新版本失败！");
                }
            }
            BangumiDataStatus = "检查更新";
            BangumiDataVersionChecking = false;
        }
    }
}
