using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlarmClock
{
	public partial class DecidePage : ContentPage
	{
        Alarm alarm1;

        public Alarm CurrentAlarm 
        {
            get { return alarm1; }
            set { alarm1 = value; }
        }

        public event EventHandler OnCancelDone;
        public event EventHandler OnRepeatDone;

        public DecidePage() : this(null)
        { }

        public DecidePage (Alarm alarm)
		{
			InitializeComponent ();
            if (alarm != null)
                CurrentAlarm = alarm;
            else
                CurrentAlarm = new Alarm();

            DecidePageViewModel dpVM = new DecidePageViewModel(CurrentAlarm);
            dpVM.OnCancel += CancelDone;
            dpVM.OnRepeat += RepeatDone;
            BindingContext = dpVM;
		}

        void CancelDone(object sender, EventArgs args)
        {
            App.Current.MainPage = new MainPage();
            OnCancelDone?.Invoke(this, new EventArgs());
        }

        void RepeatDone(object sender, EventArgs args)
        {
            Navigation.PopModalAsync();
            OnRepeatDone?.Invoke(this, new EventArgs());
        }
       
    }
}
