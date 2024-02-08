using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StresslessUI.DataModels;
using System.Net;

namespace StresslessUI.Logic
{
    public class CalendarImport : Controller, ICalenderImport, IDisposable
    {
        private ILogger<CalendarImport> _logger;
        public static List<CalendarModel> _List;

        public CalendarImport() { }
        public CalendarImport(ILogger<CalendarImport> logger)
        {
            _logger = logger;
        }

        public async Task<string> downloadCalender(string _url)
        {
            string calenderString = "";

            if (!string.IsNullOrEmpty(_url))
            {
                using (WebClient webClient = new WebClient())
                {
                    calenderString = await webClient.DownloadStringTaskAsync(_url);
                }
            }

            return calenderString;
        }

        public async Task<List<CalendarModel>> retrieveEvents(string _url)
        {
            Ical.Net.Calendar calender = Ical.Net.Calendar.Load(await downloadCalender(_url));

            foreach (var calenderEvent in calender.Events)
            {
                if (string.IsNullOrEmpty(calenderEvent.Location))
                {
                    calenderEvent.Location = "Not Specified";
                }

                List<DayOfWeek> WorkingDays = await ConvertStringToDayOfWeek(ConfigurationModel._model.WorkingDays);

                if (calenderEvent.DtStart.Value >= DateTime.Now.Date &&
                    calenderEvent.DtEnd.Value <= DateTime.Now.Date.AddDays(4) &&
                    WorkingDays.Contains(calenderEvent.DtStart.Value.DayOfWeek))
                {
                    DateTime sDate = calenderEvent.DtStart.Value;
                    DateTime eDate = calenderEvent.DtEnd.Value;

                    _List = new List<CalendarModel>();

                    _List.Add(new CalendarModel()
                    {
                        StartTime = TimeOnly.FromDateTime(sDate),
                        EventDate = DateOnly.FromDateTime(calenderEvent.DtStart.Value),
                        EndTime = TimeOnly.FromDateTime(eDate),
                        Location = calenderEvent.Location,
                        Name = calenderEvent.Summary
                    });
                }
            }

            return _List;
        }

        public async Task<List<DayOfWeek>> ConvertStringToDayOfWeek(string[] strArray)
        {
            List<DayOfWeek> DayList = new List<DayOfWeek>();

            foreach (string Day in strArray)
            {
                switch (Day)
                {
                    case "Monday": DayList.Add(DayOfWeek.Monday); break;
                    case "Tuesday": DayList.Add(DayOfWeek.Tuesday); break;
                    case "Wednesday": DayList.Add(DayOfWeek.Wednesday); break;
                    case "Thursday": DayList.Add(DayOfWeek.Thursday); break;
                    case "Friday": DayList.Add(DayOfWeek.Friday); break;
                    case "Saturday": DayList.Add(DayOfWeek.Saturday); break;
                    case "Sunday": DayList.Add(DayOfWeek.Sunday); break;
                    default: throw new ArgumentException("Invalid day of week");
                }
            }

            return DayList;
        }

        public void Dispose() => GC.Collect();
    }
}
