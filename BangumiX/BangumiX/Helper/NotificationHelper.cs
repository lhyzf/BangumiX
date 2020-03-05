using BangumiX.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BangumiX.Helper
{
    public static class NotificationHelper
    {
        public static void Notify(string msg, bool IsLengthShort = false)
        {
            DependencyService.Get<IToast>().ShowToast(msg, IsLengthShort);
        }
    }
}
