using LocaRider.API.Utils.Drivers;
using LocaRider.API.Utils.Motorcycles;
using LocaRider.Application.DTO.Driver;
using LocaRider.Application.DTO.Motorcycle;
using LocaRider.Application.Interfaces.Drivers;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using System.ComponentModel.DataAnnotations;

namespace LocaRider.API.Controllers
{
    [Route("entregadores")]
    [ApiController]
    public class DriversController : ControllerBase
    {
        private readonly IDriversService _driversService;
        private readonly ILogger<DriversController> _logger;

        public DriversController(IDriversService driversService, ILogger<DriversController> logger)
        {
            _driversService = driversService;
            _logger = logger;
        }

        /// <summary>
        /// Cadastrar entregador
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerRequestExample(typeof(DriverDTO), typeof(DriversExample))]
        public async Task<ActionResult> CreateDeliverer([FromBody] DriverDTO driverDTO)
        {
            _logger.LogInformation("Iniciando CreateDeliverer com DriverDTO: {@DriverDTO}", driverDTO);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState inválido em CreateDeliverer: {@ModelState}", ModelState);
                return BadRequest(new { mensagem = "Dados inválidos" });
            }

            try
            {
                var driver = await _driversService.CreateDriverAsync(driverDTO);

                if (driver == null)
                {
                    _logger.LogWarning("Falha ao criar driver: CNPJ ou CNH já cadastrado. DriverDTO: {@DriverDTO}", driverDTO);
                    return Conflict(new { mensagem = "CNPJ ou CNH já cadastrado" });
                }

                _logger.LogInformation("Driver criado com sucesso. ID: {DriverId}", driver.identificador);
                return CreatedAtAction(nameof(CreateDeliverer), new { id = driver.identificador });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Erro em CreateDeliverer com DriverDTO: {@DriverDTO}", driverDTO);
                return BadRequest(new { mensagem = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado em CreateDeliverer com DriverDTO: {@DriverDTO}", driverDTO);
                return StatusCode(500, new { mensagem = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Enviar foto da CNH
        /// </summary>
        [HttpPost("{id}/cnh")]
        [SwaggerRequestExample(typeof(DriverBase64ImageDTO), typeof(DriversCnhImageExample))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> AddBase64Image([Required] string id, [FromBody][Required] DriverBase64ImageDTO driverBase64ImageDTO)
        {
            _logger.LogInformation("Iniciando AddBase64Image para DriverId: {DriverId}", id);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState inválido em AddBase64Image para DriverId: {DriverId}. ModelState: {@ModelState}", id, ModelState);
                return BadRequest(new { mensagem = "Dados inválidos" });
            }

            try
            {
                var relativePath = await _driversService.UpdateDriverCnhImageAsync(id, driverBase64ImageDTO);
                _logger.LogInformation("Imagem CNH atualizada com sucesso para DriverId: {DriverId}, Path: {Path}", id, relativePath);

                return CreatedAtAction(nameof(AddBase64Image), new { id }, new { caminho = relativePath });
            }
            catch (KeyNotFoundException)
            {
                _logger.LogWarning("Driver não encontrado em AddBase64Image para DriverId: {DriverId}", id);
                return NotFound(new { mensagem = "Motorista não encontrado" });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Erro em AddBase64Image para DriverId: {DriverId}", id);
                return BadRequest(new { mensagem = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado em AddBase64Image para DriverId: {DriverId}", id);
                return StatusCode(500, new { mensagem = "Erro interno do servidor" });
            }
        }
    }
}