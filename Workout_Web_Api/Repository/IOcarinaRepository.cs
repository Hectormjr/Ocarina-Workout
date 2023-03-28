using Workout_Web_Api.Models;

namespace Workout_Web_Api.Repository
{
    public interface IOcarinaRepository
    {
        Task CreateWorkoutAsync(Workout Workout);
        //Task UpdateDayliCaloriesAsync(Workout Workout);
        Task<DayReport> UpdateWorkoutOnDayReportAsync(Workout Workout);
        Task<List<Workout>> GetWorkoutsAsync();
        Task<DayReport> GetDayReportAsync();
        Task<List<WeekReport>> GetWeekReportsAsync();
        Task<List<DayReport>> GetAllDayReportsAsync();
        Task UpdateWeekReportAsync(DayReport dayReport);


    }
}
