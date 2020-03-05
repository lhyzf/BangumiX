using System;
using System.Collections.Generic;
using System.Text;

namespace BangumiX.Services
{
    public interface IToast
    {
        void ShowToast(string message, bool IsLengthShort = false);
    }
}
