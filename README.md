# Minimal API with MongoDB

A simple ASP.NET Core Minimal API with full CRUD operations, backed by MongoDB. Built as part of Modul 165/347 at GBSSG.

## Features

- REST API with GET, POST, PUT, DELETE endpoints for movies
- MongoDB persistence via the official C# driver
- Service layer using dependency injection (`IMovieService` / `MongoMovieService`)
- Runs locally with `dotnet run` or fully containerized with Docker Compose

## Tech Stack

- .NET 8 / ASP.NET Core Minimal API
- MongoDB
- Docker & Docker Compose

## Project Structure

```
min-api-with-mongo/
├── WebApi/
│   ├── Program.cs            # Endpoints & DI setup
│   ├── IMovieService.cs      # Service interface
│   ├── MongoMovieService.cs  # MongoDB implementation
│   ├── Movie.cs              # Movie model
│   └── DatabaseSettings.cs  # Connection config
├── docker-compose.yml
└── TestRequests.http         # HTTP test requests (REST Client)
```

## Getting Started

### With Docker (recommended)

```bash
docker compose up --build
```

API is available at `http://localhost:5001`.

### Locally

Start only the MongoDB container, then run the API separately:

```bash
docker compose up mongodb
dotnet run --project WebApi
```

## API Endpoints

| Method | Endpoint | Description | Success | Error |
|--------|----------|-------------|---------|-------|
| GET | `/` | Version info | 200 | — |
| GET | `/check` | MongoDB connection check | 200 | — |
| GET | `/api/movies` | Get all movies | 200 | — |
| GET | `/api/movies/{id}` | Get movie by ID | 200 | 404 |
| POST | `/api/movies` | Create a movie | 201 | 409 |
| PUT | `/api/movies/{id}` | Update a movie | 200 | 404 |
| DELETE | `/api/movies/{id}` | Delete a movie | 204 | 404 |

## Example Request

```http
POST http://localhost:5001/api/movies
Content-Type: application/json

{
  "id": "1",
  "title": "The Imitation Game",
  "year": 2014,
  "summary": "Alan Turing knackt den Enigma-Code.",
  "actors": ["Benedict Cumberbatch", "Keira Knightley"]
}
```
