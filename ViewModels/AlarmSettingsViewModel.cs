using AlarmClock.Droid;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AlarmClock
{
    class AlarmSettingsViewModel : ViewModelBase
    {
        Alarm _alarm;
        TimeSpan _time;
        string _name;
        int _interval = 1;
        bool pzt, sal, crs, prs, cum, cmt, pzr;
        bool _enabled;
        int _volume; //1 to 15
        int _buttonPressesNumber;
        int _shakesNumber;
        int _mode;
        string _definition;
        

        public AlarmSettingsViewModel() : this(null)
        {
        }

        public AlarmSettingsViewModel(Alarm alarm)
        {
            var ser = DependencyService.Get<AndroidAlarmService>();

            AddAlarmCommand = new Command(ExecuteAddAlarm);

            if (alarm != null)
            {
                Alarm = alarm;
                Time = alarm.Time;
                Name = alarm.FullName;
                Interval = alarm.Interval;
                Pzt = alarm.Pzt;
                Sal = alarm.Sal;
                Crs = alarm.Crs;
                Prs = alarm.Prs;
                Cum = alarm.Cum;
                Cmt = alarm.Cmt;
                Pzr = alarm.Pzr;
                Enabled = alarm.Enabled;
                Volume = alarm.Volume;
                ButtonPresses = alarm.ButtonPresses;
                Shakes = alarm.Shakes;
                switch (ButtonPresses)
                {
                    case 5:
                        AlarmMode = 0;
                        break;
                    case 30:
                        AlarmMode = 1;
                        break;
                    case 50:
                        AlarmMode = 2;
                        break;
                    default:
                        AlarmMode = 0;
                        break;
                }
                
                ser.CancelAlarm(alarm);
                ser.CancelNotificaton(alarm);
            }
            else
            {
                Alarm = new Alarm();
            }
        }

        public Alarm Alarm
        {
            get { return _alarm; }
            set { SetProperty(ref _alarm, value); }
        }

        public TimeSpan Time
        {
            get { return _time; }
            set { SetProperty(ref _time, value); }
        }

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public int Interval
        {
            get { return _interval; }
            set
            {
                SetProperty(ref _interval, value);
            }
        }

        public bool Pzt
        {
            get { return pzt; }
            set { SetProperty(ref pzt, value); }
        }
        public bool Sal
        {
            get { return sal; }
            set { SetProperty(ref sal, value); }
        }
        public bool Crs
        {
            get { return crs; }
            set { SetProperty(ref crs, value); }
        }
        public bool Prs
        {
            get { return prs; }
            set { SetProperty(ref prs, value); }
        }
        public bool Cum
        {
            get { return cum; }
            set { SetProperty(ref cum, value); }
        }
        public bool Cmt
        {
            get { return cmt; }
            set { SetProperty(ref cmt, value); }
        }
        public bool Pzr
        {
            get { return pzr; }
            set { SetProperty(ref pzr, value); }
        }

        public bool Enabled
        {
            get { return _enabled; }
            set { SetProperty(ref _enabled, value); }
        }

        public int Volume
        {
            get { return _volume; }
            set { SetProperty(ref _volume, value); }
        }

        public int ButtonPresses
        {
            get { return _buttonPressesNumber; }
            set { SetProperty(ref _buttonPressesNumber, value); }
        }

        public int Shakes
        {
            get { return _shakesNumber; }
            set { SetProperty(ref _shakesNumber, value); }
        }

        public int AlarmMode
        {
            get { return _mode; }
            set
            {
                SetProperty(ref _mode, value);
                //if (value == 0)
                //    AlarmDefinition = ""; 
                //if (value == 1)
                //    AlarmDefinition = "";
                //if (value == 2)
                //    AlarmDefinition = "";
            }
        }

        public string AlarmDefinition
        {
            get { return _definition; }
            set { SetProperty(ref _definition, value); }
        }

        void ExecuteAddAlarm()
        {
            Enabled = true;
            switch(AlarmMode)
            {
                case 0:
                    Volume = 2;
                    ButtonPresses = 5;
                    Shakes = 5;
                    break;
                case 1:
                    Volume = 9;
                    ButtonPresses = 30;
                    Shakes = 20;
                    break;
                case 2:
                    Volume = 15;
                    ButtonPresses = 50;
                    Shakes = 50;
                    break;
                default:
                    Volume = 2;
                    ButtonPresses = 5;
                    Shakes = 5;
                    break;
            }
            
            Alarm = new Alarm(Name, Time, Enabled, Interval, getWeek(), Volume, ButtonPresses, Shakes);
            var ser = DependencyService.Get<AndroidAlarmService>();
            ser.Remind(Alarm);
        }

        bool[] getWeek()
        {
            return new bool[] { Pzt,Sal,Crs,Prs,Cum,Cmt,Pzr};
        }

        public ICommand AddAlarmCommand { get; private set; }
    }
}
