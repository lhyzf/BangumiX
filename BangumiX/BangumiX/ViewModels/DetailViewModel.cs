using Bangumi.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BangumiX.ViewModels
{
    public class DetailViewModel : BaseViewModel
    {
        public SubjectLarge Subject { get; set; }

        public DetailViewModel(SubjectLarge subject = null)
        {
            Title = subject?.Name;
            if (string.IsNullOrEmpty(Title))
            {
                Title = subject?.NameCn;
            }
            Subject = subject;
        }
    }
}
