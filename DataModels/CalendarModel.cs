namespace StresslessUI.DataModels
{
    public class CalendarModel
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public DateOnly EventDate { get; set; }
    }
}
