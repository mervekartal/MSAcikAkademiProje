using Android.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Android.App.KeyguardManager;

namespace AlarmClock

    //alarmı ertele veya kapat tercihi
{
	public partial class AlarmPage : ContentPage
	{
        Alarm _alarm;
        KeyguardManager keyguardManager;
        KeyguardLock keyguardLock;
        bool _blocked = false;

        public AlarmPage () : this(null)
		{
        }

        public AlarmPage (Alarm alarm)
        {
            InitializeComponent(); //şimdiki

            if (alarm != null)
                CurrentAlarm = alarm;
            else
                CurrentAlarm = new Alarm();

            keyguardManager = (KeyguardManager)Forms.Context.GetSystemService(Android.Content.Context.KeyguardService);
            keyguardLock = keyguardManager.NewKeyguardLock("TAG");
            _blocked = keyguardManager.InKeyguardRestrictedInputMode(); //Klavyeyi kilitlemek ve açmak için kullanılabilecek sınıf

            if (_blocked)
            {
                keyguardLock.DisableKeyguard();
            }

            AlarmPageViewModel apVM = new AlarmPageViewModel(CurrentAlarm);
            apVM.OnAlarmTurnedOff += ToShakePage;
            BindingContext = apVM;
        }

        public Alarm CurrentAlarm
        {
            get { return _alarm; }
            set { _alarm = value; }
        }

        void ToShakePage(object sender, EventArgs args)
        {
            ShakePage shakePage = new ShakePage(CurrentAlarm);
            shakePage.OnShakeDone += ToDecidePage;
            Navigation.PushModalAsync(shakePage);
        }

        void ToDecidePage(object sender, EventArgs args)
        {
            DecidePage decidePage = new DecidePage(CurrentAlarm);
            decidePage.OnCancelDone += TurnOff;
            decidePage.OnRepeatDone += Hide;
            Navigation.PushModalAsync(decidePage);
        }

        void TurnOff(object sender, EventArgs args)
        {
            Navigation.PopModalAsync();
            var activity = (Activity)Forms.Context;
            activity.OnBackPressed();
            if (_blocked)
            {
                keyguardLock.ReenableKeyguard();
            }

            keyguardLock.Dispose();
            keyguardManager.Dispose();
        }

        void Hide(object sender, EventArgs args)
        {
            Navigation.PopModalAsync();
            var activity = (Activity)Forms.Context;
            activity.OnBackPressed();
            if (_blocked)
            {
                keyguardLock.ReenableKeyguard();
            }

            keyguardLock.Dispose();
            keyguardManager.Dispose();
        }
    }
}
