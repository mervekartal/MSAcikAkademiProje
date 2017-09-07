using System;
using Android.App;
using Android.Content;
using Android.OS;
using Xamarin.Forms;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Icu.Util;
using System.Collections.Generic;
using System.Linq;
using static Android.App.KeyguardManager;
using System.Threading;


//[assembly: Permission(Name = "alarmclock.android.permission.C2D_MESSAGE")]
//[assembly: UsesPermission(Name = "android.permission.WAKE_LOCK")]
//[assembly: UsesPermission(Name = "alarmclock.android.permission.C2D_MESSAGE")]
//[assembly: UsesPermission(Name = "com.google.android.c2dm.permission.RECEIVE")]
//[assembly: UsesPermission(Name = "android.permission.GET_ACCOUNTS")]
[assembly: Xamarin.Forms.Dependency(typeof(AlarmClock.Droid.AndroidAlarmService))]

namespace AlarmClock.Droid
{
    class AndroidAlarmService
    {
        public void Remind(Alarm alarm)
        {
            Intent tmpIntent = new Intent(Forms.Context, typeof(SimpleStartedService));            
            tmpIntent.PutExtra("alarmSaati", alarm.Time.ToString());
            tmpIntent.PutExtra("alarmAdi", alarm.FullName);
            tmpIntent.PutExtra("interval", alarm.Interval.ToString());
            tmpIntent.PutExtra("volume", alarm.Volume.ToString());
            tmpIntent.PutExtra("presses", alarm.ButtonPresses.ToString());
            tmpIntent.PutExtra("shakes", alarm.Shakes.ToString());
            tmpIntent.PutExtra("mode", "first");
            tmpIntent.PutExtra("week", alarm.Week);
            Forms.Context.StartService(tmpIntent);
        }

        public void Repeat(Alarm alarm)
        {
            CancelAlarm(alarm);
            CancelNotificaton(alarm);
            Intent tmpIntent = new Intent(Forms.Context, typeof(SimpleStartedService));
            tmpIntent.PutExtra("alarmSaati", alarm.Time.ToString());
            tmpIntent.PutExtra("alarmAdi", alarm.FullName);
            tmpIntent.PutExtra("interval", alarm.Interval.ToString());
            tmpIntent.PutExtra("volume", alarm.Volume.ToString());
            tmpIntent.PutExtra("presses", alarm.ButtonPresses.ToString());
            tmpIntent.PutExtra("shakes", alarm.Shakes.ToString());
            tmpIntent.PutExtra("mode", "repeat");
            tmpIntent.PutExtra("week", alarm.Week);
            Forms.Context.StartService(tmpIntent);
        }

        public void CancelAlarm(Alarm alarm)
        {
            var alarmIntent = new Intent(Forms.Context, typeof(AlarmReceiver));
            alarmIntent.PutExtra("alarmSaati", alarm.Time.ToString());
            alarmIntent.PutExtra("alarmAdi", alarm.FullName);
            alarmIntent.PutExtra("interval", alarm.Interval.ToString());
            alarmIntent.PutExtra("volume", alarm.Volume.ToString());
            alarmIntent.PutExtra("presses", alarm.ButtonPresses.ToString());
            alarmIntent.PutExtra("shakes", alarm.Shakes.ToString());

            var alarmPending = PendingIntent.GetBroadcast(Forms.Context, alarm.Id, alarmIntent, PendingIntentFlags.UpdateCurrent);
            var repeatingPending = PendingIntent.GetBroadcast(Forms.Context, alarm.FullName.GetHashCode(), alarmIntent, PendingIntentFlags.UpdateCurrent);

            AlarmManager _alarmManager = (AlarmManager)Forms.Context.GetSystemService(Context.AlarmService);
            _alarmManager.Cancel(alarmPending);
            _alarmManager.Cancel(repeatingPending);
        }

        public void CancelNotificaton(Alarm alarm)
        {
            var manager = NotificationManager.FromContext(Forms.Context);
            manager.Cancel(alarm.Id);
        }
    }

    [BroadcastReceiver(Permission = "com.google.android.c2dm.permission.SEND")]
    [IntentFilter(new string[] { "com.google.android.c2dm.intent.RECEIVE" }, Categories = new string[] { "alarmclock.android" })]
    [IntentFilter(new string[] { "com.google.android.c2dm.intent.REGISTRATION" }, Categories = new string[] { "alarmclock.android" })]
    [IntentFilter(new string[] { "com.google.android.gcm.intent.RETRY" }, Categories = new string[] { "alarmclock.android" })]
    [IntentFilter(new string[] { Intent.ActionBootCompleted })]

    public class AlarmReceiver : WakefulBroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            PowerManager.WakeLock sWakeLock;
            var pm = PowerManager.FromContext(context);
            sWakeLock = pm.NewWakeLock(WakeLockFlags.Full | WakeLockFlags.ScreenBright | WakeLockFlags.AcquireCausesWakeup,
                "Alarm TAG");
            sWakeLock.Acquire();

            string alarmAdi = intent.GetStringExtra("alarmAdi");
            TimeSpan alarmSaati = TimeSpan.Parse(intent.GetStringExtra("alarmSaati"));
            int alarmInterval = int.Parse(intent.GetStringExtra("interval"));
            int alarmVolume = int.Parse(intent.GetStringExtra("volume"));
            int alarmPresses = int.Parse(intent.GetStringExtra("presses"));
            int alarmShakes = int.Parse(intent.GetStringExtra("shakes"));
            intent.RemoveExtra("alarmAdi");
            intent.RemoveExtra("alarmSaati");
            intent.RemoveExtra("interval");
            intent.RemoveExtra("volume");
            intent.RemoveExtra("presses");
            intent.RemoveExtra("shakes");

            var tmp = new Intent(context, typeof(AlarmActivity));
            tmp.AddFlags(ActivityFlags.NewTask);
            tmp.AddFlags(ActivityFlags.NoHistory);
            
            tmp.PutExtra("alarmSaati", alarmSaati.ToString());
            tmp.PutExtra("alarmAdi", alarmAdi);
            tmp.PutExtra("interval", alarmInterval.ToString());
            tmp.PutExtra("volume", alarmVolume.ToString());
            tmp.PutExtra("presses", alarmPresses.ToString());
            tmp.PutExtra("shakes", alarmShakes.ToString());

            context.StartActivity(tmp);

            sWakeLock.Release();            
        }
    }

    [BroadcastReceiver(Permission = "com.google.android.c2dm.permission.SEND")]
    [IntentFilter(new string[] { "com.google.android.c2dm.intent.RECEIVE" }, Categories = new string[] { "alarmclock.android" })]
    [IntentFilter(new string[] { "com.google.android.c2dm.intent.REGISTRATION" }, Categories = new string[] { "alarmclock.android" })]
    [IntentFilter(new string[] { "com.google.android.gcm.intent.RETRY" }, Categories = new string[] { "alarmclock.android" })]
    [IntentFilter(new string[] { Intent.ActionBootCompleted })]

    public class ReminderReceiver : WakefulBroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            PowerManager.WakeLock sWakeLock;
            var pm = PowerManager.FromContext(context);
            sWakeLock = pm.NewWakeLock(WakeLockFlags.Full | WakeLockFlags.ScreenBright | WakeLockFlags.AcquireCausesWakeup,
                "Reminder TAG");
            sWakeLock.Acquire();

            var message = intent.GetStringExtra("message");
            var title = intent.GetStringExtra("title");
            TimeSpan alarmSaati = TimeSpan.Parse(intent.GetStringExtra("alarmSaati"));
            string _id = title + alarmSaati.ToString(@"hh\:mm");
            int _alarmId = _id.GetHashCode();

            var resultIntent = new Intent(context, typeof(MainActivity));
            resultIntent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);

            var pending = PendingIntent.GetActivity(context, 0,
                resultIntent,
                PendingIntentFlags.CancelCurrent);

            var builder =
                new Notification.Builder(context)
                    .SetContentTitle(title)
                    .SetContentText(message)
                    .SetSmallIcon(Resource.Drawable.Icon)
                    .SetDefaults(NotificationDefaults.Lights)
                    .SetOngoing(true);

            builder.SetContentIntent(pending);

            var notification = builder.Build();

            var manager = NotificationManager.FromContext(context);
            manager.Notify(_alarmId, notification);

            sWakeLock.Release();
        }
    }

    [Service]
    public class SimpleStartedService : IntentService
    {
        AlarmManager _alarmManager;

        public SimpleStartedService() : base ("SimpleStartedService")
        {
            _alarmManager = (AlarmManager)Forms.Context.GetSystemService(Context.AlarmService);
        }

        protected override void OnHandleIntent(Android.Content.Intent intent)
        {
            string mode = intent.GetStringExtra("mode");

            string alarmAdi = intent.GetStringExtra("alarmAdi");
            TimeSpan alarmSaati = TimeSpan.Parse(intent.GetStringExtra("alarmSaati"));
            int alarmInterval = int.Parse(intent.GetStringExtra("interval"));
            int alarmVolume = int.Parse(intent.GetStringExtra("volume"));
            int alarmPresses = int.Parse(intent.GetStringExtra("presses"));
            int alarmShakes = int.Parse(intent.GetStringExtra("shakes"));
            string week = intent.GetStringExtra("week");
            intent.RemoveExtra("alarmAdi");
            intent.RemoveExtra("alarmSaati");
            intent.RemoveExtra("interval");
            intent.RemoveExtra("volume");
            intent.RemoveExtra("presses");
            intent.RemoveExtra("shakes");
            intent.RemoveExtra("mode");
            intent.RemoveExtra("week");

            int today = (int)DateTime.Today.DayOfWeek;

            List<int> alarmDays = new List<int>();

            // sunday - 0, monday - 1 ... saturday - 6

            if (week.Contains("pzr"))
                alarmDays.Add((int)DayOfWeek.Sunday);

            if (week.Contains("pzt"))
                alarmDays.Add((int)DayOfWeek.Monday);

            if (week.Contains("sal"))
                alarmDays.Add((int)DayOfWeek.Tuesday);

            if (week.Contains("crs"))
                alarmDays.Add((int)DayOfWeek.Wednesday);

            if (week.Contains("prs"))
                alarmDays.Add((int)DayOfWeek.Thursday);

            if (week.Contains("cum"))
                alarmDays.Add((int)DayOfWeek.Friday);

            if (week.Contains("cmt"))
                alarmDays.Add((int)DayOfWeek.Saturday);

            List<long> millis = new List<long>();

            if (alarmDays.Count == 0)
            {
                long millisecondsToWait = Convert.ToInt64(alarmSaati.TotalMilliseconds
                                                            - DateTime.Now.TimeOfDay.TotalMilliseconds);
                if (millisecondsToWait <= 0)
                    millisecondsToWait += 24 * 3600 * 1000;

                millis.Add(millisecondsToWait);
            }
            else
            {
                for (int i = 0; i < alarmDays.Count; i++)
                {
                    int diff = alarmDays[i] - today;
                    long millisecondsToWait = Convert.ToInt64(alarmSaati.TotalMilliseconds
                                                            - DateTime.Now.TimeOfDay.TotalMilliseconds);
                    millisecondsToWait += diff * 24 * 3600 * 1000; // add days diff
                    if (millisecondsToWait < 0)
                        millisecondsToWait += 7 * 24 * 3600 * 1000;

                    millis.Add(millisecondsToWait);
                }
            }

            if (mode.Equals("first"))
            {
                long millisecondsToWait = millis.Min(time => time);
                //int index = millis.FindIndex((item) => (item == millisecondsToWait));
                DateTime alarmDay = DateTime.Now + TimeSpan.FromMilliseconds(millisecondsToWait);

                DayOfWeek dow = alarmDay.DayOfWeek;
                string day = "";
                switch (dow)
                {
                    case DayOfWeek.Monday:
                        day = "pzt";
                        break;
                    case DayOfWeek.Tuesday:
                        day = "sal";
                        break;
                    case DayOfWeek.Wednesday:
                        day = "crs";
                        break;
                    case DayOfWeek.Thursday:
                        day = "prs";
                        break;
                    case DayOfWeek.Friday:
                        day = "cum";
                        break;
                    case DayOfWeek.Saturday:
                        day = "cmt";
                        break;
                    case DayOfWeek.Sunday:
                        day = "pzr";
                        break;
                }

                var reminderIntent = new Intent(Forms.Context, typeof(ReminderReceiver)); //takvim denetimi
                reminderIntent.PutExtra("title", alarmAdi);
                reminderIntent.PutExtra("message", "boþ: " + alarmSaati.ToString(@"hh\:mm") + ", " + day);
                reminderIntent.PutExtra("alarmSaati", alarmSaati.ToString());
                // unique id
                string _id = alarmAdi + alarmSaati.ToString(@"hh\:mm");
                int _alarmId = _id.GetHashCode();
                var reminderPending = PendingIntent.GetBroadcast(Forms.Context, _alarmId, reminderIntent, PendingIntentFlags.UpdateCurrent);

          //      var alarmManager = (AlarmManager)Forms.Context.GetSystemService(Context.AlarmService);
                _alarmManager.Set(AlarmType.ElapsedRealtimeWakeup, SystemClock.ElapsedRealtime(), reminderPending);

                var alarmIntent = new Intent(Forms.Context, typeof(AlarmReceiver));
                alarmIntent.PutExtra("alarmSaati", alarmSaati.ToString());
                alarmIntent.PutExtra("alarmAdi", alarmAdi);
                alarmIntent.PutExtra("interval", alarmInterval.ToString());
                alarmIntent.PutExtra("volume", alarmVolume.ToString());
                alarmIntent.PutExtra("presses", alarmPresses.ToString());
                alarmIntent.PutExtra("shakes", alarmShakes.ToString());

                var alarmPending = PendingIntent.GetBroadcast(Forms.Context, _alarmId, alarmIntent, PendingIntentFlags.UpdateCurrent);
                _alarmManager.Set(AlarmType.ElapsedRealtimeWakeup, SystemClock.ElapsedRealtime() + millisecondsToWait, alarmPending);
            }
            else 
            {
                alarmSaati = DateTime.Now.TimeOfDay.Add(TimeSpan.FromMilliseconds(alarmInterval * 60 * 1000));
                var reminderIntent = new Intent(Forms.Context, typeof(ReminderReceiver));
                reminderIntent.PutExtra("title", alarmAdi + "()");
                reminderIntent.PutExtra("message", "boþ: " + alarmSaati.ToString(@"hh\:mm"));
                reminderIntent.PutExtra("alarmSaati", alarmSaati.ToString());

                string _id = alarmAdi;// + alarmTime.ToString(@"hh\:mm");
                int _alarmId = _id.GetHashCode();

                var reminderPending = PendingIntent.GetBroadcast(Forms.Context, _alarmId, reminderIntent, PendingIntentFlags.UpdateCurrent);

                var alarmManager = (AlarmManager)Forms.Context.GetSystemService(Context.AlarmService);
                _alarmManager.Set(AlarmType.ElapsedRealtimeWakeup, SystemClock.ElapsedRealtime(), reminderPending);

                var alarmIntent = new Intent(Forms.Context, typeof(AlarmReceiver));
                alarmIntent.PutExtra("alarmSaati", alarmSaati.ToString());
                alarmIntent.PutExtra("alarmAdi", alarmAdi);
                alarmIntent.PutExtra("interval", alarmInterval.ToString());
                alarmIntent.PutExtra("volume", alarmVolume.ToString());
                alarmIntent.PutExtra("presses", alarmPresses.ToString());
                alarmIntent.PutExtra("shakes", alarmShakes.ToString());
                long millisecondsToWait = alarmInterval * 60 * 1000;

                var alarmPending = PendingIntent.GetBroadcast(Forms.Context, _alarmId, alarmIntent, PendingIntentFlags.UpdateCurrent);
                _alarmManager.Set(AlarmType.ElapsedRealtimeWakeup, SystemClock.ElapsedRealtime() + millisecondsToWait, alarmPending);
            }
        }
    }
}