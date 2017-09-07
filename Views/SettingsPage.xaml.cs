using AlarmClock.Droid;
using Android.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace AlarmClock
{
	public partial class SettingsPage : ContentPage
	{
        Alarm _alarm;

        public event EventHandler OnAddAlarm;

        public SettingsPage() : this(null)
        { }

		public SettingsPage (Alarm alarm)
		{      
            if (alarm != null)
            {
                CurrentAlarm = alarm;
            }
            else
            {
                CurrentAlarm = new Alarm();
            }
            BindingContext = new AlarmSettingsViewModel(CurrentAlarm);
            InitializeComponent();
        }

        private async void OnOKClicked(object sender, EventArgs e)
        {
            if (OnAddAlarm != null)
            {
                var alarmArg = new AlarmEventArgs();
                alarmArg.Alarm = ((AlarmSettingsViewModel)BindingContext).Alarm;
                alarmArg.OldAlarm = CurrentAlarm;

                OnAddAlarm(this, alarmArg);
            }

            await Navigation.PopModalAsync();
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        public Alarm CurrentAlarm
        {
            get { return _alarm; }
            set { _alarm = value; }
        }
	}

    class AlarmEventArgs : EventArgs
    {
        Alarm _alarm, _old;

        public Alarm Alarm
        {
            get { return _alarm; }
            set { _alarm = value; }
        }

        public Alarm OldAlarm
        {
            get { return _old; }
            set { _old = value; }
        }
    }
}
