using Android.Content;
using Android.OS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AlarmClock
{
    class AlarmPageViewModel : ViewModelBase
    {
        public event EventHandler OnAlarmTurnedOff;

        Alarm _alarm;

        bool _stopped = false;
        int _clicksNumber = 0;
        int _clicksLimit;
        int _clicksLeft;

        Vibrator vibrator = (Vibrator)Forms.Context.GetSystemService(Context.VibratorService);

        //button hareketi
        static readonly TimeSpan duration = TimeSpan.FromSeconds(0.5);
        Random random = new Random();
        Point[] points = new Point[] {  new Point(0,0),
                                        new Point(-98,-200),
                                        new Point(-98,180),
                                        new Point(86,-200),
                                        new Point(86,180)};
        Point _startPoint;
        Point _endPoint;
        Point _animationVector;
        DateTime _startTime;
        double _x;
        double _y;
        bool moving = false;

        //button hareket
        bool rotating = false;
        double _rotation;
        double _startRotation;
        double _endRotation;
        DateTime _startTimeRotation;

        public AlarmPageViewModel() : this(null)
        { }

        public AlarmPageViewModel(Alarm alarm)
        {
            ClickCommand = new Command(HandleButtonClick);
            CurrentAlarm = alarm;
            ClicksLimit = CurrentAlarm.ButtonPresses;
            ClicksLeft = ClicksLimit;
            Rotation = 0;

            //her saniyede yarım saniye titreşim
            vibrator.Vibrate(new long[] { 500, 500 }, 0);

            //melodi
            DependencyService.Get<IAudio>().PlayMp3File("sanicRape.mp3", CurrentAlarm.Volume);

            Device.StartTimer(TimeSpan.FromMilliseconds(16), OnTimerTick);            
        }

        public Alarm CurrentAlarm
        {
            get { return _alarm; }
            set { _alarm = value; }
        }

        public int ClicksNumber
        {
            get { return _clicksNumber; }
            set { SetProperty(ref _clicksNumber, value); }
        }

        public int ClicksLimit
        {
            get { return _clicksLimit; }
            set { SetProperty(ref _clicksLimit, value); }
        }

        public int ClicksLeft
        {
            get { return _clicksLeft; }
            set { SetProperty(ref _clicksLeft, value); }
        }

        public double ButtonX
        {
            get { return _x; }
            set { SetProperty(ref _x, value); }
        }

        public double ButtonY
        {
            get { return _y; }
            set { SetProperty(ref _y, value); }
        }

        public double Rotation
        {
            get { return _rotation; }
            set { SetProperty(ref _rotation, value); }
        }

        void HandleButtonClick()
        {
            ClicksNumber++;
            ClicksLeft = ClicksLimit - ClicksNumber;
            if (ClicksNumber >= ClicksLimit) // Alarm kapa
            {
                DependencyService.Get<IAudio>().Stop();
                _stopped = true;
                vibrator.Cancel();
                OnAlarmTurnedOff?.Invoke(this, new EventArgs());
            }
        }

        bool OnTimerTick()
        {
            if (_stopped)
                return true;
           
            DependencyService.Get<IAudio>().SetVolume(CurrentAlarm.Volume);
            //hareket
            if (!moving)
            {
                moving = true;
                _startPoint = new Point(ButtonX, ButtonY);

                _endPoint = points[random.Next(points.Length)];

                _animationVector = new Point(_endPoint.X - _startPoint.X, _endPoint.Y - _startPoint.Y);

                _startTime = DateTime.Now;
            }

            if(!rotating)
            {
                _startTimeRotation = DateTime.Now;
                _startRotation = Rotation;
                _endRotation = (Rotation == 0) ? 45 : 0;
                rotating = true;
            }

            TimeSpan elapsedTime = DateTime.Now - _startTime;
            TimeSpan elapsedTimeRotation = DateTime.Now - _startTimeRotation;

           
            double t = Math.Max(0, Math.Min(1, elapsedTime.TotalMilliseconds / duration.TotalMilliseconds));
            double t_rot = Math.Max(0, Math.Min(1, elapsedTimeRotation.TotalMilliseconds*4 / duration.TotalMilliseconds));

            ButtonX = _startPoint.X + t * _animationVector.X;
            ButtonY = _startPoint.Y + t * _animationVector.Y;

            if (_endRotation == 45)
                Rotation = _startRotation + t_rot * _endRotation;
            else
                Rotation = _startRotation - t_rot * _startRotation;


            if (Math.Abs(ButtonX - _endPoint.X) <= 0.0001 &&
                Math.Abs(ButtonY - _endPoint.Y) <= 0.0001)
                moving = false;
            if (Math.Abs(Rotation - _endRotation) <= 0.0001)
                rotating = false;

            return true;
        }

        public ICommand ClickCommand { get; private set; }
    }
}
