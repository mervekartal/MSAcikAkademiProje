using AlarmClock.Droid;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AlarmClock
{
    class DecidePageViewModel : ViewModelBase
    {
        Alarm _alarm;

        public event EventHandler OnCancel;
        public event EventHandler OnRepeat;

        public DecidePageViewModel() : this(null)
        { }

        public DecidePageViewModel(Alarm alarm)
        {
            CurrentAlarm = alarm;
            ClickCancel = new Command(CancelAlarm);
            ClickRepeat = new Command(RepeatAlarm);
        }

        public Alarm CurrentAlarm
        {
            get { return _alarm; }
            set { SetProperty(ref _alarm, value); }
        }

        public void CancelAlarm()
        {
            var ser = DependencyService.Get<AndroidAlarmService>();
            ser.Remind(CurrentAlarm);
            //alarm iptal
            OnCancel?.Invoke(this, new EventArgs());
        }

        public void RepeatAlarm()
        {
            //alarm tekrar

            var ser = DependencyService.Get<AndroidAlarmService>();
            ser.Repeat(CurrentAlarm);
            OnRepeat?.Invoke(this, new EventArgs());
        }

        public ICommand ClickCancel { get; private set; }
        public ICommand ClickRepeat { get; private set; }
    }
}
