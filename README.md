# Collage Backend Induction Project Starter

This repository is the **official starter project** for the Collage backend induction.

It provides a minimal, intentionally structured ASP.NET Core Web API that demonstrates how backend features at Collage are organised and built. You will extend this project as part of your induction assignment.

---

## 🎯 Purpose of This Starter Project

This project exists to:

- provide a clean, working ASP.NET Core Web API baseline
- demonstrate the expected **Controller → Service → Model → Response** flow
- remove setup friction so you can focus on learning and building
- show how controllers call services via dependency injection
- show how response models are shaped and returned as JSON
- use Swagger to test endpoints quickly

This starter is intentionally small so the structure is obvious.

You are **not expected to build an application from scratch**.  
Instead, you will **extend this existing structure** as you work through the induction project.

---

## 🧱 What This Project Includes

This starter project includes:

- ASP.NET Core Web API
- Swagger for API testing
- Dependency Injection setup
- A simple Health endpoint demonstrating structure
- A clear folder layout you should follow

This project intentionally does **not** include:

- databases
- authentication or authorization
- DTOs or mapping layers
- external APIs (e.g. TMDb, which is used in the induction project)
- complex business logic

These concepts are introduced later in the program.

---

## ⚠️ Important Note About the Health Example

The Health controller, service, and model are provided **purely as a reference example**.

They are **not part of your induction assignment submission**.

Use them to understand:

- where logic should live
- how controllers and services interact
- how responses are returned

You should follow the same structure when building your own features.

---

## ✅ Key Files to Pay Attention To (Very Important)

These files demonstrate the structure you should follow:

### 1) `Controllers/HealthController.cs`

- Defines API endpoints (routes)
- Receives incoming requests
- Delegates work to the service
- Returns JSON responses

**Controllers should stay thin.**

---

### 2) `Services/IHealthService.cs`

- Defines the service “contract” (what the service promises to do)
- Allows controllers to depend on an interface rather than an implementation

---

### 3) `Services/HealthService.cs`

- Contains the logic (even if minimal)
- Returns a model that the controller sends back to the client

**All meaningful logic should live in services, not controllers.**

---

### 4) `Models/HealthResponse.cs`

- Defines the shape of the JSON response
- Makes the API output explicit and consistent

---

### 5) `Program.cs`

This is where the application is wired together. Pay attention to:

- `AddControllers()` (enables controller-based APIs)
- `AddSwaggerGen()` (enables Swagger)
- service registrations (dependency injection), e.g. `AddScoped<...>()`
- `MapControllers()` (connects controller routes to the runtime)

---

## ▶️ How to Run the Project

From the project root:

```bash
dotnet restore
dotnet run
```

When the application starts, your terminal will print something like:

```bash
Now listening on: https://localhost:7xxx
Now listening on: http://localhost:5xxx
```

The port numbers may differ on your machine.

---

## 🧪 How to Test Endpoints Using Swagger

1. Open your web browser and navigate to `http://localhost:5xxx/swagger` (replace `5xxx` with the actual port number printed in your terminal).
2. You should see the Swagger UI, which provides a user-friendly interface to interact with your API.
3. Use the Swagger UI to test your API endpoints by sending requests and viewing responses.

Example responses:

```json
{
  "status": "ok",
  "service": "collage-backend-induction-starter",
  "timestampUtc": "2025-01-01T00:00:00Z"
}
```

If you can successfully call this endpoint, your local setup is complete.

Handle API KEY - keep it personal

1.dotnet add package DotNetEnv

# Initialize user secrets for your project

2. dotnet user-secrets init

# Add your API key

3. dotnet user-secrets set "TmdbSettings:ApiKey" "your_api_key_here"

4. verify by : dotnet user-secrets list
