using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;
using Android.Support.Design.Widget;
using Xamarin.Forms.Platform.Android;
using Android.Content;

namespace AlarmClock.Droid
{
	[Activity (Label = "AlarmClock", Icon = "@drawable/icon", Theme="@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
        public event EventHandler OnSaveEvent;

        protected override void OnCreate (Bundle bundle)
		{
            Intent.AddFlags(ActivityFlags.NoHistory);
            base.OnCreate (bundle);
            Forms.Init (this, bundle);
            LoadApplication(new App());
        }

        public override void OnBackPressed()
        {
            OnSaveEvent?.Invoke(this, new EventArgs());
            base.OnBackPressed();
        }

        protected override void OnStop()
        {
            OnSaveEvent?.Invoke(this, new EventArgs());
            base.OnStop();
        }

        protected override void OnPause()
        {
            OnSaveEvent?.Invoke(this, new EventArgs());
            base.OnPause();
        }
    }
}

