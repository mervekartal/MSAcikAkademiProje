using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace AlarmClock
{
    interface ILocalize
    {
        CultureInfo GetCurrentCultureInfo();
        void SetLocale(CultureInfo ci);
    }
}
