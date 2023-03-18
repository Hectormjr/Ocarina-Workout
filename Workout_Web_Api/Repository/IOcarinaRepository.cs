using Workout_Web_Api.Models;

namespace Workout_Web_Api.Repository
{
    public interface IOcarinaRepository
    {
        Task CreateWorkoutAsync(Workout Workout);
        //Task UpdateDayliCaloriesAsync(Workout Workout);
        Task UpdateWorkoutOnDayReportAsync(Workout Workout);
        Task<List<Workout>> GetWorkoutsAsync();
        Task<DayReport> GetDayReportAsync();
        Task<List<DayReport>> GetWeekReportAsync();
        Task UpdateCaloriesThresholdOnDayReportAsync(DayReport dayReport, bool ThresholdReached);
        Task<List<DayReport>> GetAllDayReportsAsync();


    }
}
