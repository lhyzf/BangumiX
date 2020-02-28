using Android.Content;
using BangumiX.Droid.Renderers;
using BangumiX.Renderers;
using Google.Android.Material.FloatingActionButton;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(MyFloatButton), typeof(MyFloatButtonRenderer))]
namespace BangumiX.Droid.Renderers
{
    public class MyFloatButtonRenderer : ViewRenderer<MyFloatButton, FloatingActionButton>
    {
        private Context _context;

        public MyFloatButtonRenderer(Context context) : base(context)
        {
            this._context = context;
        }

        private FloatingActionButton fab;

        protected override void OnElementChanged(ElementChangedEventArgs<MyFloatButton> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                fab.Click -= Fab_Click;
            }

            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    fab = new FloatingActionButton(_context);
                    fab.LayoutParameters = new LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent);
                    fab.Clickable = true;
                    fab.BackgroundTintList = new Android.Content.Res.ColorStateList(new int[][] { new int[0] }, new int[] { Android.Graphics.Color.ParseColor("#2196F3") });
                    fab.SetImageResource(Resource.Drawable.abc_ic_star_black_48dp);
                    fab.Drawable.SetTintList(new Android.Content.Res.ColorStateList(new int[][] { new int[0] }, new int[] { Android.Graphics.Color.ParseColor("#FFFFFF") }));
                    SetNativeControl(fab);
                }
                fab.Click += Fab_Click;
            }

        }

        private void Fab_Click(object sender, EventArgs e)
        {
            Element.Command?.Execute(null);
        }
    }
}