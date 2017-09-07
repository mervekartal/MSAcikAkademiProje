using AlarmClock.Droid;
using Android.App;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlarmClock
{
    public partial class MainPage : ContentPage
	{
        public MainPage()
		{
            InitializeComponent();
            BindingContext = new MainViewModel();
            try
            {
                var activity = (MainActivity)Forms.Context;
                activity.OnSaveEvent += Save;
            }
            catch { }
        }

        async void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            if (args.SelectedItem != null)
            {
                ListView lv = sender as ListView;
                ObservableCollection<Alarm> la = lv.ItemsSource as ObservableCollection<Alarm>;
                Alarm si = args.SelectedItem as Alarm;
                int index = la.IndexOf(si);
                Alarm toChangeAlarm = la[index];
                var settingsPage = new SettingsPage(toChangeAlarm);
                settingsPage.OnAddAlarm += HandleChangeAlarm;
                await Navigation.PushModalAsync(settingsPage);

                
                ((ListView)sender).SelectedItem = null;
            }
        }

        async void OnAddButtonClicked(object sender, EventArgs e)
        {
            var settingsPage = new SettingsPage();
            settingsPage.OnAddAlarm += HandleAddAlarm;
            await Navigation.PushModalAsync(settingsPage);
        }

        void HandleAddAlarm(object sender, EventArgs e)
        {
            Alarm newAlarm = ((AlarmEventArgs)e).Alarm;
            ((MainViewModel)BindingContext).AlarmList.AddAlarm(newAlarm);
        }

        void HandleChangeAlarm(object sender, EventArgs e)
        {
            Alarm newAlarm = ((AlarmEventArgs)e).Alarm;
            Alarm oldAlarm = ((AlarmEventArgs)e).OldAlarm;
            var alarm = ((MainViewModel)BindingContext).AlarmList.Alarms.FirstOrDefault(i => 
                    (
                    i.FullName == oldAlarm.FullName &&
                    i.Interval == oldAlarm.Interval &&
                    i.Time == oldAlarm.Time
                    ));
            if (alarm != null)
            {
                alarm.FullName = newAlarm.FullName;
                alarm.Enabled = true;
                alarm.Interval = newAlarm.Interval;
                alarm.Time = newAlarm.Time;
                alarm.Pzt = newAlarm.Pzt;
                alarm.Sal = newAlarm.Sal;
                alarm.Crs = newAlarm.Crs;
                alarm.Prs = newAlarm.Prs;
                alarm.Cum = newAlarm.Cum;
                alarm.Cmt = newAlarm.Cmt;
                alarm.Pzr = newAlarm.Pzr;
            }
        }

        public void OnDelete(object sender, EventArgs e)
        {
            Alarm toDeleteAlarm = (sender as MenuItem).CommandParameter as Alarm;

            var alarm = ((MainViewModel)BindingContext).AlarmList.Alarms.FirstOrDefault(i =>
                    (
                    i.FullName == toDeleteAlarm.FullName &&
                    i.Interval == toDeleteAlarm.Interval &&
                    i.Time == toDeleteAlarm.Time
                    ));

            ((MainViewModel)BindingContext).AlarmList.Alarms.Remove(alarm);
            var ser = DependencyService.Get<AndroidAlarmService>();
            ser.CancelAlarm(alarm);
            ser.CancelNotificaton(alarm);
        }

        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            var ser = DependencyService.Get<AndroidAlarmService>();
            
            Alarm alarm = (sender as Switch).BindingContext as Alarm;
            if (e.Value)
            {
                ser.Remind(alarm);
            }
            else
            {
                ser.CancelAlarm(alarm);
                ser.CancelNotificaton(alarm);
            }            
        }

        void Save(object sender, EventArgs args)
        {
            DataHandler.SaveData(((MainViewModel)BindingContext).AlarmList.Alarms.ToList());
        }
    }
}
