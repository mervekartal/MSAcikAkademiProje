using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace AlarmClock
{
    class MainViewModel : ViewModelBase
    {
        AlarmListViewModel alarmList;
        DateTime dateTime;

        public MainViewModel()
        {
            AlarmList = new AlarmListViewModel();
            DateTime = DateTime.Now;

            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                this.DateTime = DateTime.Now;
                return true;
            });
        }

        public AlarmListViewModel AlarmList
        {
            get { return alarmList; }
            protected set { SetProperty(ref alarmList, value); }
        }

        public DateTime DateTime
        {
            get
            {
                return dateTime;
            }
            set
            {
                SetProperty(ref dateTime, value);
            }
        }
    }
}
