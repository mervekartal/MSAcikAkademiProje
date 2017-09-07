using Android.Content.Res;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Xamarin.Forms;

namespace AlarmClock
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();

            MainPage = new AlarmClock.MainPage();
		}

        public App(Alarm alarm)
        {
            if (alarm != null)
            {
                InitializeComponent();
                MainPage = new AlarmClock.AlarmPage(alarm);
            }
        }

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
