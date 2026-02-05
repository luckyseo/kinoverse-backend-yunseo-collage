# Kinoverse-backend induction project

## Project Overview

- This project is an induction project from the Collage, designed to provide practical experience on ASP.NET. During this phase, the intern learns the key concepts, such as caching, utilising external api, http client, Dto, and error handling.

## Tech stack used

- ASP.NET (C#)
- Git
- Follows MVC structure

## How to run the API locally

1. on terminal: dotnet run
2. on browser: http://localhost:5262/swagger
3. test API

## How to configure the TBDb API Key

- login TMDb webite
- get API Key from setting > API
- dotnet user-secrets init (using .NET Secret manager)
- dotnet user-secrets set "TmdbSettings:ApiKey" "Your_Actual_Key"
- On appsetting.json / appsetting.Development.json add below
  //dotnet user-secrets list

```json
"Tmdb": {
    "ApiKey": "",
    "BaseUrl": "https://api.themoviedb.org/3/"
  },
//if it's empty, .NET explores appsetting.json -> User Secrets
```

-Program.cs //bind here! binding -> .NET takes the values from secret storage and inject them into TmdbOptions class

```c#
builder.Services.Configure<TmdbOptions>(
    builder.Configuration.GetSection("Tmdb")
);
```

- Create Configuration.cs and add code below

```c#
public class TmdbOptions
    {
        public string ApiKey { get; set; } = null!;
        public string BaseUrl { get; set; } = null!;
    }
```

- Creat TmdbClient.cs and use API Key without exposting the actual key like below

```c#
 private readonly TmdbOptions _options;

 public TmdbClient(HttpClient httpClient, IOptions<TmdbOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value; //actual API Key here
        }

 var response = await _httpClient.GetAsync(
                $"discover/movie?with_genres={genreId}&api_key={_options.ApiKey}"
            );
```

## Available API endpoints (brief summary)

1. GET /api/movies/scifi : fetches the list of sci-fi movies with emotion summary(top emotions & user Emotion).
   e.g http://localhost:5262/api/movies/genre/Science Fiction

- As this project doesnt not have login feature, and does not know the current user, userEmotion returns the most selected emotion.

2. GET /api/movies/{movieId} : fetches the details of the movie using movieId with emotionState & userEmotion.
   e.g http://localhost:5262/api/movies/550
3. GET /api/movies/{movieId}/recommendations : lists the movie recommendations with emotion summary (top emotion & user Emotion)
   e.g http://localhost:5262/api/movies/550/recommendations
4. POST /api/movies/{movieId}/emotions : Can post emotions from the user to the movie
   e.g http://localhost:5262/api/movies/550/emotions
   request {
   userId: "string",
   emotion: "string"
   }
   response: movieId, emotionState, userEmotion

5. GET /api/emotions : lists all the available emotions
   e.g http://localhost:5262/api/emotions

6. GET api/movies/{movieId}/users : lists all the users who left the emotions on movie
   e.g http://localhost:5262/api/movies/550/users

## Optional enhancement

- app.UseMiddleware<Exceptions>(); -> Utilised middleware instead of writing mutiple try & catch
- api/movies/scifi -> api/movies/{genre}
  instead of fetching one genre "scifi", extended the api functionality to whole genre that TMDb supports.
- MovieDetails/MovieSummaryDto inherits Movie
- Genre class

-- this branch is for the //review
