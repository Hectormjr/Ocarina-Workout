
using Workout_Web_Api.Models;
using Workout_Web_Api.Repository;
using Workout_Web_Api.Services;

namespace WebApiMongoDB.Services
{
    public class WorkoutServices: IWorkoutServices
    {
        private readonly IOcarinaRepository _ocarinaRepository;

        public WorkoutServices(IOcarinaRepository ocarinaRepository)
        {
            _ocarinaRepository = ocarinaRepository;
        }

        public async Task CreateWorkoutAsync(Workout Workout) {
            await _ocarinaRepository.CreateWorkoutAsync(Workout);

            DayReport dayReport = await _ocarinaRepository.UpdateWorkoutOnDayReportAsync(Workout);
            await _ocarinaRepository.UpdateWeekReportAsync(dayReport);
        }

        public async Task<List<Workout>> GetWorkoutsAsync()
        {
           return await _ocarinaRepository.GetWorkoutsAsync();
        }
        public async Task<DayReport> GetDayReportAsync()
        {
            DayReport dayReport =  await _ocarinaRepository.GetDayReportAsync();

            return dayReport;
        }

        public async Task<List<DayReport>> GetAllDayReportsAsync()
        {
            List<DayReport> allDayReport = await _ocarinaRepository.GetAllDayReportsAsync();

            return allDayReport;
        }

        public async Task<List<WeekReport>> GetWeekReportsAsync()
        {
            List<WeekReport> weekReports = await _ocarinaRepository.GetWeekReportsAsync();

            return weekReports;
        }
    }
}
