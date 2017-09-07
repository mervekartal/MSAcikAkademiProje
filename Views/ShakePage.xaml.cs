using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace AlarmClock
{
    public partial class ShakePage : ContentPage
    {
        public event EventHandler OnShakeDone;

        Alarm _alarm;

        public ShakePage() : this(null)
        { }

        public ShakePage(Alarm alarm)
        {
            InitializeComponent();
            if (alarm != null)
                CurrentAlarm = alarm;
            else
                CurrentAlarm = new Alarm();
            ShakePageViewModel spVM = new ShakePageViewModel(CurrentAlarm);
            spVM.OnAlarmTurnedOff += ShakeDone;
            BindingContext = spVM;
        }

        public Alarm CurrentAlarm
        {
            get { return _alarm; }
            set { _alarm = value; }
        }

        void ShakeDone(object sender, EventArgs args)
        {
            Navigation.PopModalAsync();
            OnShakeDone?.Invoke(this, new EventArgs());
        }



        public string adamdanalacagincevap;
        public Dictionary<string,string> Sorular;
        //public string[] Cevaplar;

        public int yedek;
        public void Button_OnnClicked(object sender, EventArgs e)
        {
            cevapEntry.Text = adamdanalacagincevap;
            Random rastgele = new Random();
            Sorular = new Dictionary<string, string>()
            {
                {"Türkiye'nin başkenti neresidir?","Ankara" },
                {"Atatürk nerede doğmuştur?","Selanik" },
                {"İzmir'in plaka kodu kaçtır?","35" },
                {"Atın yavrusuna ne ad verilir?","Tay" },
                {"İlk cumhurbaşkanımız kimdir?","Mustafa Kemal Atatürk" },
                {"İstanbul kaç yılında fethedilmiştir?","1453" },
                {"Fındık üretimi en çok hangi coğrafi bölgede olur?","Karadeniz" },
                {"Türkçede cümle bitince sonuna ne konur?","Nokta" },
                {"Atatürk hangi tarihte Samsun'a çıkmıştır?","19 Mayis 1919" },
                {"06 plaka kodu hangi ile aittir?","Ankara" }
            };
            int sayi = rastgele.Next(0, Sorular.Count);
        
            txtSorular.Text = Sorular.Keys.ToList()[sayi];
            yedek = sayi;
        }
        public int sayi;
        public void Button_Cvp(object sender, EventArgs e)
        {
            if (Sorular[txtSorular.Text] == cevapEntry.Text)
            {
                DisplayAlert("Alarm", "Doğru cevap alarm sustur", "Tamam");
                
                var decidePage = new DecidePage();
                Navigation.PushModalAsync(decidePage);

            }

            else
            {
                DisplayAlert("Alarm", "Yanlış Cevap", "Tamam");
            }


        }
    }
}





