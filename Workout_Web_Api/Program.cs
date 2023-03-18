using WebApiMongoDB.Models;
using WebApiMongoDB.Services;
using Workout_Web_Api.Repository;
using Workout_Web_Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<WorkoutDatabaseSettings>
    (builder.Configuration.GetSection("DevNetStoreDataBase"));

builder.Services.AddScoped<IWorkoutServices, WorkoutServices>();
builder.Services.AddSingleton<IOcarinaRepository, OcarinaRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(MyAllowSpecificOrigins);
}

app.UseAuthorization();

app.MapControllers();

app.Run();
