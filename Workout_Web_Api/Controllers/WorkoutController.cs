using Microsoft.AspNetCore.Mvc;
using Workout_Web_Api.Models;
using Workout_Web_Api.Services;

namespace Workout_Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutController : ControllerBase
    {
        private readonly IWorkoutServices _workoutServices;

        public WorkoutController(IWorkoutServices workoutServices)
        {
            _workoutServices = workoutServices;
        }
        [HttpPost]
        public async Task<Workout> AddWorkout([FromBody] Workout workout)
        {
            await _workoutServices.CreateWorkoutAsync(workout);

            return workout;
        }

        [HttpGet]
        [Route("GetWorkouts")]
        public async Task<List<Workout>> GetWorkouts()
        {
            var output = await _workoutServices.GetWorkoutsAsync();
            return output;

        }

        [HttpGet]
        [Route("GetDayReport")]
        public async Task<DayReport> GetDayReport()
        {
            var output = await _workoutServices.GetDayReportAsync();
            return output;

        }

        [HttpGet]
        [Route("GetAllDayReports")]
        public async Task<List<DayReport>> GetAllDayReports()
        {
            var output = await _workoutServices.GetAllDayReportsAsync();
            return output;

        }

        [HttpGet]
        [Route("GetWeekReports")]
        public async Task<List<WeekReport>> GetWeekReports()
        {
            var output = await _workoutServices.GetWeekReportsAsync();
            return output;

        }

    }
}
