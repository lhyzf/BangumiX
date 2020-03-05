using System;
using System.Collections.Generic;
using System.Text;

namespace BangumiX.Models
{
    public class Setting
    {
        public bool OrderByAirTime { get; set; }
        public bool UseBangumiData { get; set; }
        private bool _useBangumiDataAirSites;
        public bool UseBangumiDataAirSites
        {
            get => UseBangumiData && _useBangumiDataAirSites;
            set => _useBangumiDataAirSites = value;
        }
        private bool _useBangumiDataAirTime;
        public bool UseBangumiDataAirTime
        {
            get => UseBangumiData && _useBangumiDataAirTime;
            set => _useBangumiDataAirTime = value;
        }
    }
}
