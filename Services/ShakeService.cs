using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AlarmClock.Droid;
using Xamarin.Forms;
using Android.Hardware;

[assembly: Dependency(typeof(ShakeService))]

namespace AlarmClock.Droid
{ 
    [Activity(Label = "ShakeService")]
    public class ShakeService : Activity, ISensorEventListener
    {
        private int lastUpdate;
        private float lastX;
        private float lastY;
        private float lastZ;
        private int lastShake;
        public event EventHandler OnShake;
        private int _count = 0;

        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {
        
        }

        public void OnSensorChanged(SensorEvent e)
        {
            const int EventTimeLimit = 100;
            var current = System.Environment.TickCount;
            var updateDelta = current - lastUpdate;
            if (updateDelta < EventTimeLimit)
                return;

            const int ShakeThreshold = 350;
            var x = e.Values[0];
            var y = e.Values[1];
            var z = e.Values[2];
            var delta = x + y + z - lastX - lastY - lastZ;
            var speed = Math.Abs(delta) / updateDelta * 1000;

            const int ShakesRequired = 3;


            if (speed > ShakeThreshold)
            {
                lastShake = current;
                _count++;
                if (_count >= ShakesRequired)
                {
                    //shake happened
                    _count = 0;
                    OnShake?.Invoke(this, new EventArgs());
                }
            }

            lastUpdate = current;
            lastX = x;
            lastY = y;
            lastZ = z;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
    }
}