using AlarmClock.Droid;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace AlarmClock
{
    class AlarmListViewModel : ViewModelBase
    {
        ObservableCollection<Alarm> alarms;

        public AlarmListViewModel()
        {
            var save = DataHandler.LoadData();
            if (save != null)
            {
                var ser = DependencyService.Get<AndroidAlarmService>();
                Alarms = new ObservableCollection<Alarm>(save.Alarms);
                foreach (var alarm in Alarms)
                {
                    if (alarm.Enabled)
                        ser.Remind(alarm);
                }
            }
            else
                Alarms = new ObservableCollection<Alarm>();
        }

        public ObservableCollection<Alarm> Alarms
        {
            get { return alarms; }
            set { SetProperty(ref alarms, value); }
        }

        public void AddAlarm(Alarm alarm)
        {
            Alarms.Add(alarm);
        }
    }
}
