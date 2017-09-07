using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Content.PM;
using Android.Views;
using Xamarin.Forms;

namespace AlarmClock.Droid
{
    [Activity(Label = "AlarmActivity", Icon = "@drawable/icon", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class AlarmActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            this.Window.ClearFlags(WindowManagerFlags.Fullscreen);

            string alarmAdi = Intent.GetStringExtra("alarmAdi");
            TimeSpan alarmSaati = TimeSpan.Parse(Intent.GetStringExtra("alarmSaati"));
            int alarmInterval = int.Parse(Intent.GetStringExtra("interval"));
            int alarmVolume = int.Parse(Intent.GetStringExtra("volume"));
            int alarmPresses = int.Parse(Intent.GetStringExtra("presses"));
            int alarmShakes = int.Parse(Intent.GetStringExtra("shakes"));
            Intent.RemoveExtra("alarmAdi");
            Intent.RemoveExtra("alarmSaati");
            Intent.RemoveExtra("interval");
            Intent.RemoveExtra("volume");
            Intent.RemoveExtra("presses");
            Intent.RemoveExtra("shakes");
            Alarm alarm = new Alarm(alarmAdi, alarmSaati, true, alarmInterval,
                new bool[] { false, false, false, false, false, false, false },
                alarmVolume, alarmPresses, alarmShakes);

            Forms.Init(this, bundle);
            LoadApplication(new App(alarm));
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
        }
    }
}