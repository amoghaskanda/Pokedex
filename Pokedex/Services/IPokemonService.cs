using Pokedex.Models;

namespace Pokedex.Services
{
    public interface IPokemonService
    {
        Task<Pokemon> GetPokemon(string name, bool shouldTranslateDescription = false);
        Task<Pokemon> TranslatedPokemonInfo(Pokemon pokemon);
        Task<string?> YodaTranslation(string? sentence);
        Task<string?> ShakespeareTranslation(string? sentence);
        Task<string?> Translate(string? descriptionCleaned, string Url);
        Task<Species?> GetPokemonSpecies(string name);
    }
}
