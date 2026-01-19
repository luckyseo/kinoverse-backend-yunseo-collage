using Collage.Backend.Induction.Starter.Services;
using Models;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using Clients;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<TmdbClient>((sp, client) =>
{
    var options = sp.GetRequiredService<IOptions<TmdbOptions>>().Value;

    client.BaseAddress = new Uri(options.BaseUrl);
    client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json")
    );
});

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<TmdbOptions>(
    builder.Configuration.GetSection("Tmdb")
);

// 🔹 Register application services
//builder.Services.AddScoped<IHealthService, HealthService>();
builder.Services.AddScoped<IEmotionService, EmotionsService>();
builder.Services.AddScoped<IMovieService, MoviesService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
