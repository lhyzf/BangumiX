using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace BangumiX.Controls
{
    public class InfiniteListView : ListView
    {
        public static readonly BindableProperty LoadMoreCommandProperty = BindableProperty.Create(nameof(LoadMoreCommand), typeof(ICommand), typeof(InfiniteListView));
        public static readonly BindableProperty HasMoreItemsProperty = BindableProperty.Create(nameof(HasMoreItems), typeof(bool), typeof(InfiniteListView), true);

        public ICommand LoadMoreCommand
        {
            get { return (ICommand)GetValue(LoadMoreCommandProperty); }
            set { SetValue(LoadMoreCommandProperty, value); }
        }

        public bool HasMoreItems
        {
            get
            {
                return (bool)GetValue(HasMoreItemsProperty);
            }
            set
            {
                SetValue(HasMoreItemsProperty, value);
            }
        }

        public InfiniteListView()
        {
            ItemAppearing += InfiniteListView_ItemAppearing;
        }
        public InfiniteListView(ListViewCachingStrategy strategy) : base(strategy)
        {
            ItemAppearing += InfiniteListView_ItemAppearing;
        }

        private void InfiniteListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            if (!HasMoreItems)
            {
                return;
            }
            var items = ItemsSource as IList;

            if (items != null && items.Count > 1 && e.Item == items[items.Count - 1])
            {
                if (LoadMoreCommand != null && LoadMoreCommand.CanExecute(null))
                    LoadMoreCommand.Execute(null);
            }
        }
    }
}
