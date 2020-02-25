using System;
using System.Collections.Generic;
using System.Text;

namespace BangumiX.Models
{
    public enum MenuItemType
    {
        Progress,
        Collection,
        Calendar,
        Setting,
        About
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
