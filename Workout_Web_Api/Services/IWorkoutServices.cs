using Workout_Web_Api.Models;

namespace Workout_Web_Api.Services

{
    public interface IWorkoutServices
    {
        Task CreateWorkoutAsync(Workout Workout);
        Task<List<Workout>> GetWorkoutsAsync();
        Task<DayReport> GetDayReportAsync();
        Task<List<WeekReport>> GetWeekReportsAsync();
        Task<List<DayReport>> GetAllDayReportsAsync();


    }
}
