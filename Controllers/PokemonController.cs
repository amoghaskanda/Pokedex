using Microsoft.AspNetCore.Mvc;
using Pokedex.Models;
using Pokedex.Services;

namespace Pokedex.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly ILogger<PokemonController> _logger;
        private readonly PokemonService _pokemonService;

        public PokemonController(ILogger<PokemonController> logger, PokemonService pokemonService)
        {
            _logger = logger;
            _pokemonService = pokemonService;
        }


        [HttpGet("{name}")]
        [ProducesResponseType<Pokemon>(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromRoute] string name)
        {
            try
            {
                var data = await _pokemonService.BuildPokedex(name);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("translated/{name}")]
        [ProducesResponseType<Pokemon>(StatusCodes.Status200OK)]
        public Task<Pokemon> GetTranslated([FromRoute] string name)
        {
            return _pokemonService.GetTranslated(name);
        }
    }
}
