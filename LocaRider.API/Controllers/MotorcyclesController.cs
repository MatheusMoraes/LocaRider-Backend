using LocaRider.API.Utils.Motorcycles;
using LocaRider.Application.DTO.Motorcycle;
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
        private readonly ILogger<MotorcyclesController> _logger;

        public MotorcyclesController(IMotorcyclesService motorcyclesService, ILogger<MotorcyclesController> logger)
        {
            _motorcyclesService = motorcyclesService;
            _logger = logger;
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
            _logger.LogInformation("Iniciando CreateMotorcycle com MotorcycleDTO: {@MotorcycleDTO}", motorcycleDTO);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState inválido em CreateMotorcycle: {@ModelState}", ModelState);
                return BadRequest(new { mensagem = "Dados inválidos" });
            }

            try
            {
                var createdMotorcycle = await _motorcyclesService.CreateMotorcycleAsync(motorcycleDTO);

                if (createdMotorcycle == null)
                {
                    _logger.LogWarning("Falha ao criar moto: placa ou identificador já cadastrado. MotorcycleDTO: {@MotorcycleDTO}", motorcycleDTO);
                    return Conflict("Moto já cadastrada (placa ou identificador).");
                }

                _logger.LogInformation("Moto criada com sucesso.");
                return Created();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado em CreateMotorcycle com MotorcycleDTO: {@MotorcycleDTO}", motorcycleDTO);
                return StatusCode(500, new { mensagem = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Consultar motos existentes
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MotorcycleDTO>>> GetAllMotorcycles([FromQuery] string? placa)
        {
            _logger.LogInformation("Iniciando GetAllMotorcycles. Filtro placa: {Placa}", placa);

            try
            {
                var motorcycles = await _motorcyclesService.GetAllMotorcyclesAsync(placa);
                _logger.LogInformation("GetAllMotorcycles retornou {Count} motos", motorcycles.Count());
                return Ok(motorcycles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado em GetAllMotorcycles. Filtro placa: {Placa}", placa);
                return StatusCode(500, new { mensagem = "Erro interno do servidor" });
            }
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
            _logger.LogInformation("Iniciando UpdateMotorcycle para Id: {MotorcycleId} com DTO: {@MotorcyclePlateDTO}", id, motorcyclePlateDTO);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState inválido em UpdateMotorcycle para Id: {MotorcycleId}. ModelState: {@ModelState}", id, ModelState);
                return BadRequest(new { mensagem = "Dados inválidos" });
            }

            try
            {
                var updated = await _motorcyclesService.UpdateMotorcycleAsync(id, motorcyclePlateDTO);

                if (!updated)
                {
                    _logger.LogWarning("Falha ao atualizar moto. Moto não encontrada ou placa já cadastrada. Id: {MotorcycleId}", id);
                    return NotFound("Moto não encontrada ou placa já cadastrada");
                }

                _logger.LogInformation("Moto atualizada com sucesso. Id: {MotorcycleId}", id);
                return Ok(new { mensagem = "Placa modificada com sucesso" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado em UpdateMotorcycle para Id: {MotorcycleId}", id);
                return StatusCode(500, new { mensagem = "Erro interno do servidor" });
            }
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
            _logger.LogInformation("Iniciando GetMotorcycleById para Id: {MotorcycleId}", id);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState inválido em GetMotorcycleById para Id: {MotorcycleId}. ModelState: {@ModelState}", id, ModelState);
                return BadRequest(new { mensagem = "Request mal formada" });
            }

            try
            {
                var motorcycle = await _motorcyclesService.GetMotorcycleByIdAsync(id);

                if (motorcycle == null)
                {
                    _logger.LogWarning("Moto não encontrada em GetMotorcycleById para Id: {MotorcycleId}", id);
                    return NotFound("Moto não encontrada");
                }

                _logger.LogInformation("Moto encontrada com sucesso para Id: {MotorcycleId}", id);
                return Ok(motorcycle);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado em GetMotorcycleById para Id: {MotorcycleId}", id);
                return StatusCode(500, new { mensagem = "Erro interno do servidor" });
            }
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
            _logger.LogInformation("Iniciando DeleteMotorcycle para Id: {MotorcycleId}", id);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState inválido em DeleteMotorcycle para Id: {MotorcycleId}. ModelState: {@ModelState}", id, ModelState);
                return BadRequest(new { mensagem = "Dados inválidos" });
            }

            try
            {
                var deleted = await _motorcyclesService.DeleteMotorcycleAsync(id);

                if (!deleted)
                {
                    _logger.LogWarning("Falha ao deletar moto. Moto não encontrada. Id: {MotorcycleId}", id);
                    return NotFound("Moto não encontrada");
                }

                _logger.LogInformation("Moto deletada com sucesso. Id: {MotorcycleId}", id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado em DeleteMotorcycle para Id: {MotorcycleId}", id);
                return StatusCode(500, new { mensagem = "Erro interno do servidor" });
            }
        }
    }
}
