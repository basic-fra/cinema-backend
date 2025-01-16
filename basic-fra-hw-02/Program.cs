using basic_fra_hw_02.Configuration;
using basic_fra_hw_02.Filters;
using basic_fra_hw_02.Logics;
using basic_fra_hw_02.Services;
using Microsoft.Extensions.Options;

public class Program
{
    public static Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Register services
        builder.Services.AddControllers();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("ReactPolicy", policy =>
            {
                policy.WithOrigins("http://localhost:3001") // React app URL
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });

        //string connectionString = "Server=(localdb)\\MSSQLLocalDb;database=CinemaDb_nova;Trusted_Connection=True;";

        // Configure Swagger/OpenAPI
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Register DBConfiguration for DI
        builder.Services.Configure<DBConfiguration>(builder.Configuration.GetSection("Database"));

        builder.Services.AddSingleton<ICinemaService, CinemaService>();
        builder.Services.AddSingleton<ICinemaLogic, CinemaLogic>();

        builder.Services.AddSingleton<ICinemaHallService, CinemaHallService>();
        builder.Services.AddSingleton<ICinemaHallLogic, CinemaHallLogic>();

        builder.Services.AddSingleton<IMovieService, MovieService>();
        builder.Services.AddSingleton<IMovieLogic, MovieLogic>();

        builder.Services.AddSingleton<IPersonService, PersonService>();
        builder.Services.AddSingleton<IPersonLogic, PersonLogic>();

        builder.Services.AddSingleton<ITicketService, TicketService>();
        builder.Services.AddSingleton<ITicketLogic, TicketLogic>();

        builder.Services.AddControllers(options =>
        {
            // Register LogFilter globally for all controllers
            options.Filters.Add<LogFilter>();
        });

        builder.Services.Configure<ValidationConfiguration>(builder.Configuration.GetSection("Validation"));
        builder.Services.Configure<DBConfiguration>(builder.Configuration.GetSection("Database"));

        var app = builder.Build();

        // Configure the HTTP request pipeline
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors("ReactPolicy");

        app.UseAuthorization();
        app.MapControllers();
        app.Run();

        return Task.CompletedTask;
    }
}

