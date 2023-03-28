using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Globalization;
using WebApiMongoDB.Models;
using Workout_Web_Api.Models;
using System;
using System.Globalization;
using System.Drawing;
using System.Reflection;

namespace Workout_Web_Api.Repository
{
    public class OcarinaRepository : IOcarinaRepository
    {
        private readonly IMongoCollection<Workout> _workoutCollection;
        private readonly IMongoCollection<DayReport> _dayReportCollection;
        private readonly IMongoCollection<WeekReport> _weekReportCollection;
        public OcarinaRepository(IOptions<WorkoutDatabaseSettings> ocarinaRepository)
        {
            var mongoClient = new MongoClient(ocarinaRepository.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(ocarinaRepository.Value.DatabaseName);

            _workoutCollection = mongoDatabase.GetCollection<Workout>
                (ocarinaRepository.Value.WorkoutCollectionName);

            _dayReportCollection = mongoDatabase.GetCollection<DayReport>
                (ocarinaRepository.Value.DayReportCollectionName);

            _weekReportCollection = mongoDatabase.GetCollection<WeekReport>
                (ocarinaRepository.Value.WeekReportCollectionName);

            if (_weekReportCollection.CountDocuments(FilterDefinition<WeekReport>.Empty) == 0)
            {
                List<WeekReport> weekReportsOfAllYear = new List<WeekReport>();
                CultureInfo culture = CultureInfo.CurrentCulture;
                Calendar calendar = culture.Calendar;

                for (var m = 1; m <= 12; m++)
                {
                    int firstDayOfMonthHandler = 0;
                    var daysInMonth = calendar.GetDaysInMonth(2023, m);
                    for (var day = 1; day <= daysInMonth;  day += 7)
                    {
                        var weekDay = new DateTime(DateTime.Now.Year, m, day);
                        var weekOfYear = calendar.GetWeekOfYear(new DateTime(DateTime.Now.Year, m, day), CalendarWeekRule.FirstDay, culture.DateTimeFormat.FirstDayOfWeek);

                        var weekOfMonth = weekOfYear  - ((m - 1) * 4);

                        if (m >= 5 && m < 8)
                        {
                            weekOfMonth = weekOfMonth - 1;
                        }

                        if (m >= 8 && m < 10)
                        {
                            weekOfMonth = weekOfMonth - 2;
                        }

                        if (m >= 10)
                        {
                            weekOfMonth = weekOfMonth - 3;
                        }


                        if (day == 1 && weekDay.DayOfWeek.ToString() != "Sunday")
                        {
                            firstDayOfMonthHandler = Convert.ToInt32(weekDay.DayOfWeek) - 6;
                        }

                        weekReportsOfAllYear.Add(
                            new WeekReport
                            {
                                Week = weekOfMonth,
                                Month = m,
                                Sunday = new DayReport()
                                {
                                    Date = weekDay.AddDays(firstDayOfMonthHandler)
                                },
                                Monday = new DayReport()
                                {
                                    Date = weekDay.AddDays(firstDayOfMonthHandler + 1)
                                },
                                Tuesday = new DayReport()
                                {
                                    Date = weekDay.AddDays(firstDayOfMonthHandler + 2)
                                },
                                Wednesday = new DayReport()
                                {
                                    Date = weekDay.AddDays(firstDayOfMonthHandler + 3)
                                },
                                Thursday = new DayReport()
                                {
                                    Date = weekDay.AddDays(firstDayOfMonthHandler + 4)
                                },
                                Friday = new DayReport()
                                {
                                    Date = weekDay.AddDays(firstDayOfMonthHandler + 5)
                                },
                                Saturday = new DayReport()
                                {
                                    Date = weekDay.AddDays(firstDayOfMonthHandler + 6)
                                },
                            });

                        if (m == 2 && weekOfMonth == 4)
                        {
                            weekReportsOfAllYear.Add(
                           new WeekReport
                           {
                               Week = weekOfMonth+1,
                               Month = m,
                               Sunday = new DayReport()
                               {
                                   Date = weekDay.AddDays(firstDayOfMonthHandler + 7)
                               },
                               Monday = new DayReport()
                               {
                                   Date = weekDay.AddDays(firstDayOfMonthHandler + 8)
                               },
                               Tuesday = new DayReport()
                               {
                                   Date = weekDay.AddDays(firstDayOfMonthHandler + 9)
                               },
                               Wednesday = new DayReport()
                               {
                                   Date = weekDay.AddDays(firstDayOfMonthHandler + 10)
                               },
                               Thursday = new DayReport()
                               {
                                   Date = weekDay.AddDays(firstDayOfMonthHandler + 11)
                               },
                               Friday = new DayReport()
                               {
                                   Date = weekDay.AddDays(firstDayOfMonthHandler + 12)
                               },
                               Saturday = new DayReport()
                               {
                                   Date = weekDay.AddDays(firstDayOfMonthHandler + 13)
                               },
                           });
                        }
                    }
                }

                _weekReportCollection.InsertMany(weekReportsOfAllYear);

            }

        }
        public async Task CreateWorkoutAsync(Workout Workout)
        {
            await _workoutCollection.InsertOneAsync(Workout);
        }
        public async Task<DayReport> UpdateWorkoutOnDayReportAsync(Workout Workout)
        {
            var filterCommand = Builders<DayReport>.Filter.And(
                Builders<DayReport>.Filter.Where(x => x.Date.Day == Workout.Date.Day),
                Builders<DayReport>.Filter.Where(x => x.Date.Month == Workout.Date.Month)
                );

            var dailyDayReportDocument = _dayReportCollection.Find(filterCommand).FirstOrDefault();

            if (dailyDayReportDocument != null)
            {
                var totalCaloriesBurnedOnDay = dailyDayReportDocument.Calories.Sum() + Workout.Calories;
                var totalIntensityOnDay = dailyDayReportDocument.Intensity.Sum() + Workout.Intensity;
                var ThresholdReached = false;

                if (totalCaloriesBurnedOnDay >= 300) ThresholdReached = true;
                else ThresholdReached = false;


                var updateCommand = Builders<DayReport>.Update.Push("Calories", Workout.Calories)
                                                                    .Push("Intensity", Workout.Intensity)
                                                                    .Push("Exercise", Workout.Exercise)
                                                                    .Set("CaloriesThresholdReached", ThresholdReached)
                                                                    .Set("TotalCalories", totalCaloriesBurnedOnDay )
                                                                    .Set("TotalIntensity", totalIntensityOnDay)
                                                                    .Set("Variation", totalCaloriesBurnedOnDay - 300);

                await _dayReportCollection.UpdateOneAsync(filterCommand, updateCommand);


                return await _dayReportCollection.Find(filterCommand).FirstOrDefaultAsync();
            }

            else
            {
                DayReport newDailyDayReportDocument = new DayReport()
                {
                    Date = Workout.Date,
                    Calories = new List<int>(),
                    Intensity = new List<int>(),
                    Exercise = new List<string>(),
                    WeekDay = Workout.Date.DayOfWeek.ToString(),
                    TotalCalories = Workout.Calories,
                    Variation = Workout.Calories - 300,
                    CaloriesThresholdReached = false

                };
                newDailyDayReportDocument.Calories.Add(Workout.Calories);
                newDailyDayReportDocument.Intensity.Add(Workout.Intensity);
                newDailyDayReportDocument.Exercise.Add(Workout.Exercise);

                if (Workout.Calories >= 300) newDailyDayReportDocument.CaloriesThresholdReached = true;
                else newDailyDayReportDocument.CaloriesThresholdReached = false;

                await _dayReportCollection.InsertOneAsync(newDailyDayReportDocument);

                return newDailyDayReportDocument;
            }
        }

        public async Task<List<Workout>> GetWorkoutsAsync()
        {
           return await _workoutCollection.Find(x => true).ToListAsync();
        }

        public async Task<DayReport> GetDayReportAsync()
        {
            var output = await _dayReportCollection.Find(x => x.Date.Day == DateTime.Now.Day).FirstOrDefaultAsync();
            return output;
        }

        public async Task<List<WeekReport>> GetWeekReportsAsync()
        {
            var output = await _weekReportCollection.Find(x => true).ToListAsync();

            return output;
        }

        public async Task<List<DayReport>> GetAllDayReportsAsync()
        {
            var output = await _dayReportCollection.Find(x => true).ToListAsync();
            return output;
        }

        public async Task UpdateWeekReportAsync(DayReport dayReport)
        {
            CultureInfo culture = CultureInfo.CurrentCulture;
            Calendar calendar = culture.Calendar;
            
            int month = dayReport.Date.Month;
            int weekOfMonth = calendar.GetWeekOfYear(dayReport.Date, CalendarWeekRule.FirstDay, culture.DateTimeFormat.FirstDayOfWeek);

            weekOfMonth = weekOfMonth - ((month - 1) * 4);

            var filterCommand = Builders<WeekReport>.Filter.And(
                Builders<WeekReport>.Filter.Eq("Month", month),
                Builders<WeekReport>.Filter.Eq("Week", weekOfMonth)
                );

            var weekReportDocument = _weekReportCollection.Find(filterCommand).FirstOrDefault();

            UpdateDefinition<WeekReport> updateWeekReportCommand = null;

            if (weekReportDocument != null)
            {
                switch (dayReport.WeekDay)
                {
                    case "Sunday":
                        updateWeekReportCommand = Builders<WeekReport>.Update.Set("Sunday", dayReport);
                        break;
                    case "Monday":
                        updateWeekReportCommand = Builders<WeekReport>.Update.Set("Monday", dayReport);
                        break;
                    case "Tuesday":
                        updateWeekReportCommand = Builders<WeekReport>.Update.Set("Tuesday", dayReport);
                        break;
                    case "Wednesday":
                        updateWeekReportCommand = Builders<WeekReport>.Update.Set("Wednesday", dayReport);
                        break;
                    case "Thursday":
                        updateWeekReportCommand = Builders<WeekReport>.Update.Set("Thursday", dayReport);
                        break;
                    case "Friday":
                        updateWeekReportCommand = Builders<WeekReport>.Update.Set("Friday", dayReport);
                        break;
                    case "Saturday":
                        updateWeekReportCommand = Builders<WeekReport>.Update.Set("Saturday", dayReport);
                        break;

                }

                await _weekReportCollection.UpdateOneAsync(filterCommand, updateWeekReportCommand);
            }
        }

    }
}
