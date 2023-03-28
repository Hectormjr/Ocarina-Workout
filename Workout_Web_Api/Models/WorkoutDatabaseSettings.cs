namespace WebApiMongoDB.Models
{
    public class WorkoutDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string WorkoutCollectionName { get; set; } = null!;
        public string DayReportCollectionName { get; set; } = null!;
        public string WeekReportCollectionName { get; set; } = null!;

    }
}
