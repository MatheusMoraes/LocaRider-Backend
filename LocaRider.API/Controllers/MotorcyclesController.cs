using LocaRider.API.Utils.Motorcycles;
using LocaRider.Application.DTO.Motorcycle;
using LocaRider.Application.DTO.Users;
using LocaRider.Application.Interfaces.Motorcycle;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.ComponentModel.DataAnnotations;

namespace LocaRider.API.Controllers
{
    [Route("motos")]
    [ApiController]
    public class MotorcyclesController : ControllerBase
    {

        private readonly IMotorcyclesService _motorcyclesService;

        public MotorcyclesController(IMotorcyclesService motorcyclesService)
        {
            _motorcyclesService = motorcyclesService;
        }

        /// <summary>
        /// Cadastrar uma nova moto
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        [SwaggerRequestExample(typeof(MotorcycleDTO), typeof(MotorcyclesObjectExample))]
        public async Task<ActionResult> CreateMotorcycle(MotorcycleDTO motorcycleDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { mensagem = "Dados Inválidos" });

            var createdMotorcyle = await _motorcyclesService.CreateMotorcycleAsync(motorcycleDTO);

            if (createdMotorcyle == null)
                return Conflict("Moto já cadastrada (placa ou identificador).");

            return Created();
        }

        /// <summary>
        /// Consultar motos existentes
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MotorcycleDTO>>> GetAllMotorcycles([FromQuery] string? placa)
        {
            var motorcycles = await _motorcyclesService.GetAllMotorcyclesAsync(placa);
            return Ok(motorcycles);
        }

        /// <summary>
        /// Modificar a placa de uma moto
        /// </summary>
        [HttpPut("{id}/placa")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(IDictionary<string, string[]>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerRequestExample(typeof(MotorcyclePlateDTO), typeof(MotorcyclesPlateExample))]
        public async Task<IActionResult> UpdateMotorcycle([Required] string id, [FromBody] MotorcyclePlateDTO motorcyclePlateDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { mensagem = "Dados inválidos" });

            var updated = await _motorcyclesService.UpdateMotorcycleAsync(id, motorcyclePlateDTO);
            if (!updated) return NotFound("Moto não encontrada ou placa já cadastrada");

            return Ok(new { mensagem = "Placa modificada com sucesso" });
        }

        /// <summary>
        /// Consultar motos existentes por id
        /// </summary>
        [HttpGet("{id}")]
        [SwaggerResponse(StatusCodes.Status200OK, "Detalhes do usuário", typeof(MotorcycleDTO))]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [SwaggerRequestExample(typeof(MotorcycleDTO), typeof(MotorcyclesObjectExample))]
        public async Task<ActionResult<MotorcycleDTO>> GetMotorcycleById([Required] string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { mensagem = "Request mal formada" });

            var motorcycle = await _motorcyclesService.GetMotorcycleByIdAsync(id);
            if (motorcycle == null) return NotFound("Moto não encontrada");
            return Ok(motorcycle);
        }

        /// <summary>
        /// Remover uma moto
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteMotorcycle([Required] string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { mensagem = "Dados inválidos" });

            var deleted = await _motorcyclesService.DeleteMotorcycleAsync(id);
            if (!deleted) return NotFound("Moto não encontrado");

            return Ok();
        }
    }

}
