using FluentAssertions;
using Pokedex.Models;
using Pokedex.Services;
using System.Collections;
using System.Net;
using System.Text.Json;

namespace Pokedex.Tests
{
    public class PokemonServiceTests
    {
        public PokemonServiceTests()
        { }

        [Theory]
        [ClassData(typeof(GetPokemonValidTestCases))]
        public async Task GetPokemon_ValidTestCase(string name, bool shouldTranslateDescription, Species values, Pokemon expected)
        {
            var response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(values, Settings.CustomJsonOptions), System.Text.Encoding.UTF8, "application/json")
            };
            var mockHttpMessageHandler = new MockMessageHandler(response);

            var client = new HttpClient(mockHttpMessageHandler);
            var PokemonService = new PokemonService(client);

            var result = await PokemonService.GetPokemon(name, shouldTranslateDescription);

            result.Should().NotBeNull();
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(" ")]
        [InlineData(" 23131232")]
        [InlineData("R@ichu")]
        [InlineData("<script><body onload=alert('pwned')></script>")]
        public async Task GetPokemon_InvalidNames(string name)
        {
            var response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
            };
            var mockHttpMessageHandler = new MockMessageHandler(response);

            var client = new HttpClient(mockHttpMessageHandler);
            var PokemonService = new PokemonService(client);

            Func<Task> BuildPokedex = () => PokemonService.GetPokemon(name);
            await BuildPokedex.Should().ThrowAsync<InvalidOperationException>().WithMessage("Not Found");
        }

        [Theory]
        [InlineData(Catalog.someString, Catalog.someStringYodaTranslated)]
        [InlineData(Catalog.anotherString, Catalog.anotherStringYodaTranslated)]
        [InlineData(" ", "")]
        public async Task YodaTranslation_Valid(string description, string expected)
        {
            object responseFromApi = new { contents = new { translated = expected } };

            var response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(responseFromApi), System.Text.Encoding.UTF8, "application/json")
            };
            var mockHttpMessageHandler = new MockMessageHandler(response);

            var client = new HttpClient(mockHttpMessageHandler);
            var PokemonService = new PokemonService(client);

            var result = await PokemonService.YodaTranslation(description);
            result.Should().NotBeNull();
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(Catalog.someString, Catalog.anotherString)]
        [InlineData(Catalog.someString, Catalog.anotherStringShakespeareTranslated)]
        [InlineData(Catalog.anotherString, Catalog.someStringYodaTranslated)]
        public async Task YodaTranslation_Invalid(string description, string expected)
        {
            object responseFromApi = new { contents = new { translated = expected } };

            var response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(responseFromApi), System.Text.Encoding.UTF8, "application/json")
            };
            var mockHttpMessageHandler = new MockMessageHandler(response);

            var client = new HttpClient(mockHttpMessageHandler);
            var PokemonService = new PokemonService(client);

            var result = await PokemonService.YodaTranslation(description);
            result.Should().NotBeNull();
            result.Should().NotBeSameAs(expected);
        }

        [Theory]
        [InlineData(Catalog.someString, Catalog.someStringShakespeareTranslated)]
        [InlineData(Catalog.anotherString, Catalog.anotherStringShakespeareTranslated)]
        [InlineData(Catalog.whiteSpace, Catalog.blank)]
        public async Task ShakespeareTranslation_Valid(string description, string expected)
        {
            object responseFromApi = new { contents = new { translated = expected } };

            var response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(responseFromApi), System.Text.Encoding.UTF8, "application/json")
            };
            var mockHttpMessageHandler = new MockMessageHandler(response);

            var client = new HttpClient(mockHttpMessageHandler);
            var PokemonService = new PokemonService(client);

            var result = await PokemonService.ShakespeareTranslation(description);
            result.Should().NotBeNull();
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(Catalog.someString, Catalog.anotherString)]
        [InlineData(Catalog.someString, Catalog.anotherStringShakespeareTranslated)]
        [InlineData(Catalog.anotherString, Catalog.someStringYodaTranslated)]
        public async Task ShakespeareTranslation_Invalid(string description, string expected)
        {
            object responseFromApi = new { contents = new { translated = expected } };

            var response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(responseFromApi), System.Text.Encoding.UTF8, "application/json")
            };
            var mockHttpMessageHandler = new MockMessageHandler(response);

            var client = new HttpClient(mockHttpMessageHandler);
            var PokemonService = new PokemonService(client);

            var result = await PokemonService.ShakespeareTranslation(description);
            result.Should().NotBeNull();
            result.Should().NotBeSameAs(expected);
        }

    }

    public class GetPokemonValidTestCases : IEnumerable<object[]>
    {
        public static readonly List<object[]> data = new()
        {
            new object[] { "mewtwo", false, Catalog.mewtwoSpecies, Catalog.mewtwo },
            new object[] { "venusaur", false, Catalog.venusaurSpecies, Catalog.venusaur }
        };

        public IEnumerator<object[]> GetEnumerator()
        {
            return data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    public class GetPokemonInvalidTestCases : IEnumerable<object[]>
    {
        public static readonly List<object[]> data = new()
        {
            new object[] { "mewtwo", Catalog.mewtwoSpecies, Catalog.mewtwo },
            new object[] { "venusaur", Catalog.venusaurSpecies, Catalog.venusaur }
        };

        public IEnumerator<object[]> GetEnumerator()
        {
            return data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    public static class Catalog
    {
        public static readonly Pokemon mewtwo = new()
        {
            Name = "mewtwo",
            Description = "It was created by a scientist after years of horrific gene splicing and DNA engineering experiments.",
            Habitat = "rare",
            IsLegendary = true,
        };

        public static readonly Species mewtwoSpecies = new()
        {
            Name = "mewtwo",
            Habitat = new HabitatEntry() { HabitatName = "rare" },
            FlavorTextEntries = [new FlavorTextEntry() { FlavorText = "It was created by\na scientist after\nyears of horrific\u000cgene splicing and\nDNA engineering\nexperiments." }],
            IsLegendary = true
        };

        public static readonly Pokemon venusaur = new()
        {
            Name = "venusaur",
            Description = "The plant blooms when it is absorbing solar energy. It stays on the move to seek sunlight.",
            Habitat = "grassland",
            IsLegendary = false,
        };

        public static readonly Species venusaurSpecies = new()
        {
            Name = "venusaur",
            FlavorTextEntries = [new FlavorTextEntry() { FlavorText = "The plant blooms\nwhen it is\nabsorbing solar\u000cenergy. It stays\non the move to\nseek sunlight." }],
            Habitat = new HabitatEntry { HabitatName = "grassland" },
            IsLegendary = false
        };

        public static readonly Pokemon venusaurTranslatedDescription = new()
        {
            Name = "venusaur",
            Description = "The plant blooms at which hour 't is absorbing solar energy. 't stays on the moveth to seek sunlight.",
            Habitat = "grassland",
            IsLegendary = false,
        };
        public static readonly Pokemon mewtwoTranslatedDescription = new()
        {
            Name = "mewtwo",
            Description = "Created by a scientist after years of horrific gene splicing and dna engineering experiments,  it was.",
            Habitat = "rare",
            IsLegendary = true,
        };

        public const string someString = "It is warm on a warm day";
        public const string someStringYodaTranslated = "Warm on a warm day,  it is";
        public const string someStringShakespeareTranslated = "'t is warm on a warm day";
        public const string anotherString = "Fly me to the moon. Let me play among the stars.";
        public const string anotherStringYodaTranslated = "Fly me to the moon.Me play among the stars,  let";
        public const string anotherStringShakespeareTranslated = "Fly me to the moon. Did let me playeth 'mongst the stars.";
        public const string whiteSpace = " ";
        public const string blank = "";


    }
}