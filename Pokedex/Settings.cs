using System.Text.Json;

namespace Pokedex
{
    public class Settings
    {
        public const string PokemonSpeciesUrl = "https://pokeapi.co/api/v2/pokemon-species/";
        public const string YodaTranslationUrl = "https://api.funtranslations.com/translate/yoda.json";
        public const string ShakespeareTranslationUrl = "https://api.funtranslations.com/translate/shakespeare.json";
        public static readonly JsonSerializerOptions CustomJsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };
    }
}
