using Android.App;
using Android.OS;
using Android.Widget;
using BangumiX.Droid.Services;
using BangumiX.Services;

[assembly: Xamarin.Forms.Dependency(typeof(ToastAndroid))]
namespace BangumiX.Droid.Services
{
    public class ToastAndroid : IToast
    {
        public void ShowToast(string message, bool IsLengthShort = false)
        {
            Handler mainHandler = new Handler(Looper.MainLooper);
            Java.Lang.Runnable runnableToast = new Java.Lang.Runnable(() =>
            {
                var duration = IsLengthShort ? ToastLength.Short : ToastLength.Long;
                Toast.MakeText(Application.Context, message, duration).Show();
            });
            mainHandler.Post(runnableToast);
        }
    }
}