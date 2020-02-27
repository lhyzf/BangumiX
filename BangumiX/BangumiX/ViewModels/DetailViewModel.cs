using Bangumi.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BangumiX.ViewModels
{
    public class DetailViewModel : BaseViewModel
    {
        public SubjectLarge Subject { get; set; }

        public DetailViewModel(SubjectLarge subject = null)
        {
            Title = subject?.NameCn;
            if (string.IsNullOrEmpty(Title))
            {
                Title = subject?.Name;
            }
            Subject = subject;
        }

    }
}
