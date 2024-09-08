# Pokedex API

This project is a .NET Core application written in C#. It presents endpoint to get a simplified Pokedex with options to translate description.
## Prerequisites

The project is written in C# and targets .NET Core 8.0. It requires the [.NET Core 8.0 SDK](https://dotnet.microsoft.com/en-us/download). Additionally, [Visual Studio](https://visualstudio.microsoft.com/thank-you-downloading-visual-studio/?sku=Community&channel=Release&version=VS2022&source=VSLandingPage&cid=2030&passive=false) is recommended for a smoother experience.
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
## Installation and Usage

```console
# Clone the repository
git clone https://github.com/amoghaskanda/Pokedex.git

# Enter Project Directory
cd Pokedex/Pokedex

# Build Solution
dotnet build

# Run Unit Tests
dotnet test

# Run Solution
dotnet run
```
Alternatively, open ```Pokedex.sln``` using Visual Studio and run Debug > Start to launch the swagger interface.
The Unit Tests can be run from Test > Run All Tests
## Run
To interact with the API, use [Swagger](http://localhost:5000/swagger/index.html)
#### Available Endpoints
```console
[1]: /pokemon/{name}
[2]: /pokemon/translated/{name}
```

