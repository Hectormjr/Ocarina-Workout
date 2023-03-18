
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
            await _ocarinaRepository.UpdateWorkoutOnDayReportAsync(Workout);
        }

        public async Task<List<Workout>> GetWorkoutsAsync()
        {
           return await _ocarinaRepository.GetWorkoutsAsync();
        }
        public async Task<DayReport> GetDayReportAsync()
        {
            DayReport dayReport =  await _ocarinaRepository.GetDayReportAsync();
            var totalCaloriesBurnedOnDay = dayReport.Calories.Sum();

            if (totalCaloriesBurnedOnDay >= 300) 
            {
                await _ocarinaRepository.UpdateCaloriesThresholdOnDayReportAsync(dayReport, true);
            }
            else
            {
                await _ocarinaRepository.UpdateCaloriesThresholdOnDayReportAsync(dayReport, true);
            }

            return dayReport;
        }

        public async Task<List<DayReport>> GetAllDayReportsAsync()
        {
            List<DayReport> allDayReport = await _ocarinaRepository.GetAllDayReportsAsync();

            return allDayReport;
        }

        public async Task<int> GetWeekIntensityAsync()
        {
            List<DayReport> weekReport = await _ocarinaRepository.GetWeekReportAsync();
            int totalWeekIntensity = 0;

             weekReport.ForEach(dayReport=>
             {
                 totalWeekIntensity += dayReport.Intensity.Sum();
             }) ;
            return totalWeekIntensity;
        }
    }
}
