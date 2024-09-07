# Pokedex API

This project is a .NET Core application written in C#. It presents endpoint to get a simplified Pokedex with options to translate description.
## Prerequisites

This sample is written in C# and targets .NET Core 8.0. It requires the [.NET Core 8.0 SDK](https://dotnet.microsoft.com/en-us/download).
## Project structure
```console
.
├── ...
├── README.md
├── Pokedex                    
│   ├── Pokedex.sln                     # Project solution.
│   ├── Pokedex.csproj
│   ├── Dockerfile
│   ├── Controllers
│   │    └── ...  
│   ├── Models
│   │    └── ...
│   ├── Services
│   │    └── ...
├── Pokedex.Tests
│   ├── Pokedex.Tests.csproj
│   ├── PokemonServiceTests.Cs          # Unit tests
│   └── ...
└── ...
```
## Run Tests
```console
cd ./Pokedex/
dotnet test
```

## Build and Run Solution

- Terminal:
```console
dotnet build
dotnet run
```
- Visual Studio
    - Open Pokedex.sln in Visual Studio
    - Testing: Test > Rull all tests
    - Run: Open Debug > Start Debugging  
## Run
To interact with the API, use [Swagger](http://localhost:5000/swagger)