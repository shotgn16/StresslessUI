using StresslessUI.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StresslessUI.Logic
{
    public interface ICalenderImport
    {
        Task<string> downloadCalender(string _url);
        Task<List<CalendarModel>> retrieveEvents(string _url);
        Task<List<DayOfWeek>> ConvertStringToDayOfWeek(string[] strArray);
        void Dispose();
    }
}
