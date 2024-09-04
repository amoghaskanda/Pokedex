using Microsoft.Extensions.Options;
using Pokedex.Controllers;
using Pokedex.Models;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Web;

namespace Pokedex.Services
{
    public class PokemonService
    {
        private readonly ILogger<PokemonController> _logger;
        private readonly Config _config;
        private readonly JsonSerializerOptions _jsonoptions;
        private static readonly HttpClient _httpClient = new();

        public PokemonService(ILogger<PokemonController> logger, IOptions<Config> config)
        {
            _logger = logger;
            _config = config.Value;
            _jsonoptions = new()
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            };
        }

        public async Task<Species?> GetSpecies(string name)
        {
            var Url = _config.PokemonUrl + name.ToLower();
            var response = await _httpClient.GetAsync(Url);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(response.Content.ToString());
                throw new InvalidOperationException(response.ReasonPhrase);
            }

            var _species = await response.Content.ReadAsStreamAsync();
            return JsonSerializer.Deserialize<Species>(_species, _jsonoptions);
        }

        public async Task<Pokemon> BuildPokedex(string name)
        {
            try
            {
                Species? species = await GetSpecies(name);
                return new Pokemon()
                {
                    Name = species.Name,
                    IsLegendary = species.IsLegendary,
                    Description = species.Description,
                    Habitat = species.Habitat.Name!,
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }

        public async Task<Pokemon> GetTranslated(string name)
        {

            Pokemon pokemon = await BuildPokedex(name);
            if (pokemon.IsLegendary || pokemon.Habitat == "cave")
            {
                pokemon.Description = await YodaTranslation(pokemon.Description);
            }
            else
            {
                pokemon.Description = await ShakespeareTranslation(pokemon.Description);
            }
            return pokemon;
        }

        public async Task<string?> YodaTranslation(string? sentence)
        {
            return await Translate(sentence, _config.YodaUrl!);
        }

        public async Task<string?> ShakespeareTranslation(string? sentence)
        {
            return await Translate(sentence, _config.ShakespeareUrl!);
        }

        private async Task<string?> Translate(string? descriptionCleaned, string Urltype)
        {
            try
            {
                var builder = new UriBuilder(Urltype);
                var query = HttpUtility.ParseQueryString(string.Empty);
                query["text"] = descriptionCleaned;
                builder.Query = query.ToString();

                var response = await _httpClient.GetAsync(builder.ToString());
                string responseBody = await response.Content.ReadAsStringAsync();

                var json = JsonObject.Parse(responseBody);

                return json["contents"]["translated"].ToString();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }
    }
}
