namespace StresslessUI.DataModels
{
    internal class ConfigurationModel
    {
        public Guid ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string[] WorkingDays { get; set; }
        public TimeOnly DayStartTime { get; set; }
        public TimeOnly DayEndTime { get; set; }
        public string CalenderImport { get; set; }
        public CalendarModel[] Calender { get; set; }
        public string UiLoc { get; set; }

        public static ConfigurationModel _model = new();
    }
}
