using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using WebApiMongoDB.Models;
using Workout_Web_Api.Models;

namespace Workout_Web_Api.Repository
{
    public class OcarinaRepository : IOcarinaRepository
    {
        private readonly IMongoCollection<Workout> _workoutCollection;
        private readonly IMongoCollection<DayReport> _dayReportCollection;
        public OcarinaRepository(IOptions<WorkoutDatabaseSettings> ocarinaRepository)
        {
            var mongoClient = new MongoClient(ocarinaRepository.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(ocarinaRepository.Value.DatabaseName);

            _workoutCollection = mongoDatabase.GetCollection<Workout>
                (ocarinaRepository.Value.WorkoutCollectionName);

            _dayReportCollection = mongoDatabase.GetCollection<DayReport>
                (ocarinaRepository.Value.DayReportCollectionName);

        }
        public async Task CreateWorkoutAsync(Workout Workout)
        {
            await _workoutCollection.InsertOneAsync(Workout);
        }
        public async Task UpdateWorkoutOnDayReportAsync(Workout Workout)
        {
            var filterCommand = Builders<DayReport>.Filter.Where(x => x.Date.Day == DateTime.Now.Day);

            var dailyDayReportDocument = _dayReportCollection.Find(filterCommand).FirstOrDefault();

            if (dailyDayReportDocument != null)
            {
                var updateCommand = Builders<DayReport>.Update.Push("Calories", Workout.Calories)
                                                                    .Push("Intensity", Workout.Intensity)
                                                                    .Push("Exercise", Workout.Exercise);
                await _dayReportCollection.UpdateOneAsync(filterCommand, updateCommand);
            }

            else
            {
                DayReport newDailyDayReportDocument = new DayReport()
                {
                    Date = Workout.Date,
                    Calories = new List<int>(),
                    Intensity = new List<int>(),
                    Exercise = new List<string>(),
                };
                newDailyDayReportDocument.Calories.Add(Workout.Calories);
                newDailyDayReportDocument.Intensity.Add(Workout.Intensity);
                newDailyDayReportDocument.Exercise.Add(Workout.Exercise);

                await _dayReportCollection.InsertOneAsync(newDailyDayReportDocument);
            }
        }

        public async Task<List<Workout>> GetWorkoutsAsync()
        {
           return await _workoutCollection.Find(x => true).ToListAsync();
        }

        public async Task<DayReport> GetDayReportAsync()
        {
            var output = await _dayReportCollection.Find(x => x.Date.Day == DateTime.Now.AddDays(1).Day).FirstOrDefaultAsync();
            return output;
        }

        public async Task<List<DayReport>> GetWeekReportAsync()
        {
            var dayOfWeek = Convert.ToInt32(DateTime.Now.DayOfWeek);
            var daysToGoOnWeek = 7 - dayOfWeek;
            var firstDay = DateTime.Now.AddDays(-dayOfWeek).Day;
            var lastDay = DateTime.Now.AddDays(daysToGoOnWeek).Day;
            
            var filter = Builders<DayReport>.Filter
                    .Where(x => x.Date.Day >= firstDay && x.Date.Day <= lastDay);

            var output = await _dayReportCollection.Find(filter).ToListAsync();
            return output;
        }
        public async Task UpdateCaloriesThresholdOnDayReportAsync(DayReport dayReport, bool ThresholdReached)
        {
            var filterCommand = Builders<DayReport>.Filter.Where(x => x.Date.Day == dayReport.Date.Day);

            var updateCommand = Builders<DayReport>.Update.Set(x => x.CaloriesThresholdReached, ThresholdReached)
                                                            .Set("TotalCalories", dayReport.Calories.Sum())
                                                            .Set("Variation", dayReport.Calories.Sum() - 300);

            await _dayReportCollection.UpdateOneAsync(filterCommand, updateCommand);


        }

        public async Task<List<DayReport>> GetAllDayReportsAsync()
        {
            var output = await _dayReportCollection.Find(x => true).ToListAsync();
            return output;
        }

    }
}
