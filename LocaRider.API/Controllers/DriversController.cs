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

        public DriversController(IDriversService driversService)
        {
            _driversService = driversService;
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
            if (!ModelState.IsValid)
                return BadRequest(new { mensagem = "Dados inválidos" });

            try
            {
                var driver = await _driversService.CreateDriverAsync(driverDTO);

                if (driver == null)
                    return Conflict(new { mensagem = "CNPJ ou CNH já cadastrado" });

                return CreatedAtAction(nameof(CreateDeliverer), new { id = driver.identificador });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
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
            if (!ModelState.IsValid)
                return BadRequest(new { mensagem = "Dados inválidos" });

            try
            {
                var relativePath = await _driversService.UpdateDriverCnhImageAsync(id, driverBase64ImageDTO);

                return CreatedAtAction(nameof(AddBase64Image), new { id }, new { caminho = relativePath });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { mensagem = "Motorista não encontrado" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }
    }
}