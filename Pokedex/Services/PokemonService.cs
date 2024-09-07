using Pokedex.Models;
using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Web;

namespace Pokedex.Services
{

    public class PokemonService : IPokemonService
    {
        public HttpClient _httpClient;
        public PokemonService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<Pokemon> GetPokemon(string name, bool shouldTranslateDescription = false)
        {
            var species = await GetPokemonSpecies(name);
            var pokemon = new Pokemon()
            {
                Name = species?.Name,
                IsLegendary = species?.IsLegendary,
                Description = species?.Description,
                Habitat = species?.Habitat!.HabitatName,
            };
            if (shouldTranslateDescription)
                return await TranslatedPokemonInfo(pokemon);
            return pokemon;

        }

        public async Task<Pokemon> TranslatedPokemonInfo(Pokemon pokemon)
        {

            if (pokemon.IsLegendary!.Value || pokemon.Habitat == "cave")
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
            return await Translate(sentence, Settings.YodaTranslationUrl);
        }

        public async Task<string?> ShakespeareTranslation(string? sentence)
        {
            return await Translate(sentence, Settings.ShakespeareTranslationUrl);
        }

        public async Task<string?> Translate(string? descriptionCleaned, string Url)
        {
            try
            {
                var builder = new UriBuilder(Url);
                var query = HttpUtility.ParseQueryString(string.Empty);
                query["text"] = descriptionCleaned;
                builder.Query = query.ToString();

                var response = await _httpClient.GetAsync(builder.ToString());
                string responseBody = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == HttpStatusCode.TooManyRequests)
                    throw new HttpRequestException(response.ReasonPhrase);
                var json = JsonObject.Parse(responseBody);

                return json["contents"]["translated"].ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<Species?> GetPokemonSpecies(string name)
        {
            try
            {
                string Url = Settings.PokemonSpeciesUrl + name.ToLower();
                var response = await _httpClient.GetAsync(Url);
                var stream = await response.Content.ReadAsStreamAsync();
                if (!response.IsSuccessStatusCode)
                    throw new InvalidOperationException(response.ReasonPhrase);
                return JsonSerializer.Deserialize<Species?>(stream, Settings.CustomJsonOptions);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
