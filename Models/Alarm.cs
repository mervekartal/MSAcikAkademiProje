using AlarmClock.Droid;
using System;
using System.Text;
using Xamarin.Forms;

namespace AlarmClock
{
    public class Alarm : ViewModelBase
    {
        string _fullname;
        TimeSpan _time;
        bool pzt, sal, crs, prs, cum, cmt, pzr;
        bool _enabled;
        int _interval;
        int _volume; //1 to 15
        int _buttonPressesNumber;
        int _shakesNumber;
        

        public Alarm() : this(string.Empty, TimeSpan.FromTicks(DateTime.Now.TimeOfDay.Ticks), true, 1, 
                                new bool[] { false,false,false,false,false,false,false}, 0, 10, 10)
        {
        }

        public Alarm(string name, TimeSpan time, bool enabled, int interval, bool[] week, int volume = 0, int bpn = 10, int sn = 10)
        {
            FullName = name;
            Time = time;
            Enabled = enabled;
            Interval = interval;
            Pzt = week[0];
            Sal = week[1];
            Crs = week[2];
            Prs = week[3];
            Cum = week[4];
            Cmt = week[5];
            Pzr = week[6];

            Volume = volume;
            ButtonPresses = bpn;
            Shakes = sn;
        }

        public string FullName
        {
            get { return _fullname; }
            set { SetProperty(ref _fullname, value); }
        }

        public TimeSpan Time
        {
            get { return _time; }
            set { SetProperty(ref _time, value); }
        }

        string _week;
        public string Week
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                if (Pzt) sb.Append("pzt, ");
                if (Sal) sb.Append("sl, ");
                if (Crs) sb.Append("çrş, ");
                if (Prs) sb.Append("prş, ");
                if (Cum) sb.Append("cum, ");
                if (Cmt) sb.Append("сmt, ");
                if (Pzr) sb.Append("pzr, ");
                if (sb.Length > 2)
                    sb.Remove(sb.Length - 2, 2);
                return sb.ToString();
            } 
            set { SetProperty(ref _week, value); }   
        }

        public bool Pzt
        {
            get { return pzt; }
            set { SetProperty(ref pzt, value); OnPropertyChanged("Week"); }
        }
        public bool Sal
        {
            get { return sal; }
            set { SetProperty(ref sal, value); OnPropertyChanged("Week"); }
        }
        public bool Crs
        {
            get { return crs; }
            set { SetProperty(ref crs, value); OnPropertyChanged("Week"); }
        }
        public bool Prs
        {
            get { return prs; }
            set { SetProperty(ref prs, value); OnPropertyChanged("Week"); }
        }
        public bool Cum
        {
            get { return cum; }
            set { SetProperty(ref cum, value); OnPropertyChanged("Week"); }
        }
        public bool Cmt
        {
            get { return cmt; }
            set { SetProperty(ref cmt, value); OnPropertyChanged("Week"); }
        }
        public bool Pzr
        {
            get { return pzr; }
            set { SetProperty(ref pzr, value); OnPropertyChanged("Week"); }
        }

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                SetProperty(ref _enabled, value);
            }
        }

        public int Interval
        {
            get { return _interval; }
            set { SetProperty(ref _interval, value); }
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

        public int Id
        {
            get
            { 
                string _id = FullName + Time.ToString(@"hh\:mm");
                return _id.GetHashCode();
            }
        }
    }
}
