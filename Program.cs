using Collage.Backend.Induction.Starter.Services;
using Models;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using Clients;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<TmdbOptions>(
    builder.Configuration.GetSection("Tmdb")
);
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient<TmdbClient>((sp, client) =>
{
    var options = sp.GetRequiredService<IOptions<TmdbOptions>>().Value;
    // if (string.IsNullOrEmpty(options.ApiKey))
    // {
    //     Console.WriteLine("CRITICAL ERROR: ApiKey is MISSING!");
    // }
    // else
    // {
    //     Console.WriteLine($"SUCCESS: ApiKey loaded. Starts with: {options.ApiKey.Substring(0, 4)}***");
    // }
    client.BaseAddress = new Uri("https://api.themoviedb.org/3/");
    client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json")
    );
});

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// 🔹 Register application services
//builder.Services.AddScoped<IHealthService, HealthService>();
builder.Services.AddScoped<IEmotionService, EmotionsService>();
builder.Services.AddScoped<IMovieService, MoviesService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); //show detailed error pages in development
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{   //production error handling
    //navigate to baseURL/error on exceptions
    app.UseExceptionHandler("/error");
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
