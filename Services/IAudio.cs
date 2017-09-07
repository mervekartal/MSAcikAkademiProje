using System;

namespace AlarmClock
{
    public interface IAudio
    {
        bool PlayMp3File(string fileName, int volume);
        void Stop();
        void SetVolume(int volume);
    }
}
