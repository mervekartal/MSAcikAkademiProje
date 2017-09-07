using Android.Hardware;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Android.Runtime;
using AlarmClock.Droid;

namespace AlarmClock
{
    class ShakePageViewModel : ViewModelBase
    {
        public event EventHandler OnAlarmTurnedOff;

        Alarm _alarm;

        private int _shakeCount = 0;
        private double _shakesLeft;
        private double _shakeLimit;
        private ShakeService ss = new ShakeService();
        private SensorManager manager;

       
        bool _stopped = false;


       

        public ShakePageViewModel(Alarm alarm)
        {
            CurrentAlarm = alarm;
           
            manager = SensorManager.FromContext(Forms.Context);
            var accelerometer = manager.GetDefaultSensor(SensorType.Accelerometer);
            ss.OnShake += AddShake;
            manager.RegisterListener(ss, accelerometer, SensorDelay.Fastest);

            DependencyService.Get<IAudio>().PlayMp3File("sanicRape.mp3", CurrentAlarm.Volume);
            Device.StartTimer(TimeSpan.FromMilliseconds(16), OnTimerTick);
        }

        public Alarm CurrentAlarm
        {
            get { return _alarm; }
            set { _alarm = value; }
        }

        public int ShakeCount
        {
            get { return _shakeCount; }
            set { SetProperty(ref _shakeCount, value); }
        }

        public double ShakesLeft
        {
            get { return _shakesLeft; }
            set { SetProperty(ref _shakesLeft, value); }
        }

        public void AddShake(object sender, EventArgs args)
        {
            ShakeCount++;
            ShakesLeft = _shakeLimit - ShakeCount;

            if (ShakeCount == _shakeLimit)
            { // alarm stopped
                DependencyService.Get<IAudio>().Stop();
                _stopped = true;
            //    Rotation = 0;
                manager.UnregisterListener(ss);
                manager.Dispose();
                ss.Dispose();
                OnAlarmTurnedOff?.Invoke(this, new EventArgs());
            }
        }

       

        bool OnTimerTick()
        {
            if (_stopped)
                return true;

            //ses değiştirme

            int vol = CurrentAlarm.Volume + 3;
            if (vol > 15)
                vol = 15;
            DependencyService.Get<IAudio>().SetVolume(vol);

           

            return true;
        }




        
       
      
            
            


       



    }
}
