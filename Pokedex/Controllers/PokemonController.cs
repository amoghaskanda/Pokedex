using Microsoft.AspNetCore.Mvc;
using Pokedex.Services;

namespace Pokedex.Controllers
{
    [ApiController]
    [Route("pokemon")]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonService _pokemonService;

        public PokemonController(IPokemonService pokemonService)
        {
            _pokemonService = pokemonService;
        }


        [HttpGet("{name}")]
        public async Task<IActionResult> Get([FromRoute] string name)
        {
            try
            {
                var data = await _pokemonService.GetPokemon(name);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("translated/{name}")]
        public async Task<IActionResult> GetTranslated([FromRoute] string name)
        {
            try
            {
                var data = await _pokemonService.GetPokemon(name, shouldTranslateDescription: true);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}