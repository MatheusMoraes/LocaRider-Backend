using LocaRider.Application.DTO.Motorcycle;
using LocaRider.Application.DTO.Rental;
using LocaRider.Application.Interfaces.Motorcycle;
using LocaRider.Application.Interfaces.Rentals;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LocaRider.API.Controllers
{
    [Route("locacao")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IRentalsService _rentalsService;
        private readonly ILogger<RentalsController> _logger;

        public RentalsController(IRentalsService rentalsService, ILogger<RentalsController> logger)
        {
            _rentalsService = rentalsService;
            _logger = logger;
        }

        /// <summary>
        /// Alugar uma moto
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> CreateRental(RentalDTO rentalDTO)
        {
            _logger.LogInformation("Iniciando CreateRental com RentalDTO: {@RentalDTO}", rentalDTO);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState inválido em CreateRental: {@ModelState}", ModelState);
                return BadRequest(new { mensagem = "Dados inválidos" });
            }

            try
            {
                var createdRental = await _rentalsService.CreateRentalAsync(rentalDTO);

                _logger.LogInformation("Locação criada com sucesso.");
                return Created();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado em CreateRental com RentalDTO: {@RentalDTO}", rentalDTO);
                return StatusCode(500, new { mensagem = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Consultar locação por id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<RentalDTO>> GetRentalById([Required] string id)
        {
            _logger.LogInformation("Iniciando GetRentalById para RentalId: {RentalId}", id);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState inválido em GetRentalById para RentalId: {RentalId}. ModelState: {@ModelState}", id, ModelState);
                return BadRequest(new { mensagem = "Dados inválidos" });
            }

            try
            {
                var rental = await _rentalsService.GetRentalByIdAsync(id);

                if (rental == null)
                {
                    _logger.LogWarning("Locação não encontrada para RentalId: {RentalId}", id);
                    return NotFound("Locação não encontrada.");
                }

                _logger.LogInformation("Locação encontrada com sucesso para RentalId: {RentalId}", id);
                return Ok(rental);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado em GetRentalById para RentalId: {RentalId}", id);
                return StatusCode(500, new { mensagem = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Informar data de devolução e calcular valor
        /// </summary>
        [HttpPut("{id}/devolucao")]
        public async Task<IActionResult> UpdateRentalDevolutionDate([Required] string id, [FromBody][Required] RentalDevolutionDTO rentalDevolutionDTO)
        {
            _logger.LogInformation("Iniciando UpdateRentalDevolutionDate para RentalId: {RentalId} com DTO: {@RentalDevolutionDTO}", id, rentalDevolutionDTO);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState inválido em UpdateRentalDevolutionDate para RentalId: {RentalId}. ModelState: {@ModelState}", id, ModelState);
                return BadRequest(new { mensagem = "Dados inválidos" });
            }

            try
            {
                var updated = await _rentalsService.SetReturnDateAsync(id, rentalDevolutionDTO);

                if (!updated)
                {
                    _logger.LogWarning("Falha ao atualizar data de devolução. Locação não encontrada. RentalId: {RentalId}", id);
                    return NotFound("Locação não encontrada.");
                }

                _logger.LogInformation("Data de devolução informada com sucesso para RentalId: {RentalId}", id);
                return Ok(new { mensagem = "Data de devolução informada com sucesso" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado em UpdateRentalDevolutionDate para RentalId: {RentalId}", id);
                return StatusCode(500, new { mensagem = "Erro interno do servidor" });
            }
        }
    }
}
