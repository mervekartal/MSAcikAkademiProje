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
using Android.Media;

[assembly: Dependency(typeof(AudioService))]

namespace AlarmClock.Droid
{
    class AudioService : IAudio
    {
        int _originalVolume;

        public AudioService()
        {
            _audioManager = (AudioManager)Android.App.Application.Context.GetSystemService(Context.AudioService);
            _originalVolume = _audioManager.GetStreamVolume(Stream.Music);
            _mediaPlayer = new MediaPlayer();
            _mediaPlayer.SetAudioStreamType(Stream.Music);
        }

        private MediaPlayer _mediaPlayer;
        private AudioManager _audioManager;

        public bool PlayMp3File(string fileName, int volume)
        {
            if (fileName.Equals("sanicRape.mp3"))
            {
                _audioManager.SetStreamVolume(Stream.Music, volume, 0); //SET VOLUME HERE
                _mediaPlayer = MediaPlayer.Create(global::Android.App.Application.Context, Resource.Raw.sanicRape);
                _mediaPlayer.Start();
            }

            if (fileName.Equals("illuminati.mp3"))
            {
                int illVol = volume + 3;
                if (illVol > 15)
                    illVol = 15;
                _audioManager.SetStreamVolume(Stream.Music, volume, 0); //SET VOLUME HERE
                _mediaPlayer = MediaPlayer.Create(global::Android.App.Application.Context, Resource.Raw.illuminati);
                _mediaPlayer.Start();
            }

            return true;
        }

        public void Stop()
        {
            _mediaPlayer.Stop();
            _mediaPlayer.Release();
            _audioManager.SetStreamVolume(Stream.Music, _originalVolume, 0);
        }

        public void SetVolume(int volume)
        {
            //_audioManager.GetStreamMaxVolume(Stream.Music)
            _audioManager.SetStreamVolume(Stream.Music, volume, 0); //SET VOLUME HERE
        }
    }
}