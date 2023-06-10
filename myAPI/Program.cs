using Microsoft.EntityFrameworkCore;
using myAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Build the configuration
IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddEnvironmentVariables()//.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();
// Retrieve the connection string from the configuration
string connectionString = configuration["CONNECTION_STRING"];// configuration.GetConnectionString("YourConnectionStringKey");
//builder.Services.AddSingleton<DbContext, YourDbContext>();
builder.Services.AddDbContext<YourDbContext>(options => options.UseNpgsql(connectionString), ServiceLifetime.Singleton);

var app = builder.Build();


// Initialize the database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<YourDbContext>();
    //dbContext.Database.EnsureDeleted(); // or dbContext.Database.EnsureCreated();
    dbContext.Database.EnsureCreated(); // or dbContext.Database.EnsureCreated();

    // Additional initialization logic if needed
    dbContext.SaveChanges();
}





// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.MapPetEndpoints();

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
